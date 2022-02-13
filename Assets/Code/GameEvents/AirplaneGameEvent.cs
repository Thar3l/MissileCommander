using System;
using Entities.Enemy;
using UnityEngine;
using Random = System.Random;

namespace GameEvents
{
    public class AirplaneGameEvent : MonoBehaviour, IGameEvent
    {
        public struct PlaneStartEndPosition
        {
            public Vector2 startPosition;
            public Vector2 endPosition;
        }
        
        [SerializeField] private Airplane _airplane;
        [SerializeField] Vector2[] planeStartPositions;
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (var startPos in planeStartPositions)
            {
                Gizmos.DrawCube(startPos, Vector2.one);
            }
        }
    
        PlaneStartEndPosition GetPlaneStartAndTargetPosition()
        {
            var rnd = UnityEngine.Random.Range(0, 2);
            int startIndex = 0;
            int endIndex = 0;
            if (rnd == 1)
            {
                startIndex = 0;
                endIndex = 1;
            }
            else
            {
                startIndex = 1;
                endIndex = 0;
            }
            
            return new PlaneStartEndPosition
                {
                    startPosition = planeStartPositions[startIndex],
                    endPosition = planeStartPositions[endIndex]
                };
        }
    
        public void Execute()
        {
            var startEndPos = GetPlaneStartAndTargetPosition();
            _airplane.transform.position = startEndPos.startPosition;
            _airplane.Initialize(startEndPos.endPosition);
            Debug.Log("Starting Airplane game event.");
        }
    
        public bool IsRunning()
        {
            return _airplane.gameObject.activeSelf;
        }
    }
}
