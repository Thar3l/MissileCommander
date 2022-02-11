using UnityEngine;

namespace Entities.Factories
{
    public class ExplosionFactory: EntityFactory<Explosion>
    {
        public ExplosionFactory(Explosion prefab) : base(prefab)
        {
        }

        protected override void SetNewEntityProperties(Explosion obj)
        {
            obj.OnEndExplosion += (entity) => DestroyEntity(obj);
            obj.transform.SetParent(EntityManager.Instance.transform);
        }
    
        public Explosion CreateExplosion(Vector2 spawnPosition)
        {
            var explosion = CreateEntity(spawnPosition);
            explosion.Initialize(0.5f, 1f);
            return explosion;
        }
    }
}
