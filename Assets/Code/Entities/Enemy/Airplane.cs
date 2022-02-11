using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Enemy
{
    public class Airplane : ExplosiveUnit
    {
        [SerializeField] private SpriteRenderer renderer;
        private Vector2 flyPosition;

        public delegate void DestroyPlane(Airplane airplane);
        public event DestroyPlane OnDestroyPlane;

        public override void Initialize(Vector2 targetPosition)
        {
            this.flyPosition = targetPosition;
            SetLookDirection();
            gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            Move();
        }

        void Destroy()
        {
            OnDestroyPlane?.Invoke(this);
        }

        Vector2 GetDirectionToFlyTarget()
        {
            var position = transform.position;
            return (new Vector2(position.x, position.y) - flyPosition).normalized;
        }

        void SetLookDirection()
        {
            renderer.flipX = GetDirectionToFlyTarget().x < 0;
        }

        void Move()
        {
            transform.position += transform.right * 0.01f * (GetDirectionToFlyTarget().x < 0 ? -1 : 1);
            if (Vector2.Distance(transform.position, flyPosition) < 0.1f)
            {
                Explode();
            }
        }
    }
}
