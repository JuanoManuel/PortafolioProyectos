using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchDataPanel : MonoBehaviour
{
    public Text winner;
    public Text opponent;
    public Text type;
    public Text date;
    public Text duration;

    public void Activate(MatchData data)
    {
        winner.text = data.Winner;
        opponent.text = data.Opponent;
        type.text = data.Type;
        date.text = data.Date;
        duration.text = data.Duration;
        gameObject.SetActive(true);
    }

    public void OpenReplayScene(string replaySceneName)
    {
        SceneManager.LoadScene(replaySceneName);
    }
}