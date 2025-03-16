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
    public GameObject itemDropPrefab;
    public new Transform transform;
    public string newItemName;
    public string newItemRarity;
    public Sprite newItemSprite;
    public int newItemBuffsLenght;
    public float newItemMinValueBuffsIncrease; //Increase the MIN values of the buffs
    public float newItemMaxValueBuffsIncrease; //Increase the MAX values of the buffs
    public SpriteArrays weaponSprites;
    public SpriteArrays shieldSprites;
    public SpriteArrays chestSprites;
    public SpriteArrays helmetSprites;
    public SpriteArrays legginSprites;


    public void GenerateLoot()
    {
        ItemCreator itemCreator = GetComponent<ItemCreator>();
        //Item Spawn Chance (in %)
        int ItemSpawnChance = 100;
        int randomValue = Random.Range(0, 100);
        if (randomValue < ItemSpawnChance)
        {
            // Determine Item Type
            ItemType newItemType = (ItemType)Random.Range(0, System.Enum.GetValues(typeof(ItemType)).Length);
            ItemTypeCase(newItemType);

            // Determine Item Rarity
            newItemRarity = GetRandomRarity(itemRarity);
            ItemRarityCase(newItemRarity);
            itemCreator.CreateNewItem(newItemName, newItemType, newItemSprite, newItemRarity, newItemBuffsLenght, newItemMinValueBuffsIncrease, newItemMaxValueBuffsIncrease);
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

        /*
    public class WeightedRandomDict : MonoBehaviour
    {
        void Start()
        {
            Dictionary<string, int> lootTable = new Dictionary<string, int>()
            {
                { "Common", 50 },
                { "Rare", 30 },
                { "Epic", 15 },
                { "Legendary", 5 }
            };

            string selectedItem = GetRandomWeightedItem(lootTable);
            Debug.Log("Selected Item: " + selectedItem);
        }

        string GetRandomWeightedItem(Dictionary<string, int> items)
        {
            int totalWeight = 0;
            foreach (var item in items.Values)
                totalWeight += item;

            int randomValue = UnityEngine.Random.Range(0, totalWeight);
            int currentSum = 0;

            foreach (var item in items)
            {
                currentSum += item.Value;
                if (randomValue < currentSum)
                    return item.Key;
            }

            return null;
        }
    }





            private void GenerateLoot()
            {

            }

            private void GenerateBuffs(BaseObject itemPrefabReference)
            {
                GameObject playerGameObject = GameObject.Find("Player"); // Find the player GameObject
                playerReference = playerGameObject.GetComponent<PlayerStats>(); // Get the PlayerStats component from the GameObject

                var buffs = itemPrefabReference.data.buffs;
                int[] buffsChance = { 30, 25, 20, 15, 10 };
                int buffsSize = GetRandomBuffSize(buffsChance);
                Debug.Log("The loot its going to have " + buffsSize + " buffs");
                ItemBuff[] newItemBuff = new ItemBuff[buffsSize];

                for (int i = 0; i < buffsSize; i++)
                {
                    int min = Random.Range(1, (playerReference.level * 2));
                    int max = Random.Range(1, (playerReference.level * 5));

                    Attributes selectedAttribute = (Attributes)Random.Range(0, System.Enum.GetValues(typeof(Attributes)).Length);
                    if (selectedAttribute == Attributes.Speed)
                    {
                        max = Random.Range(1, (playerReference.level * 5));
                        if (max >= 10)
                            max = 10;
                    }
                    newItemBuff[i] = new ItemBuff(min, max)
                    {
                        attributes = (Attributes)Random.Range(0, System.Enum.GetValues(typeof(Attributes)).Length),
                    };
                }
                itemPrefabReference.data.buffs = newItemBuff;

                //Attributes weight = GetRandomBuffs(buffsSize);

            }
            int GetRandomBuffSize(int[] buffsChance)
            {
                int[] buffWeights = new int[buffsChance.Length];
                buffWeights[0] = buffsChance[0];
                for (int i = 1; i < buffsChance.Length; i++)
                {
                    buffWeights[i] = buffWeights[i - 1] + buffsChance[i];
                }

                int randomBuffSize = Random.Range(0, buffWeights[buffWeights.Length - 1]);

                for (int i = 0; i < buffWeights.Length; i++)
                {
                    if (randomBuffSize < buffWeights[i])
                        return i;
                }

                return buffsChance[0];
            }

            private Attributes GetRandomBuffs(int buffsChance)
            {
                int[] cumulativeWeights = new int[buffsChance];
                cumulativeWeights[0] = buffsChance;
                for (int i = 1; i < buffsChance; i++)
                {
                    cumulativeWeights[i] = cumulativeWeights[i - 1] + buffsChance;
                }

                int randomValue = Random.Range(0, cumulativeWeights[cumulativeWeights.Length - 1]);



                return Attributes.Defence;
            }

        */
    }
}

