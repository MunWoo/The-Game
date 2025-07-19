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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        slider.maxValue = health;
        slider.value = health;
=======
=======
>>>>>>> Stashed changes
        int maxValueRoundedUp = Mathf.CeilToInt(maxHealth);
        int valueRoundedUp = Mathf.CeilToInt(health);
        slider.maxValue = playerStats.currentHealth;
        slider.value = playerStats.maxHealth;
        barText.text = $"Hp:{valueRoundedUp}/{maxValueRoundedUp}";
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    }

    private void Update()
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        GameObject Player = GameObject.Find("Player");
        PlayerStats playerStats = Player.GetComponent<PlayerStats>();
        slider.value = playerStats.currentHealth;
=======
=======
>>>>>>> Stashed changes
        int maxValueRoundedUp = Mathf.CeilToInt(playerStats.maxHealth);
        int valueRoundedUp = Mathf.CeilToInt(playerStats.currentHealth);
        slider.value = playerStats.currentHealth;
        slider.maxValue = playerStats.maxHealth;
        barText.text = $"Hp:{valueRoundedUp}/{maxValueRoundedUp}";
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    }

}
