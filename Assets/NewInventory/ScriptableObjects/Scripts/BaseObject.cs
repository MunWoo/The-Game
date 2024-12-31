using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ItemType
{
    Helmet,
    Weapon,
    Chestplate,
    Leggins,
    Shield,
    Misc
}
public enum Attributes
{
    Agility,
    Strength,
    Defence,
    AttackSpeed

}

public class BaseObject : ScriptableObject
{
    public int itemId;
    public string itemName;
    public ItemType itemType;
    public Sprite itemSprite;
    public ItemBuff[] buffs;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }

}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;
    public ItemBuff[] buffs;
    public Item()
    {
        Name = "";
        Id = -1;
    }
    public Item(BaseObject item)
    {
        if (item == null)
        {
            Debug.LogError("BaseObject 'item' is null!");
            return;
        }

        if (item.buffs == null)
        {
            Debug.LogError($"BaseObject 'item.buffs' is null for item {item.itemName}!");
            return;
        }

        Name = item.itemName;
        Debug.Log($"Item Name: {Name}");
        Id = item.itemId;
        Debug.Log($"Item ID: {Id}");
        buffs = new ItemBuff[item.buffs.Length];

        for (int i = 0; i < buffs.Length; i++)
        {
            if (item.buffs[i] == null)
            {
                Debug.LogError($"ItemBuff at index {i} is null!");
                continue;
            }

            // Initialize each buff
            buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max)
            {
                attributes = item.buffs[i].attributes
            };
        }

        // Sort buffs array by value in descending order
        Array.Sort(buffs, (a, b) => b.value.CompareTo(a.value));

        // Log sorted buffs
        foreach (var buff in buffs)
        {
            if (buff != null)
            {
                Debug.Log($"Attribute: {buff.attributes}, Value: {buff.value}");
            }
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public Attributes attributes;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }
    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}