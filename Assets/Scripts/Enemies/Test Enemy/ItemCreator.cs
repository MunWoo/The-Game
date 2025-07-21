using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public GameObject itemDropPrefab;
    public ItemDatabase itemDatabase;
    public ItemBuff[] buffsForItem;

    public void CreateNewItem(
        string newItemName,
        ItemType newItemType,
        Sprite newItemSprite,
        int newItemSpriteId,
        string newItemRarity,
        int buffCount,
        float buffMinMultiplier,
        float buffMaxMultiplier)
    {
        int newId = itemDatabase.ItemObjects.Length;

        // Create new BaseObject
        BaseObject newItem = ScriptableObject.CreateInstance<BaseObject>();
        {
            newItem.stackable = false;
            newItem.itemName = newItemName;
            newItem.itemType = newItemType;
            newItem.itemSprite = newItemSprite;
            newItem.spriteId = newItemSpriteId;
            newItem.data = new Item
            {
                Id = newId,
                Name = newItemName
            };
        }
        ;

        if (buffCount > 0)
        {
            newItem.data.buffs = GenerateBuffs(buffCount, buffMinMultiplier, buffMaxMultiplier);
        }

        AddItemToDatabase(newItem);
        SaveItemAsset(newItem);
        SpawnItemObject(newItem);

        Debug.Log($"{newItemName} | Type: {newItemType} | Rarity: {newItemRarity} | Buffs: {buffCount} | Buff Range: {buffMinMultiplier}/{buffMaxMultiplier}");
    }

    private ItemBuff[] GenerateBuffs(int count, float minMultiplier, float maxMultiplier)
    {
        PlayerStats player = GameObject.FindWithTag("Player")?.GetComponent<PlayerStats>();
        if (player == null)
        {
            Debug.LogWarning("PlayerStats not found!");
            return null;
        }

        ItemBuff[] buffs = new ItemBuff[count];
        int level = GlobalPlayerData.instance.playerLevel;

        for (int i = 0; i < count; i++)
        {
            Attributes attr = (Attributes)Random.Range(0, System.Enum.GetValues(typeof(Attributes)).Length);

            int min = Mathf.FloorToInt(5 + (level * minMultiplier));
            int max = Mathf.FloorToInt(1 + (level * maxMultiplier));

            if (attr == Attributes.Speed)
            {
                max = Mathf.Clamp(Random.Range(1, Mathf.FloorToInt(5 + (level * minMultiplier))), 1, 10);
            }

            buffs[i] = new ItemBuff(min, max) { attributes = attr };
        }

        buffsForItem = buffs; // optional if still needed externally
        return buffs;
    }

    private void AddItemToDatabase(BaseObject item)
    {
        int currentLength = itemDatabase.ItemObjects.Length;
        System.Array.Resize(ref itemDatabase.ItemObjects, currentLength + 1);
        itemDatabase.ItemObjects[currentLength] = item;
    }

    private void SpawnItemObject(BaseObject item)
    {
        GameObject drop = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        if (drop.TryGetComponent(out GroundItem groundItem))
        {
            groundItem.itemInfo = item;
        }
        else
        {
            Debug.LogWarning("GroundItem component not found on itemDropPrefab!");
        }
    }

    private void SaveItemAsset(BaseObject item)
    {
        string folderPath = "Assets/NewInventory/ScriptableObjects/Items";
        string assetPath = $"{folderPath}/Item_{item.data.Name}.asset";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/NewInventory/ScriptableObjects", "Items");
        }

        AssetDatabase.CreateAsset(item, assetPath);
        AssetDatabase.SaveAssets();
    }
}
