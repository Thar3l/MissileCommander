using System.Collections;
using System.Collections.Generic;
using GameUtils;
using UnityEngine;
using Random = System.Random;

namespace Entities.Enemy
{
    public class Airplane : ExplosiveUnit
    {
        [SerializeField] private SpriteRenderer renderer;
        
        [Header("Bomb Target Position Properties")]
        [SerializeField] private Vector2 targetCenterPosition;
        [SerializeField] private Vector2 targetRange;
        
        private Vector2 flyPosition;

        private int maxBombCount = 2;
        private int currentBombCount = 0;
        private bool CanThrowBombs => currentBombCount > 0;

        private float minThrowBombDelay = 1f;
        private float maxThrowBombDelay = 4f;

        void Awake()
        {
            SetTeam(1);
            OnHitEntity += (e) => Hide();
        }

        public override void Initialize(Vector2 targetPosition)
        {
            flyPosition = targetPosition;
            SetLookDirection();
            gameObject.SetActive(true);
            currentBombCount = maxBombCount;
            StartCoroutine(ThrowBomb());
        }

        private void FixedUpdate()
        {
            Move();
        }

        IEnumerator ThrowBomb()
        {
            while (CanThrowBombs)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(minThrowBombDelay, maxThrowBombDelay));
                var missile = EntityManager.Instance.UnitFactory.CreateUnit(transform.position, GetBombTargetPosition()) as Missile;
                missile.SetTeam(1);
                missile.SetSpeed(1f);
                currentBombCount--;
            }
        }

        Vector2 GetBombTargetPosition()
        {
            return Utils.PickRandomPositionFromRange(targetCenterPosition, targetRange);
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
            transform.position += transform.right * 0.03f * (GetDirectionToFlyTarget().x < 0 ? 1 : -1);
            if (Vector2.Distance(transform.position, flyPosition) < 0.1f)
            {
                Hide();
            }
        }

        void Hide()
        {
            StopCoroutine(ThrowBomb());
            gameObject.SetActive(false);
        }
    }
}
