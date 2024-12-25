using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryUI;
    public Inventory inventory;
    public GameObject inventorySlotPrefab;

    public void UpdateInventory()
    {
        // Clear the current UI slots (optional: if inventory slots persist between updates)
        foreach (Transform child in inventoryUI.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < inventory.Container.Count; i++)
        {

            var obj = Instantiate(inventorySlotPrefab, inventoryUI.transform);
            Image itemSprite = obj.transform.Find("ItemSprite").GetComponent<Image>();
            TextMeshProUGUI itemAmount = obj.GetComponentInChildren<TextMeshProUGUI>();
            itemSprite.sprite = inventory.Container[i].item.itemSprite;
            itemAmount.text = inventory.Container[i].amount.ToString();
        }
    }
}