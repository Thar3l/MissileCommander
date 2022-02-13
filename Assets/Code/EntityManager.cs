using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameUtils;
using Entities.Enemy;
using Entities.Factories;
using Entities;
using UnityEngine.UI;

public class EntityManager : Singleton<EntityManager>
{
    [Header("Spawners")]
    [SerializeField] private MissileSpawner missileSpawner;
    
    [Header("Prefabs")]
    [SerializeField] private Missile missilePrefab;
    [SerializeField] private Airplane airplanePrefab;
    [SerializeField] private Explosion explosionPrefab;

    public UnitFactory UnitFactory;
    public ExplosionFactory ExplosionFactory;

    private List<Entity> entityList;
    private GameRoundListener unitStateListener;

    public bool Initialize()
    {
        CreateInstances();
        BindEvents();
        missileSpawner.Init();
        return true;
    }

    void CreateInstances()
    {
        entityList = new List<Entity>();
        ExplosionFactory = new ExplosionFactory(explosionPrefab);
        UnitFactory = new UnitFactory(missilePrefab, SpawnExplosion);
    }

    void BindEvents()
    {
        UnitFactory.OnCreateEntityEvent += AddEntity;
        UnitFactory.OnDestroyEntityEvent += RemoveEntity;
        GameManager.Instance.OnGameStart += CreateUnitStateListener;
        GameManager.Instance.OnGameStop += Reset;
    }

    void CreateUnitStateListener()
    {
        unitStateListener = GameManager.Instance.GameRoundChecker.AddListener();
    }

    void SpawnExplosion(ExplosiveUnit explodedUnit)
    {
        var explosion = ExplosionFactory.CreateExplosion(explodedUnit.transform.position);
        explosion.SetTeam(explodedUnit.GetTeam());
    }

    public void AddEntity(Entity entity)
    {
        entityList.Add(entity);
    }

    bool AreThereAnyEnemyUnits()
    {
        return entityList?.Where(x => x is Missile && x.GetTeam() == 1).ToList().Count > 0 || missileSpawner.CanSpawn;
    }

    public void RemoveEntity(Entity entity)
    {
        entityList.Remove(entity);
        if (!AreThereAnyEnemyUnits())
            unitStateListener?.NotifyChecker();
    }

    void Reset()
    {
        unitStateListener = null;
        DestroyAllEntities();
    }

    private void DestroyAllEntities()
    {
        foreach (var entity in entityList.Reverse<Entity>())
            entity.Destroy();
    }
}
