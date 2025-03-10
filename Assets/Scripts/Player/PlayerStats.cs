using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Gun02 gun02;
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
    public Inventory equipment;

    [Header("Attributes")]
    public Attribute[] attributes;

    public int baseDamage;
    public int baseDefence;
    public int baseSpeed;
    public int baseAttackRate;
    public int totalDamage;
    public int totalDefence;
    public int totalSpeed;
    public int totalAttackRate;

    private void Start()
    {
        //Start basic things we need
        playerMovement = GetComponent<PlayerMovement>();
        gun02 = GetComponentInChildren<Gun02>();


        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        experienceBar.SetMaxExperience(maxExperience);

        UpdateAttributes();
        SetBaseAttributes();
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
            equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            inventory.Load();
            equipment.Load();
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
        level++;
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
                    attribute.value.BaseValue += 1 / 2;
                    break;
                case Attributes.AttackRate:
                    attribute.value.BaseValue += 1;
                    break;
            }
        }
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
    void UpdateSpeedValue()
    {
        playerMovement.moveSpeed = totalSpeed;
        gun02.gunDamage = totalDamage;
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
        Debug.Log(string.Concat(attribute.type, "was updated! Value is now ", attribute.value.ModifiedValue));
        SetAttributes(attribute);
    }
    //Change values based on attributes value
    //Here we can draw the values of all the items equiped in the inventory and feed them to wherever we need
    private void SetAttributes(Attribute attribute)
    {
        switch (attribute.type)
        {
            case Attributes.Strenght:
                totalDamage = attribute.value.ModifiedValue;
                Debug.Log("Strenght = " + attribute.value.ModifiedValue);
                break;
            case Attributes.Defence:
                Debug.Log("Defence = " + attribute.value.ModifiedValue);
                totalDefence = attribute.value.ModifiedValue;
                break;
            case Attributes.Speed:
                Debug.Log("Speed = " + attribute.value.ModifiedValue);
                totalSpeed = attribute.value.ModifiedValue;
                break;
            case Attributes.AttackRate:
                Debug.Log("AttackRate = " + attribute.value.ModifiedValue);
                totalAttackRate = attribute.value.ModifiedValue;
                break;
        }
        UpdateSpeedValue();
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
                print(string.Concat("Removed ", _slot.BaseObject.name, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(" ,", _slot.AllowedItems)));

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