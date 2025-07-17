using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDebug : MonoBehaviour
{
    public static PlayerDebug instance;
    public GameObject debugContent;
    public GameObject equipmentInventory;





    public int kills;
    public int qDamage;
    public int valueIncByKills;

    private PlayerStats playerStats;
    private Gun02 gun02;

    [Header("Debug Equipment Inventory Stats")]
    public TextMeshProUGUI attackDamage;
    public TextMeshProUGUI defence;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI attackRate;
    public TextMeshProUGUI bonusAttack;
    public TextMeshProUGUI bonusDefence;
    public TextMeshProUGUI bonusSpeed;
    public TextMeshProUGUI bonusAttackRate;

    [Header("Debug Values")]
    private TextMeshProUGUI damageValueT;
    private TextMeshProUGUI killsCountT;
    private TextMeshProUGUI speedT;
    private TextMeshProUGUI attackRateT;
    private TextMeshProUGUI qDamageT;
    private TextMeshProUGUI valueIncByKillsT;

    void Awake()
    {
        instance = this;
    }

    public void Start()
    {

        //Debug Values
        damageValueT = debugContent.transform.Find("DamageValue").GetComponent<TextMeshProUGUI>();
        killsCountT = debugContent.transform.Find("Kills Count").GetComponent<TextMeshProUGUI>();
        speedT = debugContent.transform.Find("Speed").GetComponent<TextMeshProUGUI>();
        qDamageT = debugContent.transform.Find("Q Damage").GetComponent<TextMeshProUGUI>();
        attackRateT = debugContent.transform.Find("Attack Rate").GetComponent<TextMeshProUGUI>();
        valueIncByKillsT = debugContent.transform.Find("Value Increased by Kills").GetComponent<TextMeshProUGUI>();



        playerStats = GetComponent<PlayerStats>();
        gun02 = GetComponent<Gun02>();

        //UpdateDebug();
        //UpdateEquipmentWindow();
    }


    void Update()
    {
        damageValueT.text = $"Total Damage: {playerStats.totalDamage}";
        killsCountT.text = $"Kills: {kills}";
        speedT.text = $"Speed: {playerStats.totalSpeed}";
        attackRateT.text = $"Attack Rate: {playerStats.totalAttackRate}";
        qDamageT.text = $"Q Damage: {playerStats.qDamage + playerStats.totalDamage}";
        valueIncByKillsT.text = $"Q damage Increase by Kills: {playerStats.qDamage}";

        attackDamage.text = $"Total Damage: {playerStats.totalDamage}";
        defence.text = $"Defence: {playerStats.totalDefence}";
        speed.text = $"Speed: {playerStats.totalSpeed}";
        attackRate.text = $"Attack Rate: {playerStats.totalAttackRate}";
        bonusAttack.text = $"Bonus Attack: {playerStats.percentAttackDamage}";
        bonusDefence.text = $"Bonus Defence: {playerStats.percentDefence}";
        bonusSpeed.text = $"Bonus Speed: {playerStats.percentSpeed}";
        bonusAttackRate.text = $"Bonus Attack Rate: {playerStats.percentAttackRate}";
    }



















    /*
    public void UpdateDebug()
    {
        damageValueT.text = $"Total Damage: {playerStats.totalDamage}";
        killsCountT.text = $"Kills: {kills}";
        speedT.text = $"Speed: {playerStats.totalSpeed}";
        attackRateT.text = $"Attack Rate: {playerStats.totalAttackRate}";
        qDamageT.text = $"Q Damage: {playerStats.qDamage + playerStats.totalDamage}";
        valueIncByKillsT.text = $"Q damage Increase by Kills: {playerStats.qDamage}";
    }

    public void UpdateEquipmentWindow()
    {
        attackDamage.text = $"Total Damage: {playerStats.totalDamage}";
        defence.text = $"Defence: {playerStats.totalDefence}";
        speed.text = $"Speed: {playerStats.totalSpeed}";
        attackRate.text = $"Attack Rate: {playerStats.totalAttackRate}";
        bonusAttack.text = $"Bonus Attack: {playerStats.percentAttackDamage}";
        bonusDefence.text = $"Bonus Defence: {playerStats.percentDefence}";
        bonusSpeed.text = $"Bonus Speed: {playerStats.percentSpeed}";
        bonusAttackRate.text = $"Bonus Attack Rate: {playerStats.percentAttackRate}";
    }
    */
}
