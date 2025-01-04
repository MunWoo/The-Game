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
    Strenght,
    Defence,
    Speed,
    AttackRate

}
public class BaseObject : ScriptableObject
{
    public string itemName;
    public bool stackable;
    public ItemType itemType;
    public Sprite itemSprite;
    public Item data = new Item();
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
    public int Id = -1;
    public ItemBuff[] buffs;
    public Item()
    {
        Name = "";
        Id = -1;
    }
    public Item(BaseObject item)
    {
        Name = item.itemName;
        Id = item.data.Id;
        if (item.data.buffs != null)
        {
            buffs = new ItemBuff[item.data.buffs.Length];
            for (int i = 0; i < buffs.Length; i++)
            {
                // Initialize each buff
                buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
                {
                    attributes = item.data.buffs[i].attributes
                };
            }

        }
        // Sort buffs array by value in descending order
        Array.Sort(buffs, (a, b) => b.value.CompareTo(a.value));
    }
}

[System.Serializable]
public class ItemBuff : IModifiers
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

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}