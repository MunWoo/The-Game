using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Inventory equipment;

    [Header("Attributes")]
    public Attribute[] attributes;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        experienceBar.SetMaxExperience(maxExperience);
        UpdateAttributes();
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

        experienceBar.SetMaxExperience(maxExperience);
        experienceBar.UpdateExperience(currentExperience);
        level++;
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

    //Equiping items in here!
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

    //Unequiping items in here!
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.BaseObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.BaseObject.name, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(" ,", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attributes)
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
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