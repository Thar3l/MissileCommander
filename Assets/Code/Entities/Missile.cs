using UnityEngine;

namespace Entities
{
    public class Missile : ExplosiveUnit
    {
        [SerializeField] private TrailRenderer trailRenderer;
        private Vector2 _explodePosition;
        private float _speed;

        public override void Initialize(Vector2 targetPosition)
        {
            _explodePosition = targetPosition;
            SetLookDirection(_explodePosition);
            gameObject.SetActive(true);
            trailRenderer.Clear();
        }

        private void FixedUpdate()
        {
            Move();
        }
    
        public override void SetTeam(int team)
        {
            base.SetTeam(team);
            SetTrailColor(GetTeam() == 0 ? Color.green : Color.red);
        }
        
        void SetTrailColor(Color color)
        {
            trailRenderer.startColor = color;
            trailRenderer.endColor = color;
        }
    
        void SetLookDirection(Vector3 direction)
        {
            transform.up = direction - transform.position;
        }
    
        float GetSpeed()
        {
            return _speed;
        }
    
        public void SetSpeed(float speed)
        {
            this._speed = speed;
        }
    
        public void Move()
        {
            transform.position += transform.up * GetSpeed() * Time.fixedDeltaTime;
            if (Vector2.Distance(transform.position, _explodePosition) < 0.1f)
            {
                Explode();
            }
        }
    }
}

