using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    List<BaseEnemy> normalEnemies = new List<BaseEnemy>();
    List<BaseEnemy> bossEnemies = new List<BaseEnemy>();

    [Header("Spawning Settings")]
    public EnemyArray enemyArray;
    public bool canSpawn;
    public int numberOfSpawnedEnemies = 1;
    int repeatSpawn = 0;
    public int maxEnemies = 10;
    public float spawnRadius = 10f;
    public int spawnInterval = 5;
    public int spawnsForBoss = 5;
    public int spawnBoss = 1;
    public int enemiesAlive;

    [Header("Boss Settings")]
    public int healthMultiplier = 3;
    public float bossScale = 2;
    public float bossHp;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        canSpawn = true;
        repeatSpawn = numberOfSpawnedEnemies;
        PopulateNormalEnemies();
        PopulateBossEnemies();

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

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && enemiesAlive < maxEnemies)
        {
            SpawnEnemy();
        }
    }
    void SpawnEnemy()
    {
        canSpawn = false;
        if (spawnBoss == spawnsForBoss)
        {
            spawnBoss = 0;
            repeatSpawn -= 1;
            BaseEnemy enemyToSpawn = null;
            if (bossEnemies.Count > 0)
            {
                int randomIndex = Random.Range(0, bossEnemies.Count);
                enemyToSpawn = bossEnemies[randomIndex];


                // Calculate a random position within the spawn radius around the SpawnPoint
                Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                randomOffset.y = 0;  // Keep the spawning on the same Y-level (ground level)

                Vector3 spawnPosition = transform.position + randomOffset;

                // Instantiate the enemy at the calculated position
                Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            }

        }
        else
        {
            spawnBoss++;
            repeatSpawn -= 1;

            if (normalEnemies.Count > 0)
            {
                int randomIndex = Random.Range(0, normalEnemies.Count);
                BaseEnemy enemyToSpawn = normalEnemies[randomIndex];

                if (enemyToSpawn == null)
                    return;

                Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                randomOffset.y = 0;

                Vector3 spawnPosition = transform.position + randomOffset;

                Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            }

        }
        enemiesAlive++;
        if (repeatSpawn > 0)
            SpawnEnemy();
        else
            Invoke("SetSpawnTrue", spawnInterval);
    }

    public void SetSpawnTrue()
    {
        canSpawn = true;
        repeatSpawn = numberOfSpawnedEnemies;
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

}

/*
{
            spawnBoss++;
            repeatSpawn -= 1;

            if (normalEnemies.Count > 0)
            {
                int randomIndex = Random.Range(0, normalEnemies.Count);
                GameObject enemyToSpawn = normalEnemies[randomIndex];



                // Calculate a random position within the spawn radius around the SpawnPoint
                Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                randomOffset.y = 0;  // Keep the spawning on the same Y-level (ground level)

                Vector3 spawnPosition = transform.position + randomOffset;

                // Instantiate the enemy at the calculated position
                Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            }
        }
*/










