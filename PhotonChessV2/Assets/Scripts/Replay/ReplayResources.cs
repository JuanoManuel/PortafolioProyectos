using Lean.Touch;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReplayResources : MonoBehaviour
{
    public GameObject board;
    [SerializeField] GlobalData globalData;
    [SerializeField] GameObject replayControllerPanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] Text winnerText;
    private bool matchFinished;

    private void Start()
    {
        ReplayEventSystem.current.OnGameFinished += ShowWinPanel;
        matchFinished = false;
    }

    private void OnDestroy()
    {
        ReplayEventSystem.current.OnGameFinished -= ShowWinPanel;   
    }

    public void SpawnBoard()
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        GameObject spawnObject = Instantiate(board, Vector3.zero, rotation);
        ReplayEventSystem.current.BoardInstantiated(this, spawnObject);
    }

    public void SetReady()
    {
        replayControllerPanel.SetActive(true);
        GetComponent<LeanTouch>().enabled = false;
        ReplayEventSystem.current.PlayerReady();
    }

    public void NextMove()
    {
        ReplayEventSystem.current.NextMove();
    }

    public void PreviousMove()
    {
        if (matchFinished)
            winPanel.SetActive(false);
        ReplayEventSystem.current.PreviousMove();
    }

    public void GoHome()
    {
        SceneManager.LoadScene("Menu");
    }

    private void ShowWinPanel(string winner)
    {
        matchFinished = true;
        winnerText.text = winner;
        winPanel.SetActive(true);
    }
}
