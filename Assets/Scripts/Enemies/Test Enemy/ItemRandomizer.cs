using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ItemRandomizer : MonoBehaviour
{
    Dictionary<string, int> itemRarity = new Dictionary<string, int>()
{
    {"Normal", 30},
    {"Magic", 25},
    {"Special", 20},
    {"Ultra", 13},
    {"Legendary", 8},
    {"Mythical", 4}
};

    [Header("Item Info")]
    public string newItemName;
    public string newItemRarity;
    public Sprite newItemSprite;
    public int newItemSpriteId;
    public int newItemBuffsLenght;
    public float newItemMinValueBuffsIncrease; //Increase the MIN values of the buffs
    public float newItemMaxValueBuffsIncrease; //Increase the MAX values of the buffs
    public SpriteArrays weaponSprites;
    public SpriteArrays shieldSprites;
    public SpriteArrays chestSprites;
    public SpriteArrays helmetSprites;
    public SpriteArrays legginSprites;

    private ItemCreator itemCreator;

    public void Start()
    {
        itemCreator = GetComponent<ItemCreator>();
    }
    public void GenerateLoot()
    {
        int ItemSpawnChance = 10; //Item Spawn Chance (in %).
        int randomValue = Random.Range(0, 100);
        if (randomValue < ItemSpawnChance)
        {
            // Determine Item Type
            ItemType newItemType = (ItemType)Random.Range(0, System.Enum.GetValues(typeof(ItemType)).Length);
            ItemTypeCase(newItemType);

            // Determine Item Rarity
            newItemRarity = GetRandomRarity(itemRarity);
            ItemRarityCase(newItemRarity);
            itemCreator.CreateNewItem(newItemName, newItemType, newItemSprite, newItemSpriteId, newItemRarity, newItemBuffsLenght, newItemMinValueBuffsIncrease, newItemMaxValueBuffsIncrease);
            //Debug.Log($"{newItemName} of Type: {newItemType}, Rarity: {newItemRarity}, Buffs: {newItemBuffsLenght}, Min/Max Buff: {newItemMinValueBuffsIncrease}/{newItemMaxValueBuffsIncrease}");
        }
    }


    public void ItemTypeCase(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                {
                    newItemName = null;
                    var randomIndex = Random.Range(0, weaponSprites.itemSprites.Length - 1);
                    newItemSpriteId = randomIndex;
                    newItemSprite = weaponSprites.itemSprites[randomIndex];
                    switch (randomIndex)
                    {
                        default:
                            if (randomIndex >= 0 && randomIndex <= 1)
                            {
                                newItemName = "Sword";
                            }
                            else if (randomIndex >= 2 && randomIndex <= 3)
                            {
                                newItemName = "Pole";
                            }
                            if (randomIndex >= 4 && randomIndex <= 5)
                            {
                                newItemName = "Axe";
                            }
                            break;
                    }

                }

                break;

            case ItemType.Shield:
                {
                    newItemName = null;
                    var randomIndex = Random.Range(0, shieldSprites.itemSprites.Length - 1);
                    newItemSpriteId = randomIndex;
                    newItemSprite = shieldSprites.itemSprites[randomIndex];
                    switch (randomIndex)
                    {
                        default:
                            if (randomIndex >= 0 && randomIndex <= 1)
                            {
                                newItemName = "Shield";
                            }
                            else if (randomIndex >= 2 && randomIndex <= 3)
                            {
                                newItemName = "Round Shield";
                            }
                            if (randomIndex >= 4 && randomIndex <= 5)
                            {
                                newItemName = "Rectangle Shield";
                            }
                            break;
                    }
                }
                break;

            case ItemType.Chestplate:
                {
                    newItemName = null;
                    var randomIndex = Random.Range(0, chestSprites.itemSprites.Length - 1);
                    newItemSpriteId = randomIndex;
                    newItemSprite = chestSprites.itemSprites[randomIndex];
                    switch (randomIndex)
                    {
                        default:
                            if (randomIndex >= 0 && randomIndex <= 1)
                            {
                                newItemName = "Chainmail";
                            }
                            else if (randomIndex >= 2 && randomIndex <= 3)
                            {
                                newItemName = "Vest";
                            }
                            if (randomIndex >= 4 && randomIndex <= 5)
                            {
                                newItemName = "T-Shirt";
                            }
                            break;
                    }
                }
                break;

            case ItemType.Leggins:
                {
                    newItemName = null;
                    var randomIndex = Random.Range(0, legginSprites.itemSprites.Length - 1);
                    newItemSpriteId = randomIndex;
                    newItemSprite = legginSprites.itemSprites[randomIndex];
                    switch (randomIndex)
                    {
                        default:
                            if (randomIndex >= 0 && randomIndex <= 1)
                            {
                                newItemName = "Boots";
                            }
                            else if (randomIndex >= 2 && randomIndex <= 3)
                            {
                                newItemName = "High Boots";
                            }
                            if (randomIndex >= 4 && randomIndex <= 5)
                            {
                                newItemName = "Chinelos";
                            }
                            break;
                    }
                }
                break;

            case ItemType.Helmet:
                {
                    newItemName = null;
                    var randomIndex = Random.Range(0, helmetSprites.itemSprites.Length - 1);
                    newItemSpriteId = randomIndex;
                    newItemSprite = helmetSprites.itemSprites[randomIndex];
                    switch (randomIndex)
                    {
                        default:
                            if (randomIndex >= 0 && randomIndex <= 1)
                            {
                                newItemName = "Helmet";
                            }
                            else if (randomIndex >= 2 && randomIndex <= 3)
                            {
                                newItemName = "Spike Helmet";
                            }
                            if (randomIndex >= 4 && randomIndex <= 5)
                            {
                                newItemName = "Dull Helmet";
                            }
                            break;
                    }
                }
                break;
        }
        ;

    }

    string GetRandomRarity(Dictionary<string, int> rarity)
    {
        int totalWeight = 0;
        foreach (var item in rarity.Values)
        {
            totalWeight += item;
        }

        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int currentSum = 0;

        foreach (var item in rarity)
        {
            currentSum += item.Value;
            if (randomValue < currentSum)
                return item.Key;
        }
        return null;
    }

    public void ItemRarityCase(string itemRarity)
    {
        switch (itemRarity)
        {
            case "Normal":
                {
                    var oneBuffChance = 40; //value in %
                    var buffChances = Random.Range(0, 100);
                    if (buffChances <= oneBuffChance)
                    {
                        newItemBuffsLenght = 1;
                    }
                    else newItemBuffsLenght = 0;

                    newItemMaxValueBuffsIncrease = 1;
                    newItemMinValueBuffsIncrease = 1;
                }
                break;
            case "Magic":
                {
                    var oneBuffChance = 40; //Value in %
                    var twoBuffChance = 20;

                    var buffChances = Random.Range(0, 100);
                    if (buffChances <= twoBuffChance)
                        newItemBuffsLenght = 2;
                    else
                    if (buffChances <= oneBuffChance)
                        newItemBuffsLenght = 1;
                    else
                        newItemBuffsLenght = 0;

                    newItemMaxValueBuffsIncrease = 1.2F;
                    newItemMinValueBuffsIncrease = 1.1F;

                }
                break;

            case "Special":
                {
                    var twoBuffChance = 40; //Value in %
                    var threeBuffChance = 20;

                    var buffChances = Random.Range(0, 100);
                    if (buffChances <= threeBuffChance)
                        newItemBuffsLenght = 3;
                    else
                    if (buffChances <= twoBuffChance)
                        newItemBuffsLenght = 2;
                    else
                        newItemBuffsLenght = 1;

                    newItemMaxValueBuffsIncrease = 1.4F;
                    newItemMinValueBuffsIncrease = 1.2F;

                }
                break;

            case "Ultra":
                {
                    var fourBuffChance = 40; //Value in %

                    var buffChances = Random.Range(0, 100);
                    if (buffChances <= fourBuffChance)
                        newItemBuffsLenght = 4;
                    else
                        newItemBuffsLenght = 3;

                    newItemMaxValueBuffsIncrease = 1.6F;
                    newItemMinValueBuffsIncrease = 1.3F;

                }
                break;

            case "Legendary":
                {
                    newItemBuffsLenght = 4;

                    newItemMaxValueBuffsIncrease = 2F;
                    newItemMinValueBuffsIncrease = 1.8F;

                }
                break;

            case "Mythical":
                {
                    newItemBuffsLenght = 4;

                    newItemMaxValueBuffsIncrease = 4F;
                    newItemMinValueBuffsIncrease = 2F;

                }
                break;

        }
    }
}

