using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Fills the ScrollView with prefabs representing matches of the user
/// </summary>
public class FillUserMatchList : MonoBehaviour
{
    public GameObject matchListContainer;//gameobject where to add the matches
    public GameObject matchUIItem;//prefab which represents a patch
    public FirebaseUser firebase;//firebase middleware
    public GlobalData opponentData;
    public GameObject matchDataPanel;//match data panel
    // Start is called before the first frame update
    private void OnEnable()
    {
        //getting all matches of the user
        StartCoroutine(firebase.GetMatchesList(OnTaskFinished,firebase.userData["idHistorial"].ToString()));
        gameObject.SetActive(true);
    }
    /// <summary>
    /// Callback called when the coroutine ends
    /// Instantiate and fill prefabs with data needed
    /// and add them in the container
    /// </summary>
    /// <param name="ex">Firebase Exception to catch posible errors</param>
    /// <param name="matches">List of matches</param>
    private void OnTaskFinished(Firebase.FirebaseException ex, List<Dictionary<string,object>> matches)
    {
        if (ex == null)//if there is no error
        {
            //Instantiating a matchUIItem for each match in the list
            foreach(Dictionary<string,object> match in matches)
            {
                GameObject buttonPrefab = Instantiate(matchUIItem);
                //Filling data in the prefab
                if ((bool)match["online"])
                {
                    buttonPrefab.GetComponent<MatchButtonData>().ActivateOnlineMatchButton(match, firebase.user.UserId,matchDataPanel.GetComponent<MatchDataPanel>());
                }
                else
                    buttonPrefab.GetComponent<MatchButtonData>().ActivateLocalMatchButton(match, matchDataPanel.GetComponent<MatchDataPanel>());
                //making te container parent of the new Button
                buttonPrefab.transform.SetParent(matchListContainer.transform);
                buttonPrefab.transform.localScale = Vector3.one;
                buttonPrefab.transform.localPosition = Vector3.zero;
            }

        }
        else
        {
            Debug.LogError($"Error: {ex.Message}");
        }
    }
}
