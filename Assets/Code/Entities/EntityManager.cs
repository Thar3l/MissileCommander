using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameUtils;
using Entities.Enemy;
using Entities.Factories;

namespace Entities
{
    public class EntityManager : Singleton<EntityManager>
    {
        [Header("Spawners")]
        [SerializeField] private MissileSpawner missileSpawner;
        
        [Header("Prefabs")]
        [SerializeField] private Missile missilePrefab;
        [SerializeField] private Explosion explosionPrefab;

        public UnitFactory MissileFactory;
        private ExplosionFactory _explosionFactory;

        private List<Entity> _entityList;
        private GameRoundListener _unitStateListener;

        public bool Initialize()
        {
            CreateInstances();
            BindEvents();
            missileSpawner.Init();
            return true;
        }
        
        private void Reset()
        {
            _unitStateListener = null;
            DestroyAllEntities();
        }

        private void CreateInstances()
        {
            _entityList = new List<Entity>();
            _explosionFactory = new ExplosionFactory(explosionPrefab);
            MissileFactory = new UnitFactory(missilePrefab, SpawnExplosion);
        }

        private void BindEvents()
        {
            MissileFactory.OnCreateEntityEvent += AddEntity;
            MissileFactory.OnDestroyEntityEvent += RemoveEntity;
            GameManager.Instance.OnGameStart += CreateUnitStateListener;
            GameManager.Instance.OnGameStop += Reset;
        }
        
        private void DestroyAllEntities()
        {
            foreach (var entity in _entityList.Reverse<Entity>())
                entity.Destroy();
        }

        private void CreateUnitStateListener()
        {
            _unitStateListener = GameManager.Instance.GameRoundChecker.AddListener();
        }

        private void SpawnExplosion(ExplosiveUnit explodedUnit)
        {
            var explosion = _explosionFactory.CreateExplosion(explodedUnit.transform.position);
            explosion.SetTeam(explodedUnit.GetTeam());
        }

        private void AddEntity(Entity entity)
        {
            _entityList.Add(entity);
        }

        private bool AreThereAnyEnemyUnits()
        {
            var enemyMissileList = _entityList?.Where(x => x is Missile && x.GetTeam() == 1).ToList();
            return enemyMissileList?.Count > 0 || missileSpawner.CanSpawn;
        }

        private void RemoveEntity(Entity entity)
        {
            _entityList.Remove(entity);
            if (!AreThereAnyEnemyUnits())
                _unitStateListener?.NotifyChecker();
        }
    }
}
