using Entities.Enemy;
using UnityEngine;

namespace GameEvents
{
    public class AirplaneGameEvent : MonoBehaviour, IGameEvent
    {
        public struct PlaneStartEndPosition
        {
            public Vector2 StartPosition;
            public Vector2 EndPosition;
        }
        
        [SerializeField] private Airplane airplane;
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
                    StartPosition = planeStartPositions[startIndex],
                    EndPosition = planeStartPositions[endIndex]
                };
        }
    
        public void Execute()
        {
            var startEndPos = GetPlaneStartAndTargetPosition();
            airplane.transform.position = startEndPos.StartPosition;
            airplane.Initialize(startEndPos.EndPosition);
            Debug.Log("Starting Airplane game event.");
        }
    
        public bool IsRunning()
        {
            return airplane.gameObject.activeSelf;
        }
    }
}
