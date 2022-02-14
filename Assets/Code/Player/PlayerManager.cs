using Code.Player;
using GameUtils;
using UnityEngine;

namespace Player
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private PlayerBuildings playerBuildings;
        private PlayerInput _playerInput;

        public bool Initialize()
        {
            playerBuildings.OnAllCitiesDestroyed += Die;
            _playerInput = new PlayerInput(playerBuildings.ShootMissileLauncher);
            GameManager.Instance.OnGameStart += playerBuildings.RefreshMissileLaunchers;
            GameManager.Instance.OnGameStart += playerBuildings.RefreshCities;
            GameManager.Instance.OnGameNewRound += StartNewRound;
            return true;
        }

        void StartNewRound()
        {
            GivePointsForPreviousRound();
            playerBuildings.RefreshMissileLaunchers();
        }
    
        public void Die()
        {
            GameManager.Instance.StopGame();
        }

        void GivePointsForPreviousRound()
        {
            var leftCitiesCount = playerBuildings.GetLeftCities().Count;
            var leftLaunchersCount = playerBuildings.GetLeftMissileLaunchers().Count;
            Debug.LogFormat("Round ended with {0} cities, {1} launchers. Can give points for the round.", leftCitiesCount, leftLaunchersCount);
        }
    }
}

