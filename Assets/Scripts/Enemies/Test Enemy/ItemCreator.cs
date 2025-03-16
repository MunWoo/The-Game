using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public GameObject itemDropPrefab;
    public BaseObject itemPrefab;
    public ItemBuff[] buffsForItem;
    public ItemDatabase itemDatabase;
    int newId;

    public void CreateNewItem(string newItemName, ItemType newItemType, Sprite newItemSprite, string newItemRarity, int newItemBuffsLenght, float newItemMinValueBuffsIncrease, float newItemMaxValueBuffsIncrease)
    {
        newId = itemDatabase.ItemObjects.Length;

        // Create a new instance of BaseObject
        BaseObject newItem = ScriptableObject.CreateInstance<BaseObject>();
        newItem.stackable = false;
        newItem.itemName = newItemName;
        newItem.itemType = newItemType;
        newItem.itemSprite = newItemSprite;
        newItem.data = new Item();
        newItem.data.Id = newId;
        newItem.data.Name = newItemName;

        AddItemToDatabase(newItem);
        SaveItemAsset(newItem);


        if (newItemBuffsLenght > 0)
        {
            GenerateBuffs(newItemBuffsLenght, newItemMinValueBuffsIncrease, newItemMaxValueBuffsIncrease);
            newItem.data.buffs = buffsForItem;
        }

        GameObject newItemObject = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        GroundItem groundItem = newItemObject.GetComponent<GroundItem>();

        if (groundItem != null)
        {
            groundItem.itemInfo = newItem;
        }
        else Debug.Log("Missing ground item, cannont place item info into the drop item");


        Debug.Log($"{newItemName} of Type: {newItemType}, Rarity: {newItemRarity}, Buffs: {newItemBuffsLenght}, Min/Max Buff: {newItemMinValueBuffsIncrease}/{newItemMaxValueBuffsIncrease}");
    }

    public void GenerateBuffs(int buffsLenght, float buffsMinIncrease, float buffsMaxIncrease)
    {
        GameObject playerGameObject = GameObject.Find("Player");
        var playerReference = playerGameObject.GetComponent<PlayerStats>(); // Get the PlayerStats component from the GameObject
        buffsForItem = new ItemBuff[buffsLenght];
        for (int i = 0; i < buffsLenght; i++)
        {
            int min = Mathf.FloorToInt(5 + (playerReference.level * buffsMinIncrease));
            int max = Mathf.FloorToInt(1 + (playerReference.level * buffsMinIncrease));

            Attributes selectedAttribute = (Attributes)Random.Range(0, System.Enum.GetValues(typeof(Attributes)).Length);

            if (selectedAttribute == Attributes.Speed)
            {
                max = Random.Range(1, Mathf.FloorToInt(5 + (playerReference.level * buffsMinIncrease)));
                if (max >= 10)
                    max = 10;
            }

            buffsForItem[i] = new ItemBuff(min, max)
            {
                attributes = selectedAttribute
            };
            Debug.Log(selectedAttribute);
        }
    }

    public void AddItemToDatabase(BaseObject item)
    {

        int newLenght = itemDatabase.ItemObjects.Length + 1;
        System.Array.Resize(ref itemDatabase.ItemObjects, newLenght);

        itemDatabase.ItemObjects[itemDatabase.ItemObjects.Length - 1] = item;

    }

    // Save the ScriptableObject (BaseObject) in a specific folder
    private void SaveItemAsset(BaseObject newItem)
    {
        string folderPath = "Assets/NewInventory/ScriptableObjects/Items";  // Set the folder path where you want to save the item asset
        string assetPath = folderPath + "/Item_" + newItem.data.Name + ".asset";  // Generate the file path based on the item name

        // Ensure the folder exists
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/NewInventory/ScriptableObjects", "Items");
        }

        // Save the asset
        AssetDatabase.CreateAsset(newItem, assetPath);
        AssetDatabase.SaveAssets();

        // Log the saved asset path
        Debug.Log("Item asset saved at: " + assetPath);
    }

}

