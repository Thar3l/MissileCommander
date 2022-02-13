using UnityEngine;
using UnityEngine.Events;

namespace Entities.Factories{
    public class UnitFactory : EntityFactory<ExplosiveUnit>
    {
        private UnityAction<ExplosiveUnit> spawnExplosionAction;
        
        public UnitFactory(ExplosiveUnit prefab, UnityAction<ExplosiveUnit> spawnExplosionAction) : base(prefab)
        {
            this.spawnExplosionAction = spawnExplosionAction;
        }

        protected override void SetNewEntityProperties(ExplosiveUnit obj)
        {
            base.SetNewEntityProperties(obj);
            obj.OnExplosion += (entity) => DestroyEntity(obj);
            obj.OnExplosion += (entity) => spawnExplosionAction(obj);
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

