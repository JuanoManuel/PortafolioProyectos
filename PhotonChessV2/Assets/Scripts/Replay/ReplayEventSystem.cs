using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayEventSystem : MonoBehaviour
{
    public static ReplayEventSystem current;

    private void Awake()
    {
        current = this;
    }

    public event Action<BoardInstantiatedEvent> OnBoardInstantiated;
    public event Action OnPlayerReady;
    public event Action OnNextMove;
    public event Action OnPreviousMove;
    public event Action OnGoHome;
    public event Action<string> OnGameFinished;

    public void BoardInstantiated(object sender,GameObject board)
    {
        OnBoardInstantiated?.Invoke(new BoardInstantiatedEvent { Board = board });
    }

    public void EndGame(string winner)
    {
        OnGameFinished?.Invoke(winner);
    }

    public void PlayerReady()
    {
        OnPlayerReady?.Invoke();
    }

    public void NextMove()
    {
        OnNextMove?.Invoke();
    }

    public void PreviousMove()
    {
        OnPreviousMove?.Invoke();
    }

    public void GoHome()
    {
        OnGoHome?.Invoke();
    }
}
