using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    private PlayerMovement playerMovement;
    private Gun02 gun02;

    private PlayerDebug playerDebug;

    [Header("Player Stats")]
    public int currentHealth;
    public int maxHealth;
    public int currentExperience;
    public int maxExperience;
    public int qDamage = 10;

    [Header("Sliders")]

    public HealthBar healthBar;
    public ExperienceBar experienceBar;

    [Header("Inventories")]
    public Inventory inventory;
    public Inventory equipment;

    [Header("Attributes")]
    public Attribute[] attributes;

    public int baseDamage = 20;
    public int baseDefence = 50;
    public int baseSpeed = 5;
    public int baseAttackRate = 80;
    public int totalDamage;
    public int totalDefence;
    public int totalSpeed;
    public float totalAttackRate;
    public int percentAttackDamage;
    public int percentDefence;
    public int percentSpeed;
    public float percentAttackRate;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Start basic things we need
        playerMovement = GetComponent<PlayerMovement>();
        gun02 = GetComponentInChildren<Gun02>();
        playerDebug = GetComponentInChildren<PlayerDebug>();


        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth, maxHealth);
        experienceBar.SetMaxExperience(maxExperience);

        UpdateAttributes();
        CalculateValues();
        //SetBaseAttributes();
    }

    public void GainExperience(int experience)
    {
        currentExperience += experience;
        experienceBar.UpdateExperience(currentExperience);
    }

    public void TakeDamage(float damage)
    {

    }

    private void Update()
    {
        //Save and Load Inventory
        if (Input.GetKeyDown(KeyCode.F4))
        {
            inventory.Save();
            equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            inventory.Load();
            equipment.Load();
        }
        if (currentExperience >= maxExperience)
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

        LevelUpAttibutes();

        experienceBar.SetMaxExperience(maxExperience);
        experienceBar.UpdateExperience(currentExperience);
        GlobalPlayerData.Instance.playerLevel++;
    }
    void LevelUpAttibutes()
    {
        foreach (var attribute in attributes)
        {
            switch (attribute.type)
            {
                case Attributes.Strenght:
                    attribute.value.BaseValue += 5;
                    break;
                case Attributes.Defence:
                    attribute.value.BaseValue += 5;
                    break;
                case Attributes.Speed:
                    attribute.value.BaseValue += 1;
                    break;
                case Attributes.AttackRate:
                    attribute.value.BaseValue += 1;
                    break;
            }
        }
        UpdateValues();
    }
    public void SetBaseAttributes()
    {
        foreach (var attribute in attributes)
        {
            switch (attribute.type)
            {
                case Attributes.Strenght:
                    attribute.value.BaseValue = baseDamage;
                    break;
                case Attributes.Defence:
                    attribute.value.BaseValue = baseDefence;
                    break;
                case Attributes.Speed:
                    attribute.value.BaseValue = baseSpeed;
                    break;
                case Attributes.AttackRate:
                    attribute.value.BaseValue = baseAttackRate;
                    break;
            }
        }
    }
    void UpdateValues()
    {
        playerMovement.moveSpeed = totalSpeed;
        gun02.gunDamage = totalDamage;
        gun02.reloadTime = totalAttackRate;
        //playerDebug.UpdateDebug();
        //playerDebug.UpdateEquipmentWindow();
    }

    public void OnCollisionEnter(Collision other)
    {
        var itemInfo = other.gameObject.GetComponent<GroundItem>();
        if (itemInfo)
        {
            Item _item = new Item(itemInfo.itemInfo);
            if (inventory.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void AttributeModified(Attribute attribute)
    {
        SetAttributes(attribute);
    }
    //Change values based on attributes value
    //Here we can draw the values of all the items equiped in the inventory and feed them to wherever we need
    private void SetAttributes(Attribute attribute)
    {
        switch (attribute.type)
        {
            case Attributes.Strenght:
                percentAttackDamage = attribute.value.ModifiedValue;
                //totalDamage = attribute.value.ModifiedValue; //Alter this so it increases damage by a percentage, maybe set the value here, but do the calculations elsewhere
                break;
            case Attributes.Defence:
                percentDefence = attribute.value.ModifiedValue;
                //totalDefence = attribute.value.ModifiedValue;
                break;
            case Attributes.Speed:
                percentSpeed = attribute.value.ModifiedValue;
                //totalSpeed = attribute.value.ModifiedValue;
                break;
            case Attributes.AttackRate:
                percentAttackRate = attribute.value.ModifiedValue;
                //totalAttackRate = attribute.value.ModifiedValue;
                break;
        }
        CalculateValues();
        UpdateValues();
    }
    private void UpdateAttributes()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }

    //Unequip Items in here
    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.BaseObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attributes)
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

    //Equip Items in here
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.BaseObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attributes)
                        {
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                        }
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

    public void IncreaseQDamage(int kills)
    {
        qDamage += Mathf.FloorToInt(kills * 0.5f);
        playerDebug.kills = 0;
        //playerDebug.UpdateDebug();
    }

    public void CalculateValues()
    {
        totalDamage = (baseDamage * (100 + percentAttackDamage)) / 100;
        totalDefence = (baseDefence * (100 + percentDefence)) / 100;
        totalSpeed = (baseSpeed * (100 + percentSpeed)) / 100;


        //Attack Speed Formula
        float convertedAttackRate = (baseAttackRate * ((100 + percentAttackRate) / 100)) / 100f;
        totalAttackRate = 1f / convertedAttackRate;

    }

    public void SetAttributes()
    {

    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }

}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public PlayerStats parent;
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(PlayerStats _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttribueModified);
    }
    public void AttribueModified()
    {
        parent.AttributeModified(this);
    }
}