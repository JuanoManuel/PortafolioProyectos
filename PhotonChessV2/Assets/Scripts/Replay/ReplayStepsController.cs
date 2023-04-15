using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayStepsController : MonoBehaviour
{
    public Text stepText;

    private void Start()
    {
        ReplayEventSystem.current.OnBoardInstantiated += OnBoardInstantiated;
        ReplayEventSystem.current.OnPlayerReady += OnPlayerReady;
    }

    private void OnPlayerReady()
    {
        stepText.text = string.Empty;
    }

    private void OnDestroy()
    {
        //Unsubscribing to all events
        ReplayEventSystem.current.OnBoardInstantiated -= OnBoardInstantiated;
        ReplayEventSystem.current.OnPlayerReady -= OnPlayerReady;
    }

    private void OnBoardInstantiated(BoardInstantiatedEvent e)
    {
        stepText.text = "Adjust the board";
    }
}
