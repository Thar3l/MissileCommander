using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace GameEvents
{
    public interface IGameEvent
    {
        void Execute();
        bool IsRunning();
    }
    
    public class RandomGameEventHandler : MonoBehaviour
    {
        private List<IGameEvent> _eventList;
        [SerializeField] private Vector2Int eventDelayRange;
    
        void Start()
        {
            RegisterEvents();
            GameManager.Instance.OnGameStart += StartEvents;
            GameManager.Instance.OnGameStop += StopEvents;
        }
    
        void RegisterEvents()
        {
            _eventList = new List<IGameEvent>();
            foreach (Transform child in transform.GetComponentInChildren<Transform>())
            {
                var gameEvent = child.GetComponent<IGameEvent>();
                _eventList.Add(gameEvent);
                Debug.LogFormat("Registering {0}.", gameEvent);
            }
        }
    
        void StartEvents()
        {
            if (_eventList.Count > 0)
                StartCoroutine(ExecuteEvents());
        }
    
        void StopEvents()
        {
            StopCoroutine(ExecuteEvents());
        }
    
        IGameEvent PickRandomEvent()
        {
            var availableEventsList = _eventList.Where(e => !e.IsRunning()).ToList();
            if (availableEventsList.Count > 0)
                return availableEventsList[UnityEngine.Random.Range(0, availableEventsList.Count)];
            return null;
        }
    
        IEnumerator ExecuteEvents()
        {
            while (true)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(eventDelayRange.x, eventDelayRange.y+1));
                PickRandomEvent()?.Execute();
            }
        }
    }
}
