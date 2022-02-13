using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Player;
using GameUtils;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private List<Entity> buildingList;
    [SerializeField] private Vector2[] missileLauncherSpawnPoints;
    [SerializeField] private Vector2[] citySpawnPoints;

    [Header("Prefabs")]
    [SerializeField] private MissileLauncher missileLauncherPrefab;
    [SerializeField] private City cityPrefab;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        DrawSpawnPointGizmos(missileLauncherSpawnPoints);
        Gizmos.color = Color.blue;
        DrawSpawnPointGizmos(citySpawnPoints);
    }

    void DrawSpawnPointGizmos(Vector2[] spawnPoints)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            Gizmos.DrawCube(spawnPoint, Vector2.one/2);
        }
    }
    
    public bool Initialize()
    {
        buildingList = new List<Entity>();
        InputController.Instance.BindKey(KeyCode.Mouse0, Shoot);
        GameManager.Instance.OnGameStart += RefreshMissileLaunchers;
        GameManager.Instance.OnGameStart += RefreshCities;
        GameManager.Instance.OnGameNewRound += StartNewRound;
        return true;
    }

    List<MissileLauncher> GetMissileLauncherList()
    {
        return buildingList.OfType<MissileLauncher>().ToList();
    }
    
    List<City> GetCityList()
    {
        return buildingList.OfType<City>().ToList();
    }

    void StartNewRound()
    {
        GivePointsForPreviousRound();
        RefreshMissileLaunchers();
    }

    void RefreshMissileLaunchers()
    {
        if (GetMissileLauncherList().Count < 1)
        {
            SpawnMissileLaunchers();
            return;
        }
        foreach (var missileLauncher in GetMissileLauncherList())
        {
            missileLauncher.gameObject.SetActive(true);
            missileLauncher.RefreshMissileCount();
        }
    }
    
    void RefreshCities()
    {
        if (GetCityList().Count < 1)
        {
            SpawnCities();
            return;
        }
        foreach (var city in GetCityList())
            city.gameObject.SetActive(true);
    }

    void SpawnMissileLaunchers()
    {
        Spawn(missileLauncherPrefab, missileLauncherSpawnPoints, ref buildingList);
    }
    
    void SpawnCities()
    {
        Spawn(cityPrefab, citySpawnPoints, ref buildingList);
        foreach (var city in GetCityList())
            city.OnHitEntity += (x) => Die();
    }

    void Spawn<T>(T prefab, Vector2[] spawnPoints, ref List<T> container) where T : MonoBehaviour
    {
        foreach (var spawnPosition in spawnPoints)
        {
            var obj = Instantiate(
                prefab,
                spawnPosition,
                prefab.transform.rotation,
                transform);
            container.Add(obj);
        }
    }

    void Die()
    {
        var leftCities = GetCityList().Where(x => x.gameObject.activeSelf).ToList().Count;
        if (leftCities < 1)
        {
            GameManager.Instance.StopGame();
        }
    }

    void Shoot(Vector2 mousePosition)
    {
        var worldPos = InputController.Instance.GetWorldPointFromMousePosition(mousePosition);
        var launcher = GetNearestMissileLauncher(worldPos);
        launcher?.LaunchMissile(worldPos);
    }

    MissileLauncher GetNearestMissileLauncher(Vector3 shootWorldPosition)
    {
        MissileLauncher nearestLauncher = null;
        float lastDistance = 0;
        var availableLaunchers = GetMissileLauncherList().Where(x => x.gameObject.activeSelf && x.CanShoot).ToList();
        foreach (var missileLauncher in availableLaunchers)
        {
            var distanceToLauncher = Vector3.Distance(shootWorldPosition, missileLauncher.transform.position);
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

    void GivePointsForPreviousRound()
    {
        var leftCities = GetCityList().Where(c => c.gameObject.activeSelf).ToList().Count;
        var leftLaunchers = GetMissileLauncherList().Where(c => c.gameObject.activeSelf).ToList().Count;
        Debug.LogFormat("Round ended with {0} cities, {1} launchers. Can give points for the round.", leftCities, leftLaunchers);
    }
}
