using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "X Inventory System/New Database")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public BaseObject[] Items;
    public Dictionary<BaseObject, int> GetId = new Dictionary<BaseObject, int>();
    public Dictionary<int, BaseObject> GetItem = new Dictionary<int, BaseObject>();

    public void OnAfterDeserialize()
    {
        GetId = new Dictionary<BaseObject, int>();
        GetItem = new Dictionary<int, BaseObject>();
        for (int i = 0; i < Items.Length; i++)
        {
            GetId.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {

    }
}
