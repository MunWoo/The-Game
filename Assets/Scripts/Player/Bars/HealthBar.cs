using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
        public Slider slider;
        public PlayerStats playerStats;

        public void Awake()
        {
                playerStats = PlayerStats.instance;
        }

        public void SetMaxHealth(int health)
        {
                int maxValueRoundedUp = Mathf.CeilToInt(maxHealth);
                int valueRoundedUp = Mathf.CeilToInt(health);
                slider.maxValue = playerStats.currentHealth;
                slider.value = playerStats.maxHealth;
                barText.text = $"Hp:{valueRoundedUp}/{maxValueRoundedUp}";
        }

        private void Update()
        {

                GameObject Player = GameObject.Find("Player");
                PlayerStats playerStats = Player.GetComponent<PlayerStats>();
                slider.value = playerStats.currentHealth;

                int maxValueRoundedUp = Mathf.CeilToInt(playerStats.maxHealth);
                int valueRoundedUp = Mathf.CeilToInt(playerStats.currentHealth);
                slider.value = playerStats.currentHealth;
                slider.maxValue = playerStats.maxHealth;
                barText.text = $"Hp:{valueRoundedUp}/{maxValueRoundedUp}";

        }

}
