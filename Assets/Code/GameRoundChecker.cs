using System.Collections.Generic;
using UnityEngine;

public class GameRoundChecker
{
    private List<GameRoundListener> _listeners;

    public GameRoundChecker()
    {
        _listeners = new List<GameRoundListener>();
    }

    public void StopAllListeners()
    {
        _listeners = new List<GameRoundListener>();
    }

    public GameRoundListener AddListener()
    {
        var listener = new GameRoundListener();
        _listeners.Add(listener);
        listener.OnNotify += EndGameRound;
        return listener;
    }

    private void ResetListeners()
    {
        foreach (var listener in _listeners)
        {
            listener.State = false;
        }
    }

    private bool CheckGameRound()
    {
        foreach (var listener in _listeners)
        {
            if (!listener.State)
                return false;
        }

        return true;
    }

    private void EndGameRound()
    {
        if (CheckGameRound())
        {
            Debug.Log("Starting new round");
            ResetListeners();
            GameManager.Instance.StartNewRound();
        }
    }
}
