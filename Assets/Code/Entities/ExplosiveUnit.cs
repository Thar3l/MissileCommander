using UnityEngine;

namespace Entities {
    public abstract class ExplosiveUnit : Entity
    {
        public delegate void Explosion(ExplosiveUnit entity);
        public event Explosion OnExplosion;

        void Awake()
        {
            OnHitEntity += (entity) => Explode();
        }

        public abstract void Initialize(Vector2 targetPosition);

        public void Explode()
        {
            OnExplosion?.Invoke(this);
        }
    }
}
