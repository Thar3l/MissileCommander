using System.Collections;
using GameUtils;
using UI;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameRoundChecker GameRoundChecker;
    
    #region events
    public delegate void GameStart();
    public event GameStart OnGameStart;
    
    public delegate void GameNewRound();
    public event GameNewRound OnGameNewRound;
    
    public delegate void GameStop();
    public event GameStop OnGameStop;
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
        // StartGame();
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
