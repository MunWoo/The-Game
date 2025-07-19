using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject playerGameObject;
    public TextMeshProUGUI barText;
    PlayerStats playerStats;

    void Awake()
    {
        barText = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Start()
    {
        playerStats = PlayerStats.instance;

    }
    public void SetMaxHealth(float health, float maxHealth)
    {
        slider.maxValue = health;
        slider.value = health;
        barText.text = $"Hp:{health}/{maxHealth}";
    }

    public void UpdateHealthBar()
    {
        slider.value = playerStats.currentHealth;
        slider.maxValue = playerStats.maxHealth;
        barText.text = $"Hp:{playerStats.currentHealth}/{playerStats.maxHealth}";
    }

}
