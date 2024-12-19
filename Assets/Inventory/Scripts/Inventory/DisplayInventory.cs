using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject inventorySlot;
    public Transform content;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    void Start()
    {
        CreateDisplay();
    }

    public void CreateDisplay()
    {
        // Clear existing items in the content before populating
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // Loop through each item in the inventory container
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            // Instantiate the InventorySlot prefab (a GameObject)
            var obj = Instantiate(inventorySlot, content);  // Instantiate in the 'content' parent

            // Get the ItemName and ItemAmount text components by finding their child names
            TMP_Text itemNameText = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            TMP_Text itemAmountText = obj.transform.Find("ItemAmount").GetComponent<TMP_Text>();

            // Set the item name and amount based on the current inventory slot
            itemNameText.text = inventory.Container[i].item.name;
            itemAmountText.text = $"x{inventory.Container[i].amount}";
        }
    }


    public void UpdateDisplay()
    {
        // Clear existing items in the content before populating
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
            break;
        }

        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                // If it exists, update the ItemAmount
                TMP_Text itemAmountText = itemsDisplayed[inventory.Container[i]].transform.Find("ItemAmount").GetComponent<TMP_Text>();
                itemAmountText.text = $"x{inventory.Container[i].amount}";  // Set the new amount
            }
            else
            {
                // Instantiate the InventorySlot prefab (a GameObject)
                var obj = Instantiate(inventorySlot, content);  // Instantiate in the 'content' parent

                // Get the ItemName and ItemAmount text components by finding their child names
                Image itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
                TMP_Text itemNameText = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
                TMP_Text itemAmountText = obj.transform.Find("ItemAmount").GetComponent<TMP_Text>();

                // Set the item name and amount based on the current inventory slot
                itemNameText.text = inventory.Container[i].item.name;
                itemIcon.sprite = inventory.Container[i].item.sprite;
                itemAmountText.text = $"x{inventory.Container[i].amount}";
            }
        }
    }

}
