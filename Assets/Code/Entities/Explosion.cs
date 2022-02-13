using UnityEngine;

namespace Entities
{
    public class Explosion : Entity
    {
        private float _expandTimer;
        private float _expandDelay;
        private float _explosionRadius;

        public delegate void EndExplosion(Explosion explosion);
        public event EndExplosion OnEndExplosion;

        public void Initialize(float expandDelay, float explosionRadius)
        {
            _expandTimer = 0;
            _expandDelay = expandDelay;
            _explosionRadius = explosionRadius;
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
            _expandTimer += Time.fixedDeltaTime;
            var progress = Mathf.Min(_expandTimer / _expandDelay, 1);
            SetScale(_explosionRadius * progress);
            if (progress >= 1)
                OnEndExplosion?.Invoke(this);
        }
    }
}
