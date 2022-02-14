using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEvents
{
    public class RandomGameEventHandler : MonoBehaviour
    {
        private List<IGameEvent> _eventList;
        [SerializeField] private Vector2Int eventDelayRange;

        private void Start()
        {
            RegisterEvents();
            GameManager.Instance.OnGameStart += StartEvents;
            GameManager.Instance.OnGameStop += StopEvents;
        }

        private void RegisterEvents()
        {
            _eventList = new List<IGameEvent>();
            foreach (Transform child in transform.GetComponentInChildren<Transform>())
            {
                var gameEvent = child.GetComponent<IGameEvent>();
                _eventList.Add(gameEvent);
                Debug.LogFormat("Registering {0}.", gameEvent);
            }
        }

        private void StartEvents()
        {
            if (_eventList.Count > 0)
                StartCoroutine(ExecuteEventsCoroutine());
        }

        private void StopEvents()
        {
            StopCoroutine(ExecuteEventsCoroutine());
        }

        private IGameEvent PickRandomEvent()
        {
            var availableEventsList = _eventList.Where(e => !e.IsRunning()).ToList();
            if (availableEventsList.Count > 0)
                return availableEventsList[UnityEngine.Random.Range(0, availableEventsList.Count)];
            return null;
        }

        private IEnumerator ExecuteEventsCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(eventDelayRange.x, eventDelayRange.y+1));
                PickRandomEvent()?.Execute();
            }
        }
    }
}
