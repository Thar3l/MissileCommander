using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Player
{
    public class PlayerBuildings : MonoBehaviour
    {
        [SerializeField] PlayerBuildingsHelper buildingsHelper;
        private List<Entity> _buildingList;
        private List<MissileLauncher> MissileLauncherList => _buildingList.OfType<MissileLauncher>().ToList();
        private List<City> CityList => _buildingList.OfType<City>().ToList();

        public UnityAction OnAllCitiesDestroyed;


        void Awake()
        {
            _buildingList = new List<Entity>();
        }

        public List<City> GetLeftCities()
        {
            return CityList.Where(c => c.gameObject.activeSelf).ToList();
        }

        public List<MissileLauncher> GetLeftMissileLaunchers()
        {
            return MissileLauncherList.Where(c => c.gameObject.activeSelf).ToList();
        }

        public void RefreshMissileLaunchers()
        {
            if (MissileLauncherList.Count < 1)
            {
                SpawnMissileLaunchers();
                return;
            }

            foreach (var missileLauncher in MissileLauncherList)
            {
                missileLauncher.gameObject.SetActive(true);
                missileLauncher.RefreshMissileCount();
            }
        }

        void SpawnMissileLaunchers()
        {
            buildingsHelper.Spawn(
                buildingsHelper.missileLauncherPrefab,
                buildingsHelper.missileLauncherSpawnPoints,
                transform,
                ref _buildingList);
        }

        MissileLauncher GetNearestMissileLauncherToPoint(Vector3 point)
        {
            MissileLauncher nearestLauncher = null;
            float lastDistance = 0;
            var availableLaunchers = MissileLauncherList.Where(x => x.gameObject.activeSelf && x.CanShoot).ToList();
            foreach (var missileLauncher in availableLaunchers)
            {
                var distanceToLauncher = Vector3.Distance(point, missileLauncher.transform.position);
                if (lastDistance <= 0)
                {
                    lastDistance = distanceToLauncher;
                    nearestLauncher = missileLauncher;
                    continue;
                }

                if (distanceToLauncher < lastDistance)
                    nearestLauncher = missileLauncher;
            }

            return nearestLauncher;
        }

        public void ShootMissileLauncher(Vector3 point)
        {
            var launcher = GetNearestMissileLauncherToPoint(point);
            launcher?.LaunchMissile(point);
        }

        public void RefreshCities()
        {
            if (CityList.Count < 1)
            {
                SpawnCities();
                return;
            }

            foreach (var city in CityList)
                city.gameObject.SetActive(true);
        }

        void SpawnCities()
        {
            buildingsHelper.Spawn(
                buildingsHelper.cityPrefab,
                buildingsHelper.citySpawnPoints,
                transform,
                ref _buildingList);

            BindCityOnDieEvent();
        }

        void CityDestroy()
        {
            var leftCitiesCount = GetLeftCities().Count;
            if (leftCitiesCount < 1)
                OnAllCitiesDestroyed?.Invoke();
        }

        private void BindCityOnDieEvent()
        {
            foreach (var city in CityList)
                city.OnHitEntity += (x) => CityDestroy();
        }
    }
}