using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{

    [Header("Spawning Settings")]
    public GameObject enemyPrefab;
    public bool canSpawn;
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

    [Header("Enemies Health")]

    public int enemie01Health = 120;



    void Start()
    {
        canSpawn = true;
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
            //Get the health component and get the transform component
            TestEnemy testEnemy = enemyPrefab.GetComponent<TestEnemy>();
            Transform testEnemyTransform = enemyPrefab.transform;

            //Multiply the boss HP
            testEnemy.maxHealth = enemie01Health * healthMultiplier;

            //Spawn boss with increased size
            testEnemyTransform.localScale = new Vector3(testEnemy.originalScale * bossScale, testEnemy.originalScale * bossScale, testEnemy.originalScale * bossScale);



            //Spawn Enemy
            // Calculate a random position within the spawn radius around the SpawnPoint
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = 0;  // Keep the spawning on the same Y-level (ground level)

            Vector3 spawnPosition = transform.position + randomOffset;

            // Instantiate the enemy at the calculated position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        }
        else
        {
            spawnBoss++;


            //Get the health component and get the transform component
            TestEnemy testEnemy = enemyPrefab.GetComponent<TestEnemy>();
            Transform testEnemyTransform = enemyPrefab.transform;
            //Spawn enemy with default scale
            testEnemyTransform.localScale = new Vector3(testEnemy.originalScale, testEnemy.originalScale, testEnemy.originalScale);
            //Spawn with normal HP
            testEnemy.maxHealth = enemie01Health;


            // Calculate a random position within the spawn radius around the SpawnPoint
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = 0;  // Keep the spawning on the same Y-level (ground level)

            Vector3 spawnPosition = transform.position + randomOffset;

            // Instantiate the enemy at the calculated position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
        enemiesAlive++;
        Invoke("SetSpawnTrue", spawnInterval);
    }

    public void SetSpawnTrue()
    {
        canSpawn = true;
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

}













