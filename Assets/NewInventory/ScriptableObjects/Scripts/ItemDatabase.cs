using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "X Inventory System/New Database")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public BaseObject[] ItemObjects;
    //public Dictionary<int, BaseObject> GetItem = new Dictionary<int, BaseObject>();

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            //ItemObjects[i].data.Id = i;
            //GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        //GetItem = new Dictionary<int, BaseObject>();
    }
}
