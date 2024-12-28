using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    public GameObject inventorySlotPrefab;

    public override void CreateSlots()
    {
        //Makes sure it creates a new dictionary
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventorySlotPrefab, inventoryUI.transform);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }


        /*
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            var obj = Instantiate(inventorySlotPrefab, inventoryUI.transform);
            Image itemSprite = obj.transform.Find("ItemSprite").GetComponent<Image>();
            TextMeshProUGUI itemAmount = obj.GetComponentInChildren<TextMeshProUGUI>();
            itemSprite.sprite = inventory.itemDatabase.GetItem[slot.item.Id].itemSprite;
            itemAmount.text = inventory.Container.Items[i].amount.ToString();
            itemsDisplayed.Add(slot, obj);
        }
        */
    }




}
