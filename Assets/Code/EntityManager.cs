using UnityEngine;
using GameUtils;
using Entities.Enemy;
using Entities.Factories;
using Entities;

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

    public bool Initialize()
    {
        ExplosionFactory = new ExplosionFactory(explosionPrefab);
        UnitFactory = new UnitFactory(missilePrefab, SpawnExplosion);
        missileSpawner.Init();
        return true;
    }

    void SpawnExplosion(Transform unitTransform)
    {
        ExplosionFactory.CreateExplosion(unitTransform.position);
    }
}
