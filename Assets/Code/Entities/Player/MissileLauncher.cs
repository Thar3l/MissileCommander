using UnityEngine;
using GameUtils;

namespace Entities.Player
{
    public class MissileLauncher : Entity
    {
        [SerializeField] private int maxMissileCount;

        private int _currentMissileCount;
        public bool CanShoot => _currentMissileCount > 0;

        void Start()
        {
            RefreshMissileCount();
            OnHitEntity += (entity) => gameObject.SetActive(false);
        }

        public void RefreshMissileCount()
        {
            _currentMissileCount = maxMissileCount;

        }

        public void LaunchMissile(Vector2 targetPosition)
        {
            if (CanShoot)
            {
                var missile = EntityManager.Instance.UnitFactory.CreateUnit(transform.position, targetPosition) as Missile;
                missile.SetTeam(0);
                missile.SetSpeed(3f);
                _currentMissileCount--;
            }
        }
    }
}
