using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Middleware for handling interaction between the app and firebase
/// </summary>
[CreateAssetMenu(fileName = "new Firebase manager",menuName ="Dependencies/FirebaseManager")]
public class FirebaseUser : ScriptableObject 
{
    [Header("Firebase data")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public Dictionary<string, object> userData = new Dictionary<string, object>();
    public Firebase.Auth.FirebaseUser user;
    private DatabaseReference rootReference;

    /// <summary>
    /// Creates an Instance of the firebase realtime database
    /// </summary>
    public void StartFirebaseService()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
                InitializeFirebaseService();
            else
                Debug.LogError($"Error with firebase dependencies: {dependencyStatus}");
        });
    }


    //Setting up the service
    private void InitializeFirebaseService()
    {
        Debug.Log("Initializing firebase service");
        auth = FirebaseAuth.DefaultInstance;
        rootReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    #region Auth
    /// <summary>
    /// Firebase Auth method for login to the app
    /// </summary>
    /// <param name="email">user's email</param>
    /// <param name="password">user's password</param>
    /// <param name="callback">callback to let the invoker know the status of the petition. Params are: title, description and type of the alert</param>
    /// <returns></returns>
    public IEnumerator Login(string email, string password, UnityAction<string,string,AlertPanel.AlertType> callback)
    {
        //calling the firebase login service
        Task<Firebase.Auth.FirebaseUser> task = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        //When completed check if any error ocurred
        if (task.Exception != null)
        {
            AuthErrorHandler(task, callback);//Handle the error
        }
        else
        {
            user = task.Result;
            //gettiing all user data from the realtime database
            Task<DataSnapshot> userDataTask = rootReference.Child("users").Child(user.UserId).GetValueAsync();
            yield return new WaitUntil(predicate: () => userDataTask.IsCompleted);
            using (var enumerable = userDataTask.Result.Children.GetEnumerator())
            {
                //populating userData dictory for local uses
                while (enumerable.MoveNext())
                    userData[enumerable.Current.Key] = enumerable.Current.Value;
            }

            Debug.Log($"Login successfull");
            callback("", "", AlertPanel.AlertType.Info);
        }
    }
    /// <summary>
    /// Firebase method for register a new user via Auth but also creating a new node in the "users" tree
    /// </summary>
    /// <param name="userName">User's display name</param>
    /// <param name="email">User's email</param>
    /// <param name="password">User's password</param>
    /// <param name="callback">callback to let the invoker know the status of the petition. Params are: title, description and type of the alert</param>
    /// <returns></returns>
    public IEnumerator SignUp(string userName,string email, string password, UnityAction<string, string, AlertPanel.AlertType> callback)
    {
        //Creating a new user
        Task<Firebase.Auth.FirebaseUser> task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        //When completed check for errors
        if (task.Exception != null)
        {
            AuthErrorHandler(task, callback);//Handling errors if any
        }
        else
        {
            user = task.Result;//Setting the user in the scriptable object
            /**Setting displayname of the user**/
            UserProfile userProfile = new UserProfile();
            userProfile.DisplayName = userName;
            Task nameTask = user.UpdateUserProfileAsync(userProfile);
            yield return new WaitUntil(predicate: () => nameTask.IsCompleted);
            /**Creating a new node in historials tree to store the user matches**/
            string key = rootReference.Child("historials").Push().Key;
            /**Creating a new node in "users" tree to store new user data**/
            Dictionary<string, object> userUpdates = new Dictionary<string, object>();
            userUpdates.Add("/users/"+user.UserId+"/numMatches", 0);
            userUpdates.Add("/users/" + user.UserId + "/numWins", 0);
            userUpdates.Add("/users/" + user.UserId + "/winRate", 0f);
            userUpdates.Add("/users/" + user.UserId + "/idHistorial", key);
            //populating user data dictionary for local uses
            userData["numMatches"] = 0;
            userData["numWins"] = 0;
            userData["winRate"] = 0f;
            userData["idHistorial"] = key;

            //Updating changes in firebase
            Task updateTask = rootReference.UpdateChildrenAsync(userUpdates);
            yield return new WaitUntil(predicate: () => updateTask.IsCompleted);

            DatabaseErrorHandler(updateTask, callback, "Singup successfull");
        }
    }
    #endregion
    #region Realtime Database
    /// <summary>
    /// Creates new match data on client side, storing the changes on the referenced dictionary
    /// </summary>
    /// <param name="refChangesDictionary">dictionary to add changes</param>
    /// <param name="winner">winner of the match</param>
    /// <param name="time">time spended</param>
    /// <param name="date">date of the match</param>
    /// <param name="replay">list of moves</param>
    /// <param name="wasOnline">wether was an online or local match</param>
    /// <param name="players">list of userId and color of each online player</param>
    /// <returns>the firebase key of the match</returns>
    public string CreateMatch(ref Dictionary<string,object> refChangesDictionary, string winner,float time,DateTime date,List<string> replay,bool wasOnline, Dictionary<string, Dictionary<string, object>> players = null)
    {
        //Creating new match key
        string matchKey = rootReference.Child("matches").Push().Key;
        //storing changes
        refChangesDictionary.Add("/matches/" + matchKey + "/time", time);
        refChangesDictionary.Add("/matches/" + matchKey + "/date", date.ToString("MM-dd-yyyy"));
        refChangesDictionary.Add("/matches/" + matchKey + "/replay", replay);
        refChangesDictionary.Add("/matches/" + matchKey + "/online", wasOnline);
        //Store players only when the match was online
        if (wasOnline)
            refChangesDictionary.Add("/matches/" + matchKey + "/players", players);
        else
            refChangesDictionary.Add("/matches/" + matchKey + "/winner", winner);
        return matchKey;
    }
    /// <summary>
    /// Update all changes stored in the dictionary given
    /// </summary>
    /// <param name="changes">Dictionary of changes</param>
    /// <param name="callback">delegate function called when the process is complete</param>
    /// <returns>Waits until the petition is complete</returns>
    public IEnumerator UpdateChanges(Dictionary<string,object> changes, UnityAction<string, string, AlertPanel.AlertType> callback)
    {
        Task updateTask = rootReference.UpdateChildrenAsync(changes);
        yield return new WaitUntil(predicate: () => updateTask.IsCompleted);

        DatabaseErrorHandler(updateTask, callback, "Changes completed successfully");
    }
    /// <summary>
    /// Gets historial key from user data and append match on that historial node
    /// </summary>
    /// <param name="id">userKey</param>
    /// <param name="matchKey">matchKey</param>
    /// <param name="callback">callback function</param>
    /// <returns></returns>
    public IEnumerator UpdateHistorial(UnityAction<string, string, AlertPanel.AlertType> callback, string userKey,string matchKey, string historialKey = null)
    {
        historialKey = historialKey == null ? userData["idHistorial"].ToString() : historialKey;
        Task updateTask = rootReference.Child("historials").Child(historialKey).Child(matchKey).SetValueAsync(true);
        yield return new WaitUntil(predicate: () => updateTask.IsCompleted);
        DatabaseErrorHandler(updateTask, callback, "Match added to HistorialKey given");
    }
    /// <summary>
    /// update THIS USER stats such as number of matches, wins and winrate
    /// </summary>
    /// <param name="isWin">if this player won the match</param>
    /// <param name="callback">callback function</param>
    /// <returns></returns>
    public IEnumerator UpdatePlayerStats(bool isWin, UnityAction<string, string, AlertPanel.AlertType> callback)
    {
        Dictionary<string, object> changes = new Dictionary<string, object>();
        int numWins = Convert.ToInt32(userData["numWins"]);
        int numMatches = Convert.ToInt32(userData["numMatches"]);
        if (isWin)
            changes.Add("/users/" + user.UserId + "/numWins", ++numWins);
        changes.Add("/users/" + user.UserId + "/numMatches", ++numMatches);
        changes.Add("/users/" + user.UserId + "/winRate", ((float)numWins) / ((float)numMatches));
        yield return UpdateChanges(changes, callback);
    }
    /// <summary>
    /// With the historial key given search in firebase the list of matches
    /// for each matchkey gets the match data and creates a dictionary representation if the data
    /// </summary>
    /// <param name="callback">Delegate function called when all the dictionaries are created, the function needs a FirebaseException and the list of dictionaries</param>
    /// <param name="idHistorial">historial key, which we'll find the matches of</param>
    /// <returns>An IEnumerator</returns>
    public IEnumerator GetMatchesList(UnityAction<FirebaseException,List<Dictionary<string,object>>> callback,string idHistorial)
    {
        //list of dictionry representations of the matches
        List<Dictionary<string, object>> matches = new List<Dictionary<string,object>>();
        //getting all the match keys related with the idHistorial
        Task<DataSnapshot> task = rootReference.Child("historials").Child(idHistorial).GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (!task.IsFaulted)
        {
            DataSnapshot data = task.Result;
            using (var enumerator = data.Children.GetEnumerator())
            {
                //for each matchKey get that match's data
                while (enumerator.MoveNext())
                {
                    //Getting the match node
                    Task<DataSnapshot> matchTask = rootReference.Child("matches").Child(enumerator.Current.Key).GetValueAsync();
                    yield return new WaitUntil(predicate: () => matchTask.IsCompleted);
                    if(!task.IsFaulted)
                    {
                        Dictionary<string, object> match = new Dictionary<string, object>();//dictoriary representation of the match
                        match["id"] = enumerator.Current.Key;//storing key for future uses
                        using (var matchEnum = matchTask.Result.Children.GetEnumerator())
                        {
                            //Converting the DataSnapshot into a dictionary
                            while (matchEnum.MoveNext())
                                match[matchEnum.Current.Key] = matchEnum.Current.Value;
                        }
                        matches.Add(match);
                    }
                }
            }
            callback(null, matches);//Once the process is complete call the callback with a null exception and the list of matches
        }
        else
        {
            Debug.LogError(task.Exception.Message);
            //if any error call the callback passing the firebase Exception to be handled in the callback
            callback(task.Exception.GetBaseException() as FirebaseException, matches);
        }
    }
    #endregion
    #region EventHandlers
    /// <summary>
    /// Method that spawns an popup alert depending of the error given
    /// </summary>
    /// <param name="task">Firebase task to be handled</param>
    /// <param name="callback">callback to let the invoker know the status of the petition. Params are: title, description and type of the alert</param>
    private void AuthErrorHandler(Task<Firebase.Auth.FirebaseUser> task, UnityAction<string, string, AlertPanel.AlertType> callback)
    {
            Debug.Log($"Auth error: {task.Exception}");
            FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    callback("Invalid Email", "Pleace rewrite your email", AlertPanel.AlertType.Error);
                    break;
                case AuthError.UserNotFound:
                    callback("User not found", "The user given does not exist", AlertPanel.AlertType.Error);
                    break;
                case AuthError.WrongPassword:
                    callback("Wrong password", "The password does not match with the email given", AlertPanel.AlertType.Error);
                    break;
                case AuthError.EmailAlreadyInUse:
                    callback("Email already in use", "Seems there is already an account with the email given", AlertPanel.AlertType.Error);
                    break;
                case AuthError.MissingEmail:
                    callback("Missing email", "Please write the email", AlertPanel.AlertType.Error);
                    break;
                case AuthError.MissingPassword:
                    callback("Missing password", "Please write a password", AlertPanel.AlertType.Error);
                    break;
                default:
                    callback("Error", "Something went wrong", AlertPanel.AlertType.Error);
                    break;
            }
    }

    private void DatabaseErrorHandler(Task task, UnityAction<string, string, AlertPanel.AlertType> callback,string successMessage = null)
    {
        //handling errors
        if (task.Exception != null)
        {
            FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
            Debug.LogError($"Exception: {task.Exception.Message} with code {firebaseException.ErrorCode}");
        }
        else if(successMessage != null)
        {
            Debug.Log(successMessage);
            callback("", "", AlertPanel.AlertType.Info);//Indicate a successfull operation
        }
    }

    #endregion
}
