using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject playerGameObject;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    private void Update()
    {
        GameObject Player = GameObject.Find("Player");
        PlayerStats playerStats = Player.GetComponent<PlayerStats>();
        slider.value = playerStats.currentHealth;
    }

}
