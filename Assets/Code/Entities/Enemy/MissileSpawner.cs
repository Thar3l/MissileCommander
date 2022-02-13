using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using Entities.Factories;
using GameUtils;

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
        public bool CanSpawn => leftSpawnCount > 0;
        
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
            GameManager.Instance.OnGameNewRound += StartSpawn;
            GameManager.Instance.OnGameStop += Stop;
            factory = EntityManager.Instance.UnitFactory;
        }

        void Stop()
        {
            leftSpawnCount = 0;
            StopCoroutine(Spawn());
        }

        void Reset()
        {
            StopCoroutine(Spawn());
            leftSpawnCount = spawnCountPerGame;
        }

        public void StartSpawn()
        {
            Reset();
            leftSpawnCount = spawnCountPerGame;
            StartCoroutine(Spawn());
        }

        IEnumerator Spawn()
        {
            while (CanSpawn)
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
            missile.SetSpeed(1f);
        }

        Vector2 PickRandomSpawnPosition()
        {
            return Utils.PickRandomPositionFromRange(transform.position, spawnRange);
        }
        
        public Vector2 PickRandomTargetPosition()
        {
            return Utils.PickRandomPositionFromRange(targetCenterPosition, targetRange);
        }
    }
}

