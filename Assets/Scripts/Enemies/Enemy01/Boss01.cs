using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01 : BaseEnemy
{
    void SetStats()
    {
        maxHealth = 300;
        health = maxHealth;
        enemyType = EnemyType.Boss;
        baseExperience = 50;

    }

    void Awake()
    {
        SetStats();
    }
}
