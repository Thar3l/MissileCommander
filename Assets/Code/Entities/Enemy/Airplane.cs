using System.Collections;
using GameUtils;
using UnityEngine;

namespace Entities.Enemy
{
    public class Airplane : ExplosiveUnit
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Header("Bomb Target Position Properties")]
        [SerializeField] private Vector2 targetCenterPosition;
        [SerializeField] private Vector2 targetRange;
        
        private Vector2 _flyPosition;
        private int _currentBombCount = 0;
        private bool CanThrowBombs => _currentBombCount > 0;
        
        private readonly int _maxBombCount = 2;
        private readonly float _minThrowBombDelay = 1f;
        private readonly float _maxThrowBombDelay = 4f;

        void Awake()
        {
            SetTeam(1);
            OnHitEntity += (e) => Hide();
        }

        public override void Initialize(Vector2 targetPosition)
        {
            _flyPosition = targetPosition;
            SetLookDirection();
            gameObject.SetActive(true);
            _currentBombCount = _maxBombCount;
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
                yield return new WaitForSeconds(UnityEngine.Random.Range(_minThrowBombDelay, _maxThrowBombDelay));
                var missile = EntityManager.Instance.MissileFactory.CreateUnit(transform.position, GetBombTargetPosition()) as Missile;
                missile.SetTeam(1);
                missile.SetSpeed(1f);
                _currentBombCount--;
            }
        }

        Vector2 GetBombTargetPosition()
        {
            return Utils.PickRandomPositionFromRange(targetCenterPosition, targetRange);
        }

        Vector2 GetDirectionToFlyTarget()
        {
            var position = transform.position;
            return (new Vector2(position.x, position.y) - _flyPosition).normalized;
        }

        void SetLookDirection()
        {
            spriteRenderer.flipX = GetDirectionToFlyTarget().x < 0;
        }

        void Move()
        {
            transform.position += transform.right * 0.03f * (GetDirectionToFlyTarget().x < 0 ? 1 : -1);
            if (Vector2.Distance(transform.position, _flyPosition) < 0.1f)
                Hide();
        }

        void Hide()
        {
            StopCoroutine(ThrowBomb());
            gameObject.SetActive(false);
        }
    }
}
