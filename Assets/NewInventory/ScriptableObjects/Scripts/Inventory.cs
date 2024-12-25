using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New X Inventory", menuName = "X Inventory System/Inventory")]
public class Inventory : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private ItemDatabase itemDatabase;
    public List<InventorySlot> Container = new List<InventorySlot>();

    private void OnEnable()
    {
#if UNITY_EDITOR
        itemDatabase = (ItemDatabase)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabase));
#else
        itemDatabase = Resources.Load<ItemDatabase>("Database");
#endif
    }

    public void AddItem(BaseObject _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].amount += _amount;
                hasItem = true;
                break;
            }
        }
        if (hasItem == false)
        {
            InventorySlot newInventorySlot = new InventorySlot(itemDatabase.GetId[_item], _item, _amount);
            Container.Add(newInventorySlot);
        }
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
        Debug.Log("Inventory saved to: " + savePath);
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
            Debug.Log("Loaded Inventory from: " + savePath);
        }
    }


    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Container.Count; i++)
        {
            Container[i].item = itemDatabase.GetItem[Container[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
    }
}


[System.Serializable]
public class InventorySlot
{
    public int ID;
    public BaseObject item;
    public int amount;
    public InventorySlot(int _id, BaseObject _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
}