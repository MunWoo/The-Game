using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestEnemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    [SerializeField] EnemyHealthBar enemyHealthBar;

    //EnemyStats
    [Header("Enemy Stats")]
    public int maxHealth = 120;
    public int health;
    public float originalScale = 1;
    public int baseExperience;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private ItemRandomizer itemDrop;
    private PlayerStats playerStats;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    public void Start()
    {
        health = maxHealth;
        itemDrop = GetComponent<ItemRandomizer>();
        GameObject playerGameObject = GameObject.Find("Player");
        playerStats = playerGameObject.GetComponent<PlayerStats>();
        enemyHealthBar.SetHealthBar(health, maxHealth);

    }
    private void Update()
    {
        //check for sightrange and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint Reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Makes the enemy not move when attacking
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack Code Here
            //
            //

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        enemyHealthBar.UpdateHealthBar(health);

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.01f);

    }
    private void DestroyEnemy()
    {
        GameObject enemySpawner = GameObject.Find("EnemySpawner");
        EnemySpawner enemySpawnerComponent = enemySpawner.GetComponent<EnemySpawner>();
        enemySpawnerComponent.enemiesAlive--;
        itemDrop.GenerateLoot();

        Destroy(gameObject);

        //Award player the Experience for the kill
        playerStats.GainExperience(baseExperience);
        PlayerDebug.instance.kills += 1;
        //playerDebug.UpdateDebug();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}