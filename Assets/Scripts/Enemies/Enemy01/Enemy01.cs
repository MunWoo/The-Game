using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01 : BaseEnemy
{
    PlayerStats playerStats;

    [Header("Attack Settings")]
    public bool canAttack;
    public float shootForce = 0;
    public float damage = 0;
    public float timeBetweenAttacks;
    public Transform attackPoint;
    public EnemyBullet attackPrefab;

    void SetStats()
    {
        maxHealth = 120;
        health = maxHealth;
        enemyType = EnemyType.Normal;
        baseExperience = 10;
        damage = 10;
        timeBetweenAttacks = 2f;
        shootForce = 4f;
        canAttack = true;
    }

    void Awake()
    {
        SetStats();
    }
    private void Start()
    {
        playerStats = PlayerStats.instance;
    }

    protected override void Update()
    {
        base.Update();
        if (isAttacking && canAttack) Attack();
    }
    public void Attack()
    {
        canAttack = false;

        Vector3 attackDirection = (playerStats.transform.position - attackPoint.position).normalized;

        // Instantiate bullet and set its rotation
        EnemyBullet bulletInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.LookRotation(attackDirection));

        //Set bullet Damage
        bulletInstance.damage = damage;

        // Set initial velocity (we'll use this in the bullet script)
        bulletInstance.Initialize(attackDirection * shootForce); // We'll add this method

        Invoke("ResetAttack", timeBetweenAttacks);
    }

    void ResetAttack()
    {
        canAttack = true;
    }

}
