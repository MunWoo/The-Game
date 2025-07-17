using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01 : BaseEnemy
{
    void SetStats()
    {
        maxHealth = 120;
        health = maxHealth;
        enemyType = EnemyType.Normal;
        baseExperience = 10;

    }

    void Start()
    {
        SetStats();
    }

}
