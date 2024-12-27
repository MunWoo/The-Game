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
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
        CreateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            var obj = Instantiate(inventorySlotPrefab, inventoryUI.transform);
            Image itemSprite = obj.transform.Find("ItemSprite").GetComponent<Image>();
            TextMeshProUGUI itemAmount = obj.GetComponentInChildren<TextMeshProUGUI>();
            itemSprite.sprite = inventory.itemDatabase.GetItem[slot.item.Id].itemSprite;
            itemAmount.text = inventory.Container.Items[i].amount.ToString();
            itemsDisplayed.Add(slot, obj);
        }
    }


    public void Update()
    {
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            if (itemsDisplayed.ContainsKey(slot))
            {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString();
            }
            else
            {
                var obj = Instantiate(inventorySlotPrefab, inventoryUI.transform);
                Image itemSprite = obj.transform.Find("ItemSprite").GetComponent<Image>();
                TextMeshProUGUI itemAmount = obj.GetComponentInChildren<TextMeshProUGUI>();
                itemSprite.sprite = inventory.itemDatabase.GetItem[slot.item.Id].itemSprite;
                itemAmount.text = inventory.Container.Items[i].amount.ToString();
                itemsDisplayed.Add(slot, obj);
            }
        }
    }
}