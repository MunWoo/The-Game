using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01 : BaseEnemy
{
    [Header("Player Attack Settings")]
    public float damage;
    public bool canAttack;
    public float timeBetweenAttacks;
    public float shootForce;
    PlayerStats playerStats;
    public Transform attackPoint;
    public EnemyBullet attackPrefab;
    void SetStats()
    {
        maxHealth = 300;
        health = maxHealth;
        enemyType = EnemyType.Boss;
        baseExperience = 50;
        damage = 20;
        timeBetweenAttacks = 2f;
        shootForce = 3f;

    }

    void Awake()
    {
        SetStats();
        playerStats = PlayerStats.instance;
        Invoke("ResetAttack", timeBetweenAttacks);
    }

    protected override void Update()
    {
        base.Update();
        //if (isAttacking && canAttack) Attack();
    }
    public void Attack()
    {
        canAttack = false;

        Vector3 attackDirection = (playerStats.transform.position - attackPoint.position).normalized;

        // Instantiate bullet and set its rotation
        EnemyBullet bulletInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.LookRotation(attackDirection));

        // Set initial velocity (we'll use this in the bullet script)
        bulletInstance.Initialize(attackDirection * shootForce); // We'll add this method

        Invoke("ResetAttack", timeBetweenAttacks);
    }

    void ResetAttack()
    {
        canAttack = true;
    }
}