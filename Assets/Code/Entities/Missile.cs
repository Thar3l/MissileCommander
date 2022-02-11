using UnityEngine;

namespace Entities
{
    public class Missile : ExplosiveUnit
    {
        [SerializeField] private TrailRenderer trailRenderer;
        private Vector2 explodePosition;
        private float speed;
    
    
        public override void Initialize(Vector2 targetPosition)
        {
            explodePosition = targetPosition;
            SetLookDirection(explodePosition);
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
            return speed;
        }
    
        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }
    
        void Move()
        {
            transform.position += transform.up * GetSpeed();
            if (Vector2.Distance(transform.position, explodePosition) < 0.1f)
            {
                Explode();
            }
        }
    }
}

