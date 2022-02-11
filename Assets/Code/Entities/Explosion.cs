using UnityEngine;

namespace Entities
{
    public class Explosion : Entity
    {
        private float expandTimer;
        private float expandDelay;
        private float explosionRadius;

        public delegate void EndExplosion(Explosion explosion);
        public event EndExplosion OnEndExplosion;

        public void Initialize(float expandDelay, float explosionRadius)
        {
            expandTimer = 0;
            this.expandDelay = expandDelay;
            this.explosionRadius = explosionRadius;
            SetScale(0);
            gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            Expand();
        }

        void SetScale(float radius)
        {
            transform.localScale = Vector2.one * radius;
        }

        void Expand()
        {
            expandTimer += Time.fixedDeltaTime;
            var progress = Mathf.Min(expandTimer / expandDelay, 1);
            SetScale(explosionRadius * progress);
            if (progress >= 1)
                OnEndExplosion?.Invoke(this);
        }
    }
}
