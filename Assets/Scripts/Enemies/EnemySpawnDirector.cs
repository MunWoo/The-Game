using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum Location
{
    Map01,
    Map02
}


public class EnemySpawnDirector : MonoBehaviour
{
    public static EnemySpawnDirector Instance;

    [Header("Enemy Spawn Settings")]
    public EnemyArray enemyArray;
    public Location location;
    public StageSpawners stageSpawners;
    public SpawnSets whereToSpawn;


    [Header("Spawner Settings")]
    public bool canSpawn = false;
    public float spawnInterval = 10; //in Seconds
    public int bossChance = 25; //in %
    public int bossTrys = 10;
    public int bossesTried = 0;
    public int enemiesToSpawn = 1;
    public int enemiesAlive = 0;
    public int maxEnemies = 10;

    [Header("Enemy Randomizer Settings")]
    BaseEnemy enemyToSpawn;
    GameObject spawnPoint;
    EnemyType enemyType;
    List<BaseEnemy> normalEnemies = new List<BaseEnemy>();
    List<BaseEnemy> bossEnemies = new List<BaseEnemy>();


    public Dictionary<EnemyType, int> enemiesSpawnChances = new Dictionary<EnemyType, int>();




    void Awake()
    {
        Instance = this;
        //Populate the dictionary with the chances
        {
            enemiesSpawnChances.Add(EnemyType.Normal, 80); //in %
            enemiesSpawnChances.Add(EnemyType.Boss, 20); //in %
        }
    }

    void Start()
    {
        whereToSpawn = GetSpawnSetForLocation(location);
        PopulateNormalEnemies();
        PopulateBossEnemies();
        Invoke("CanSpawn", spawnInterval);
    }

    void CanSpawn()
    {
        canSpawn = true;
    }

    void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            StartSpawningSequence();
        }
    }

    public void StartSpawningSequence()
    {
        enemyType = GetRandomEnemyTypeByWeight();
        WhatToSpawn(enemyType);
        spawnPoint = GetRandomSpawnNode(whereToSpawn); // spawnPoint is a GameObject in worldspace
        SpawnEnemies(enemyToSpawn, spawnPoint);
    }
    void PopulateNormalEnemies()
    {
        foreach (var enemy in enemyArray.enemies)
        {
            if (enemy.GetComponent<BaseEnemy>().enemyType == EnemyType.Normal)
            {
                normalEnemies.Add(enemy);
            }
        }
    }

    void PopulateBossEnemies()
    {
        foreach (var enemy in enemyArray.enemies)
        {
            if (enemy.GetComponent<BaseEnemy>().enemyType == EnemyType.Boss)
            {
                bossEnemies.Add(enemy);
            }
        }
    }

    //Set the spawn Location acording to the location set in the editor
    SpawnSets GetSpawnSetForLocation(Location location)
    {
        string name = location.ToString();
        SpawnSets spawnSet = stageSpawners.spawnSets.FirstOrDefault(set => set.name == name);

        if (spawnSet == null)
        {
            Debug.LogWarning($"No SpawnSet found for location: {name}");
        }

        return spawnSet;
    }
    public void WhatToSpawn(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Normal:
                {
                    int randomIndex = Random.Range(0, normalEnemies.Count);
                    enemyToSpawn = normalEnemies[randomIndex];
                }
                break;

            case EnemyType.Boss:
                {
                    int randomIndex = Random.Range(0, bossEnemies.Count);
                    enemyToSpawn = bossEnemies[randomIndex];
                }
                break;
        }
    }

    GameObject GetRandomSpawnNode(SpawnSets whereToSpawn)
    {
        int randomIndex = Random.Range(0, whereToSpawn.spawnPoints.Length);
        return whereToSpawn.spawnPoints[randomIndex];
    }

    void SpawnEnemies(BaseEnemy enemyToSpawn, GameObject spawnPoint)
    {
        var comp = spawnPoint.GetComponent<SpawnPoint>();
        comp.SpawnEnemy(enemyToSpawn);
    }

    EnemyType GetRandomEnemyTypeByWeight()
    {
        int totalWeight = 0;

        foreach (var entry in enemiesSpawnChances)
            totalWeight += entry.Value;

        int randomValue = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var entry in enemiesSpawnChances)
        {
            cumulative += entry.Value;
            if (randomValue <= cumulative)
                switch (entry.Key)
                {
                    case EnemyType.Normal:
                        {
                            if (bossesTried == bossTrys) return EnemyType.Boss;
                            else
                                bossesTried++;
                            return EnemyType.Normal;
                        }
                    case EnemyType.Boss:
                        {
                            bossesTried = 0;
                            return EnemyType.Boss;
                        }
                }
        }
        // Fallback
        Debug.Log("SpawnChances in the EnemySpawnDirector gave Error");
        return enemiesSpawnChances.Keys.First();
    }



}
