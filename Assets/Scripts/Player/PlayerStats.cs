using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    public int currentHealth;
    public int maxHealth;
    public int currentExperience;
    public int maxExperience;
    public int level = 1;

    [Header("Sliders")]

    public HealthBar healthBar;
    public ExperienceBar experienceBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        experienceBar.SetMaxExperience(maxExperience);
    }

    public void GainExperience(int experience)
    {
        currentExperience += experience;
        experienceBar.UpdateExperience(currentExperience);

        if (currentExperience == maxExperience)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentExperience -= maxExperience;
        maxHealth += 20;
        currentHealth += 20;
        maxExperience += 20;

        experienceBar.SetMaxExperience(maxExperience);
        experienceBar.UpdateExperience(currentExperience);
        level++;
    }
}
