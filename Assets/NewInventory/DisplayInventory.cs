using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DisplayInventory : MonoBehaviour
{/*
    //public MouseItem mouseItem = new MouseItem();
    public GameObject inventoryUI;
    public Inventory inventory;
    public GameObject inventorySlotPrefab;
    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        CreateSlots();
    }

    public void CreateSlots()
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
        
    }


    public void Update()
    {
        //UpdateDisplay();
        UpdateSlots();
    }
    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            if (_slot.Value.item.Id  >= 0)
            {
                _slot.Key.transform.Find("ItemSprite").GetComponent<Image>().sprite = inventory.itemDatabase.GetItem[_slot.Value.item.Id ].itemSprite;
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString();

            }
            else
            {
                _slot.Key.transform.Find("ItemSprite").GetComponent<Image>().sprite = null;
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
    public void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }
    public void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }
    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(80, 80);
        mouseObject.transform.SetParent(transform.parent.parent);
        if (itemsDisplayed[obj].item.Id  >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.itemDatabase.GetItem[itemsDisplayed[obj].item.Id].itemSprite;
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj];
    }
    public void OnDragEnd(GameObject obj)
    {
        if (mouseItem.hoverObj)
        {
            inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
        }
        else
        {
            //What to do when dropped outside an inventorySlot
            //Update to have a checkbox to confirm deletion
            inventory.RemoveItem(itemsDisplayed[obj].item);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }
    public void OnDrag(GameObject obj)
    {
        if (mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

*/
}