using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UI Button which represents a user's match
/// Fills data inside the ui prefab
/// </summary>
public class MatchButtonData : MonoBehaviour
{
    [SerializeField] Text opponentName;//UI Text for the opponent's name
    [SerializeField] Text dateText;//UI Text for the date of the match
    [SerializeField] Image color;//Image of what kind of pieces the user was in this especific match
    [SerializeField] Text matchResultText;//whether was a win or lose for the user, in case of a local match shows the winner color instead
    [SerializeField] Sprite blackIcon;//sprite to be shown when the user was the blacks
    [SerializeField] Sprite whiteIcon;//spite to be shown when the user was the whites
    [SerializeField] MatchDataPanel matchDataPanel;//reference to the panel where the match data it's shown
    [SerializeField] GlobalData globalData;
    MatchData matchData;//match data
    /// <summary>
    /// Fills the data inside the button
    /// </summary>
    /// <param name="match">dictionary representation of a match stored in firebase</param>
    /// <param name="userID">user key of the player</param>
    /// <param name="opponentName">Display name of the opponent</param>
    public void ActivateLocalMatchButton(Dictionary<string,object> match, MatchDataPanel panel)
    {
        matchDataPanel = panel;
        opponentName.text = "Local match";
        matchResultText.text = $"{match["winner"]} won";
        color.sprite = match["winner"].ToString() == "white" ? whiteIcon : blackIcon;
        dateText.text = match["date"].ToString();
        matchData = new MatchData()
        {
            Winner = matchResultText.text,
            Opponent = opponentName.text,
            Date = dateText.text,
            Duration = match["time"].ToString(),
            Type = "Local",
            Moves = match["replay"] as List<string>
        };
    }

    public void ActivateOnlineMatchButton(Dictionary<string,object> match,string userID, MatchDataPanel panel)
    {
        matchDataPanel = panel;
        matchData = new MatchData() { Type = "Online", Duration = match["time"].ToString(), Moves = match["replay"] as List<string> };
        Dictionary<string, object> players = match["players"] as Dictionary<string, object>;
        foreach(string key in players.Keys)
        {
            Dictionary<string, object> playerData = players[key] as Dictionary<string, object>;
            if (key == userID)
            {
                matchResultText.text = (bool)playerData["winner"] ? "Victory" : "Defeat";
                matchData.Winner = matchResultText.text;
                color.sprite = playerData["color"].Equals("white") ? whiteIcon : blackIcon;
                dateText.text = match["date"].ToString();
                matchData.Date = dateText.text;
                matchData.Color = playerData["color"].ToString();
            }
            else
            {
                opponentName.text = playerData["displayName"].ToString();
                matchData.Opponent = opponentName.text;
            }
        }
    }

    public void OpenMatchDataPanel()
    {
        matchDataPanel.Activate(matchData);
        globalData.matchData = matchData;
    }
}
