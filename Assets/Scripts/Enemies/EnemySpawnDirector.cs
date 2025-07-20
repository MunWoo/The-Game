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
    public static EnemySpawnDirector instance;

    [Header("Enemy Spawn Settings")]
    public EnemyArray enemyArray;
    public Location location;
    public StageSpawners stageSpawners;
    public SpawnSets whereToSpawn;


    [Header("Spawner Settings")]
    public bool canSpawn;
    public bool playerInArea = false;
    public float spawnInterval = 10; //in Seconds
    public int normalChance = 90;
    public int bossChance = 10; //in %
    public int bossTrys = 10;
    public int bossesTried = 0;
    public int enemiesToSpawn = 1;
    public int enemiesAlive = 0;
    public int maxEnemies = 10;

    [Header("Enemy Randomizer Settings")]
    BaseEnemy enemyToSpawn;
    SpawnPoint spawnPoint;
    EnemyType enemyType;
    List<BaseEnemy> normalEnemies = new List<BaseEnemy>();
    List<BaseEnemy> bossEnemies = new List<BaseEnemy>();


    public Dictionary<EnemyType, int> enemiesSpawnChances = new Dictionary<EnemyType, int>();




    void Awake()
    {
        instance = this;
        //Populate the dictionary with the chances
        {
            enemiesSpawnChances.Add(EnemyType.Normal, normalChance); //in %
            enemiesSpawnChances.Add(EnemyType.Boss, bossChance); //in %
        }
    }

    void Start()
    {
        whereToSpawn = GetSpawnSetForLocation(location);
        PopulateNormalEnemies();
        PopulateBossEnemies();
        canSpawn = true;
    }

    void ResetSpawn()
    {
        canSpawn = true;
    }

    void Update()
    {
        if (playerInArea == true && canSpawn == true && enemiesAlive < maxEnemies)
        {
            StartSpawningSequence();
            canSpawn = false;
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
        SpawnSets spawnSet = stageSpawners.spawnSets.FirstOrDefault(set => set.setName == name);

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
                    if (normalEnemies == null) Debug.Log("There are no " + EnemyType.Normal + "type enemies");
                    enemyToSpawn = normalEnemies[randomIndex];
                }
                break;

            case EnemyType.Boss:
                {
                    int randomIndex = Random.Range(0, bossEnemies.Count);
                    if (bossEnemies == null) Debug.Log("There are no " + EnemyType.Boss + "type enemies");
                    enemyToSpawn = bossEnemies[randomIndex];
                }
                break;
        }
    }

    SpawnPoint GetRandomSpawnNode(SpawnSets whereToSpawn)
    {
        int randomIndex = Random.Range(0, whereToSpawn.spawnPoints.Length);
        return whereToSpawn.spawnPoints[randomIndex];
    }

    void SpawnEnemies(BaseEnemy enemyToSpawn, SpawnPoint spawnPoint)
    {
        spawnPoint.SpawnEnemy(enemyToSpawn);
        Invoke("ResetSpawn", spawnInterval);
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
