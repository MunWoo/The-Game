using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDropDirector : MonoBehaviour
{
    public static ItemDropDirector instance;
    public GameObject itemDropPrefab;
    public ItemDatabase itemDatabase;
    public SpriteArrays borderSpriteArray;


    //Fetch the enemy type public enum of BaseEnemy
    private Dictionary<EnemyType, int> dropChance = new Dictionary<EnemyType, int>();

    class RarityData
    {
        public int OneBuffChance;
        public int TwoBuffChance;
        public int ThreeBuffChance;
        public int FourBuffChance;
        public int FixedBuffLength; // for Legendary/Mythical fixed buffs
        public float MinValueIncrease;
        public float MaxValueIncrease;
    }

    Dictionary<ItemRarity, RarityData> raritySettings = new Dictionary<ItemRarity, RarityData>()
    {
        { ItemRarity.Normal, new RarityData { FixedBuffLength = 1, MinValueIncrease = 1f, MaxValueIncrease = 1f } },
        { ItemRarity.Magic,  new RarityData { OneBuffChance = 60, TwoBuffChance = 40, MinValueIncrease = 1.1f, MaxValueIncrease = 1.2f } },
        { ItemRarity.Special,new RarityData { TwoBuffChance = 60, ThreeBuffChance = 40, MinValueIncrease = 1.2f, MaxValueIncrease = 1.5f } },
        { ItemRarity.Ultra,  new RarityData { FourBuffChance = 40, MinValueIncrease = 1.4f, MaxValueIncrease = 1.6f } },
        { ItemRarity.Legendary, new RarityData { FixedBuffLength = 4, MinValueIncrease = 1.8f, MaxValueIncrease = 2.2f } },
        { ItemRarity.Mythical, new RarityData { FixedBuffLength = 4, MinValueIncrease = 2.2f, MaxValueIncrease = 3.3f } }
    };



    Dictionary<ItemRarity, int> rarityWeights = new Dictionary<ItemRarity, int>()
        {
            { ItemRarity.Normal, 30 },
            { ItemRarity.Magic, 25 },
            { ItemRarity.Special, 20 },
            { ItemRarity.Ultra, 13 },
            { ItemRarity.Legendary, 8 },
            { ItemRarity.Mythical, 4 }
        };


    [Header("Item drop chances")]
    public int maxDropChance = 100;
    public int minDropChance = 0;


    [Header("Item Info")]

    public string newItemName; //
    public ItemRarity newItemRarity; //
    public Sprite newItemSprite; //
    public Sprite newItemSpriteBorder;
    public int newItemSpriteId; //
    public ItemBuff[] newItemBuffs; //
    public int newItemBuffsLength; //
    public float newItemMinValueBuffsIncrease; //Increase the MIN values of the buffs
    public float newItemMaxValueBuffsIncrease; //Increase the MAX values of the buffs
    public SpriteArrays weaponSprites;
    public SpriteArrays shieldSprites;
    public SpriteArrays chestSprites;
    public SpriteArrays helmetSprites;
    public SpriteArrays legginSprites;


    public void Awake()
    {
        instance = this;
        //Populate the dictionary with the chances (in %) to drop an item for each enemyType
        dropChance[EnemyType.Normal] = 20;
        dropChance[EnemyType.Boss] = 50;

    }

    //When an enemy dies, it asks the director for his chance to drop an item, then director returns the chance
    public (int chance, int min, int max) GetDropChance(EnemyType enemyType)
    {
        int min = minDropChance;
        int max = maxDropChance;


        if (dropChance.TryGetValue(enemyType, out int chance))
        {
            return (chance, minDropChance, maxDropChance);
        }

        else
        {
            Debug.Log("This enemyType: " + enemyType + " doesnt have a drop chance");
            return (0, 0, 100);
        }
    }

    //Drop an item ------------------------------------------------------------------
    public void SpawnItem(Vector3 position)
    {
        //Define what item type is going to drop, fetch the base from the pool, and apply the new info to that item, Instantiate the new object and put the new item in it
        //First roll for an item type, then copy the itemprefab and populate it with the information
        ItemType itemType = GetRandomItemType();

        //Switch Case to fetch the item sprite from the pools based on type
        FetchSprite(itemType);

        //Randomize the Rarity
        newItemRarity = GetRandomRarity();

        //Apply the buffs depending on the rarity and defines his border
        ItemRarityCase(newItemRarity);
        SetItemBorder(newItemRarity);

        //Set the ID for the new item (the last available number on the item database)
        int newId = itemDatabase.ItemObjects.Length;
        //Copy the item into a new created item
        BaseObject newItem = ScriptableObject.CreateInstance<BaseObject>();
        {
            newItem.itemName = newItemName;
            newItem.itemType = itemType;
            newItem.itemSprite = newItemSprite;
            newItem.itemRarityBorder = newItemSpriteBorder;
            newItem.itemRarity = newItemRarity;
            newItem.spriteId = newItemSpriteId;
            newItem.data = new Item
            {
                Id = newId,
                Name = newItemName
            };

            newItem.data.buffs = newItemBuffs;

        }

        //Save the item to get his ID before spawning the object
        AddItemToDatabase(newItem);

        //Then spawn the itemPrefab to populate it with the information of the new item
        GameObject itemDrop = Instantiate(itemDropPrefab, position, Quaternion.identity);

        //Populate the item in the itemDrop with the information
        GroundItem groundItem = itemDrop.GetComponent<GroundItem>();
        groundItem.itemInfo = newItem;

    }

    public ItemType GetRandomItemType()
    {
        var values = System.Enum.GetValues(typeof(ItemType));
        int randomIndex = UnityEngine.Random.Range(0, values.Length);
        return (ItemType)values.GetValue(randomIndex);
    }

    public void FetchSprite(ItemType itemType)
    {
        newItemName = null;
        switch (itemType)
        {
            case ItemType.Weapon:
                {
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

    }

    ItemRarity GetRandomRarity()
    {
        int totalWeight = 0;
        foreach (var rarity in raritySettings.Values)
        {
            // Assuming weights correspond to buff chances, sum for probability:
            // To simplify, assign weight to each rarity, here we pick a value:
            // Let's define weights matching your original dictionary:
            // Normal(30), Magic(25), Special(20), Ultra(13), Legendary(8), Mythical(4)
            // We'll store these in an array for example:
        }


        foreach (var weight in rarityWeights.Values)
            totalWeight += weight;

        int randomValue = Random.Range(0, totalWeight);
        int currentSum = 0;

        foreach (var rarity in rarityWeights)
        {
            currentSum += rarity.Value;
            if (randomValue < currentSum)
                return rarity.Key;
        }
        // Default fallback (shouldn't happen)
        return ItemRarity.Normal;
    }

    public void ItemRarityCase(ItemRarity rarity)
    {
        var data = raritySettings[rarity];
        int buffChances = Random.Range(0, 100);

        if (data.FixedBuffLength > 0)
        {
            newItemBuffsLength = data.FixedBuffLength;
        }
        else
        {
            // Assign buff length depending on chances and data thresholds

            // We check from highest to lowest chance to avoid overlaps
            if (rarity == ItemRarity.Magic)
            {
                if (buffChances <= data.TwoBuffChance)
                    newItemBuffsLength = 2;
                else if (buffChances <= data.OneBuffChance)
                    newItemBuffsLength = 1;
                else
                    newItemBuffsLength = 0;
            }
            else if (rarity == ItemRarity.Special)
            {
                if (buffChances <= data.ThreeBuffChance)
                    newItemBuffsLength = 3;
                else if (buffChances <= data.TwoBuffChance)
                    newItemBuffsLength = 2;
                else
                    newItemBuffsLength = 1;
            }
            else if (rarity == ItemRarity.Ultra)
            {
                if (buffChances <= data.FourBuffChance)
                    newItemBuffsLength = 4;
                else
                    newItemBuffsLength = 3;
            }
            else if (rarity == ItemRarity.Normal)
            {
                if (buffChances <= data.OneBuffChance)
                    newItemBuffsLength = 1;
                else
                    newItemBuffsLength = 0;
            }
            else
            {
                // Default fallback
                newItemBuffsLength = 0;
            }
        }

        newItemMinValueBuffsIncrease = data.MinValueIncrease;
        newItemMaxValueBuffsIncrease = data.MaxValueIncrease;
        SetBuffs(newItemBuffsLength, newItemMinValueBuffsIncrease, newItemMaxValueBuffsIncrease);

    }

    public void SetItemBorder(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Normal:
                newItemSpriteBorder = borderSpriteArray.itemSprites[0];
                break;
            case ItemRarity.Magic:
                newItemSpriteBorder = borderSpriteArray.itemSprites[1];
                break;
            case ItemRarity.Special:
                newItemSpriteBorder = borderSpriteArray.itemSprites[2];
                break;
            case ItemRarity.Ultra:
                newItemSpriteBorder = borderSpriteArray.itemSprites[3];
                break;
            case ItemRarity.Legendary:
                newItemSpriteBorder = borderSpriteArray.itemSprites[4];
                break;
            case ItemRarity.Mythical:
                newItemSpriteBorder = borderSpriteArray.itemSprites[5];
                break;
            default:
                Debug.Log("Theres no border for the rarity: " + rarity);
                newItemSpriteBorder = null;
                break;
        }

    }



    public void SetBuffs(int length, float minMultiplier, float maxMultiplier)
    {
        ItemBuff[] itemBuff = new ItemBuff[length];
        int level = 1; // default fallback level

        if (GlobalPlayerData.Instance != null)
        {
            level = GlobalPlayerData.Instance.playerLevel;
        }
        else
        {
            Debug.LogWarning("GlobalPlayerData.Instance is null in SetBuffs!");
        }

        for (int i = 0; i < length; i++)
        {
            Attributes attr = (Attributes)Random.Range(0, System.Enum.GetValues(typeof(Attributes)).Length);

            int min = Mathf.FloorToInt(5 + (level * minMultiplier));
            int max = Mathf.FloorToInt(1 + (level * maxMultiplier));

            if (attr == Attributes.Speed)
            {
                max = Mathf.Clamp(Random.Range(1, Mathf.FloorToInt(5 + (level * minMultiplier))), 1, 10);
            }

            itemBuff[i] = new ItemBuff(min, max) { attributes = attr };
        }
        newItemBuffs = itemBuff;

    }

    private void AddItemToDatabase(BaseObject item)
    {
        int currentLength = itemDatabase.ItemObjects.Length;
        System.Array.Resize(ref itemDatabase.ItemObjects, currentLength + 1);
        itemDatabase.ItemObjects[currentLength] = item;
    }





}
