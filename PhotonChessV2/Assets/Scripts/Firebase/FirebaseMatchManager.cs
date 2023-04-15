using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Uses Firebase middleware to manage all match storage
/// </summary>
public class FirebaseMatchManager : FirebaseManager
{
    public GlobalData opponentData;
    Dictionary<string, object> changes;
    /// <summary>
    /// Creates a new match node, and bind it to the corresponding historials
    /// </summary>
    /// <param name="winner">winner color</param>
    /// <param name="time">total time of the match</param>
    /// <param name="replay">list of chess moves using an especific string notation</param>
    /// <param name="wasOnline">whether the match was online or not</param>
    /// <param name="players">dictionary of player keys (KEY) and its color during the match (VALUE)</param>
    public void StoreMatch(string winner,float time,DateTime date,List<string> replay,bool wasOnline,string playerColor = null)
    {
        if (firebase.user == null)
            return;
        changes = new Dictionary<string, object>();
        if (wasOnline)
        {
            //creating dictionary of player objects
            Dictionary<string, Dictionary<string, object>> players = new Dictionary<string, Dictionary<string, object>>();
            //Storing this player data
            Dictionary<string, object> player = new Dictionary<string, object>();
            player["displayName"] = firebase.user.DisplayName;
            player["color"] = playerColor;
            player["winner"] = playerColor == winner ? true : false;
            player["historialKey"] = firebase.userData["idHistorial"].ToString();
            players[firebase.user.UserId] = player;
            //Storing opponent data
            player = new Dictionary<string, object>();
            player["displayName"] = opponentData.DisplayName;
            player["color"] = playerColor == "white"? "black":"white";
            player["winner"] = playerColor == winner ? false : true;
            player["historialKey"] = opponentData.historialKey;
            players[opponentData.userKey] = player;
            //creating match node
            string matchKey = firebase.CreateMatch(ref changes, winner, time, date, replay, wasOnline, players);
            //adding the matchkey to each player's historial
            foreach (string userKey in players.Keys)
            {
                StartCoroutine(firebase.UpdateHistorial(OnUpdateCompleted, userKey, matchKey, players[userKey]["historialKey"].ToString()));
            }
        }
        else //add match to this user historial
        {
            //creating match node
            string matchKey = firebase.CreateMatch(ref changes, winner, time, date, replay, wasOnline);
            StartCoroutine(firebase.UpdateHistorial(OnUpdateCompleted, firebase.user.UserId, matchKey));
        }
        //Loading all changes stored in the dictionary
        StartCoroutine(firebase.UpdateChanges(changes, OnUpdateCompleted));
    }
    /// <summary>
    /// Updates user's data
    /// </summary>
    /// <param name="userKey">user key</param>
    /// <param name="isWin">whether the player won the match or not</param>
    public void UpdatePlayerStats(bool isWin)
    {
        StartCoroutine(firebase.UpdatePlayerStats(isWin, OnUpdateCompleted));
    }
    /// <summary>
    /// Delegated method to spawn alert message if any
    /// </summary>
    /// <param name="title">alert's title</param>
    /// <param name="description">alert's description</param>
    /// <param name="alertType">type of alert</param>
    private void OnUpdateCompleted(string title,string description,AlertPanel.AlertType alertType)
    {
        if (alertType != AlertPanel.AlertType.Info)
        {
            SpawnAlertMessage(title, description, alertType);
        }
    }

    public Dictionary<string,object> GetUserData()
    {
        Dictionary<string, object> data = firebase.userData;
        data["userKey"] = firebase.user.UserId;
        return data;
    }
}
