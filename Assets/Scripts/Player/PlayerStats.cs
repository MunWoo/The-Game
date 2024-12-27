using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

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

    [Header("Inventories")]
    public Inventory inventory;
    public DisplayInventory displayInventory;

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

    private void Update()
    {
        //Save and Load Inventory
        if (Input.GetKeyDown(KeyCode.F5))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            inventory.Load();
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

    public void OnCollisionEnter(Collision other)
    {
        var itemInfo = other.gameObject.GetComponent<GroundItem>();
        if (itemInfo)
        {
            inventory.AddItem(new Item(itemInfo.itemInfo), 1);
            Destroy(other.gameObject);
        }
    }
    private void OnApplicationQuit()
    {
        inventory.Container.Items.Clear();
    }
}
