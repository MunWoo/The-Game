using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Misc
}

[System.Serializable]
public class BaseObject : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite itemSprite;

}