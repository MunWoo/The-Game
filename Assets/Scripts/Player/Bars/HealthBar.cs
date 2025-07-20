using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
        public Slider slider;
        public TextMeshProUGUI barText;
        public PlayerStats playerStats;

        void Awake()
        {
                barText = GetComponentInChildren<TextMeshProUGUI>();
        }
        void Start()
        {

        }
        public void SetMaxHealth(float health, float maxHealth)
        {
                int maxValueRoundedUp = Mathf.CeilToInt(maxHealth);
                int valueRoundedUp = Mathf.CeilToInt(health);
                slider.maxValue = playerStats.currentHealth;
                slider.value = playerStats.maxHealth;
                barText.text = $"Hp:{valueRoundedUp}/{maxValueRoundedUp}";
        }

        private void Update()
        {
                int maxValueRoundedUp = Mathf.CeilToInt(playerStats.maxHealth);
                int valueRoundedUp = Mathf.CeilToInt(playerStats.currentHealth);
                slider.value = playerStats.currentHealth;
                slider.maxValue = playerStats.maxHealth;
                barText.text = $"Hp:{valueRoundedUp}/{maxValueRoundedUp}";
        }

}
