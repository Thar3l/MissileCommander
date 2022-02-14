using System.Collections.Generic;
using Entities;
using Entities.Player;
using UnityEngine;

namespace Code.Player
{
    public class PlayerBuildingsHelper : MonoBehaviour
    {
        public Vector2[] missileLauncherSpawnPoints;
        public Vector2[] citySpawnPoints;
    
        [Header("Prefabs")]
        public MissileLauncher missileLauncherPrefab;
        public City cityPrefab;
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            DrawSpawnPointGizmos(missileLauncherSpawnPoints);
            Gizmos.color = Color.blue;
            DrawSpawnPointGizmos(citySpawnPoints);
        }

        private void DrawSpawnPointGizmos(Vector2[] spawnPoints)
        {
            foreach (var spawnPoint in spawnPoints)
            {
                Gizmos.DrawCube(spawnPoint, Vector2.one/2);
            }
        }

        public void Spawn<T>(T prefab, Vector2[] spawnPoints, Transform parent, ref List<T> container) where T : MonoBehaviour
        {
            foreach (var spawnPosition in spawnPoints)
            {
                var obj = Instantiate(
                    prefab,
                    spawnPosition,
                    prefab.transform.rotation,
                    parent);
                container.Add(obj);
            }
        }
    }
}

