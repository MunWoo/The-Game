using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Normal,
    Boss
}

public abstract class BaseEnemy : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int baseExperience;

    public BaseEnemy enemyComponent;
    public NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] public EnemyHealthBar enemyHealthBar;
    public EnemyType enemyType;

    [Header("Stafe Settings")]
    public float strafeSpeed = 4f;
    public float strafeDirection = 1f;
    public float strafeTimer;
    public float strafeInterval = 1f;

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


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    private void Update()
    {
        //check for sightrange and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        enemyHealthBar.UpdateHealthBar(health);

        if (health <= 0) Invoke(nameof(Die), 0.01f);

    }


    public void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint Reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    public void SearchWalkPoint()
    {
        //Calculate point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

    }

    public void ChasePlayer()
    {
        agent.SetDestination(PlayerStats.instance.transform.position);
    }

    public void AttackPlayer()
    {
        //Makes the enemy not move when attacking
        agent.SetDestination(transform.position);

        transform.LookAt(PlayerStats.instance.transform);

        StrafeSideToSide();

        if (!alreadyAttacked)
        {
            //Attack Code Here
            //
            //

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void StrafeSideToSide()
    {
        strafeTimer += Time.deltaTime;

        if (strafeTimer >= strafeInterval)
        {
            strafeDirection *= -1; // Change direction
            strafeTimer = 0f;
        }

        // Move left/right relative to the enemy's local right vector
        Vector3 strafe = transform.right * strafeDirection * strafeSpeed * Time.deltaTime;
        transform.position += strafe;
    }


    public void Die()
    {
        PlayerDebug.instance.kills++;
        PlayerStats.instance.GainExperience(baseExperience);
        EnemySpawner.instance.enemiesAlive--;

        //PlayerDebug.Instance.kills += 1;
        var (chance, min, max) = ItemDropDirector.instance.GetDropChance(enemyType);

        int value = Random.Range(min, max);
        if (value <= chance)
        {
            ItemDropDirector.instance.SpawnItem(transform.position);
        }
        Destroy(gameObject);
    }

}

