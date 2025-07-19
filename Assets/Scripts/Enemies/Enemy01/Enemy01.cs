using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01 : BaseEnemy
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
        maxHealth = 120;
        health = maxHealth;
        enemyType = EnemyType.Normal;
        baseExperience = 10;
        damage = 10;
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
        if (isAttacking && canAttack) Attack();
    }
    public void Attack()
    {
        canAttack = false;
        Vector3 attackDirection = playerStats.transform.position - attackPoint.position;


        EnemyBullet bulletInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.LookRotation(attackDirection));

        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
        rb.AddForce(attackDirection * shootForce, ForceMode.Impulse);
        Invoke("ResetAttack", timeBetweenAttacks);
    }

    void ResetAttack()
    {
        canAttack = true;
    }
}