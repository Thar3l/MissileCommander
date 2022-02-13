using UnityEngine;
using UnityEngine.Events;

namespace Entities.Factories{
    public class UnitFactory : EntityFactory<ExplosiveUnit>
    {
        private readonly UnityAction<ExplosiveUnit> _spawnExplosionAction;
        
        public UnitFactory(ExplosiveUnit prefab, UnityAction<ExplosiveUnit> spawnExplosionAction) : base(prefab)
        {
            this._spawnExplosionAction = spawnExplosionAction;
        }

        protected override void SetNewEntityProperties(ExplosiveUnit obj)
        {
            base.SetNewEntityProperties(obj);
            obj.OnExplosion += (entity) => DestroyEntity(obj);
            obj.OnExplosion += (entity) => _spawnExplosionAction(obj);
            obj.transform.SetParent(EntityManager.Instance.transform);
        }
        
        public ExplosiveUnit CreateUnit(Vector2 spawnPosition, Vector2 targetPosition)
        {
            var unit = CreateEntity(spawnPosition);
            unit.Initialize(targetPosition);
            return unit;
        }
    }
}

