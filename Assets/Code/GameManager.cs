using System.Collections;
using Entities;
using GameUtils;
using Player;
using UI;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public GameRoundChecker GameRoundChecker;
    
    #region events
    public event UnityAction OnGameStart;
    public event UnityAction OnGameNewRound;
    public event UnityAction OnGameStop;
    #endregion
    
    private void Awake()
    {
        GameRoundChecker = new GameRoundChecker();
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        yield return new WaitUntil(UserInterfaceManager.Instance.Initialize);
        yield return new WaitUntil(EntityManager.Instance.Initialize);
        yield return new WaitUntil(PlayerManager.Instance.Initialize);
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
    }
    
    public void StartNewRound()
    {
        OnGameNewRound?.Invoke();
    }
    
    public void StopGame()
    {
        GameRoundChecker.StopAllListeners();
        OnGameStop?.Invoke();
    }
}
