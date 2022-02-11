using System.Collections;
using GameUtils;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region events
    public delegate void GameStart();
    public event GameStart OnGameStart;
    
    public delegate void GameStop();
    public event GameStop OnGameStop;
    #endregion
    
    private void Awake()
    {
        StartCoroutine(LoadGame());
        StartGame();
    }

    IEnumerator LoadGame()
    {
        yield return new WaitUntil(EntityManager.Instance.Initialize);
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
    }
    
    public void StopGame()
    {
        OnGameStop?.Invoke();
    }
}
