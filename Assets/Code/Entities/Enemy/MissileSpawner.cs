using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using Entities.Factories;

namespace Entities.Enemy
{
    public class MissileSpawner : MonoBehaviour
    {
        [Header("Spawn Position Properties")]
        [SerializeField] private Vector2 spawnRange;
        
        [Header("Target Position Properties")]
        [SerializeField] private Vector2 targetCenterPosition;
        [SerializeField] private Vector2 targetRange;
        
        [Header("Spawner Properties")]
        [SerializeField] private float spawnDelay;
        [SerializeField] private int spawnCountPerGame;

        private UnitFactory factory;
        private int leftSpawnCount;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, spawnRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(targetCenterPosition, targetRange);
        }

        public void Init()
        {
            GameManager.Instance.OnGameStart += StartSpawn;
            GameManager.Instance.OnGameStop += Reset;
            factory = EntityManager.Instance.UnitFactory;
        }

        void Reset()
        {
            leftSpawnCount = spawnCountPerGame;
            StopCoroutine(Spawn());
        }

        public void StartSpawn()
        {
            leftSpawnCount = spawnCountPerGame;
            StartCoroutine(Spawn());
        }

        bool CanSpawn()
        {
            return leftSpawnCount > 0;
        }

        IEnumerator Spawn()
        {
            while (CanSpawn())
            {
                var missile = factory.CreateUnit(PickRandomSpawnPosition(), PickRandomTargetPosition());
                SetSpawnedMissileProperties(missile as Missile);
                leftSpawnCount--;
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        void SetSpawnedMissileProperties(Missile missile)
        {
            missile.SetTeam(1);
            missile.SetSpeed(0.04f);
        }

        Vector2 PickRandomSpawnPosition()
        {
            return PickRandomPositionFromRange(transform.position, spawnRange);
        }
        
        Vector2 PickRandomTargetPosition()
        {
            return PickRandomPositionFromRange(targetCenterPosition, targetRange);
        }
        
        Vector2 PickRandomPositionFromRange(Vector2 center, Vector2 range)
        {
            return new Vector2(
                center.x + Random.Range(-range.x/2, range.x/2),
                center.y + Random.Range(-range.y/2, range.y/2)
            );
        }
    }
}

