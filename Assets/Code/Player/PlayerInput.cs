using UnityEngine;
using UnityEngine.Events;

namespace Code.Player
{
    public class PlayerInput
    {
        private readonly UnityAction<Vector3> _launchMissileAction;

        public PlayerInput(UnityAction<Vector3> launchMissileAction)
        {
            _launchMissileAction = launchMissileAction;
            InputController.Instance.BindKey(KeyCode.Mouse0, Shoot);
        }

        void Shoot(Vector2 mousePosition)
        {
            var worldPos = InputController.Instance.GetWorldPointFromMousePosition(mousePosition);
            _launchMissileAction?.Invoke(worldPos);
        }
    }
}

