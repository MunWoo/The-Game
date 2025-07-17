using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemDescriptionWindow : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    public GameObject descriptionWindow;
    public GameObject imageObject;
    public GameObject itemName;
    public GameObject itemDescription;
    public GameObject attribute1;
    public GameObject attribute2;
    public GameObject attribute3;
    public GameObject attribute4;
    private TextMeshProUGUI[] textComponents;
    private GameObject[] gameObjectsArray;
    private Color textColor;
    public void DisplayDescription(InventorySlot obj)
    {
        Image image = imageObject.GetComponent<Image>();
        TextMeshProUGUI name = itemName.GetComponent<TextMeshProUGUI>();
        if (obj.item.Id >= 0)
        {
            //Display the window
            descriptionWindow.SetActive(true);

            //Get the image of the item
            if (itemDatabase.ItemObjects[obj.item.Id].itemSprite != null)
                image.sprite = itemDatabase.ItemObjects[obj.item.Id].itemSprite;
            else Debug.Log("There is no sprite in database of " + obj.item.Id + " ID");

            //Get the name of the item
            if (obj.item.Name != null)
                name.text = obj.item.Name;
            else Debug.Log("This item has no name in the InventorySlot class or the database");

            //Display the description of the object     (if it has one) update this one
            TextMeshProUGUI description = itemDescription.GetComponent<TextMeshProUGUI>();
            GetDescriptionColor(obj.BaseObject.itemRarity);
            description.color = textColor;
            description.text = obj.BaseObject.itemRarity.ToString();


            //Get attributes
            // Initialize the array of TextMeshProUGUI components
            textComponents = new TextMeshProUGUI[]
            {
            attribute1.GetComponent<TextMeshProUGUI>(),
            attribute2.GetComponent<TextMeshProUGUI>(),
            attribute3.GetComponent<TextMeshProUGUI>(),
            attribute4.GetComponent<TextMeshProUGUI>()
            };

            //Initialize the array of GameObjects to set active to false
            gameObjectsArray = new GameObject[]
            {
                attribute1,
                attribute2,
                attribute3,
                attribute4
            };
            //Check how many buff slots are ocupied
            int occupiedSlots = 0;
            foreach (var slot in obj.item.buffs)
            {
                if (slot != null)
                {
                    occupiedSlots++;
                }
                else break;
            }
            //Populate the text components with the buffs and deactivate the unused ones
            for (int i = 0; i < textComponents.Length; i++)
            {
                if (i < occupiedSlots)
                {
                    gameObjectsArray[i].SetActive(true);
                    textComponents[i].text = $"{obj.item.buffs[i].attributes} + {obj.item.buffs[i].value}";
                }
                else
                {
                    gameObjectsArray[i].SetActive(false);
                }
            }
        }

    }

    public void GetDescriptionColor(ItemRarity itemRarity)
    {
        switch (itemRarity)
        {
            case ItemRarity.Normal:
                textColor = Color.white;
                break;
            case ItemRarity.Magic:
                textColor = Color.blue;
                break;
            case ItemRarity.Special:
                textColor = new Color(0.976f, 0.702f, 0.196f); // Burned Yellow
                break;
            case ItemRarity.Ultra:
                textColor = new Color(0.18f, 0.173f, 0.494f); // Darked Blue
                break;
            case ItemRarity.Legendary:
                textColor = new Color(0.914f, 0.31f, 0.102f); // Orange
                break;
            case ItemRarity.Mythical:
                textColor = new Color(0.902f, 0f, 0.494f); // Pinkish Purple
                break;
            default:
                Debug.LogWarning($"No color defined for rarity: {itemRarity}");
                textColor = Color.gray;
                break;
        }
    }

    public void CloseDisplayWindow()
    {
        //Close the window
        descriptionWindow.SetActive(false);
    }
}
