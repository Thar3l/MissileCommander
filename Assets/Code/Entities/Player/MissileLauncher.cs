using UnityEngine;
using GameUtils;

namespace Entities.Player
{
    public class MissileLauncher : Entity
    {
        [SerializeField] private Camera cam;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var worldPos = Utils.MousePositionToWorldPoint(cam, Input.mousePosition);
                var missile = EntityManager.Instance.UnitFactory.CreateUnit(transform.position, worldPos) as Missile;
                missile.SetTeam(0);
                missile.SetSpeed(0.08f);
            }
        }
    }
}
