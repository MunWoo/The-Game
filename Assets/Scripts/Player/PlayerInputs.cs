using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInputs : MonoBehaviour
{
    List<GameObject> UiWindows = new List<GameObject>();
    public GameObject InventoryWindow;
    public GameObject EquipmentWindow;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            InventoryWindow.SetActive(!InventoryWindow.activeSelf);
            if (InventoryWindow.activeSelf)
                UiWindows.Add(InventoryWindow);
            if (!InventoryWindow.activeSelf)
                UiWindows.Remove(InventoryWindow);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            EquipmentWindow.SetActive(!EquipmentWindow.activeSelf);
            if (EquipmentWindow.activeSelf)
                UiWindows.Add(EquipmentWindow);
            if (!EquipmentWindow.activeSelf)
                UiWindows.Remove(EquipmentWindow);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UiWindows.Count >= 0)
            {
                UiWindows[UiWindows.Count - 1].SetActive(false);
                UiWindows.RemoveAt(UiWindows.Count - 1);
            }
            else Debug.Log("No more windows to close");
        }

    }
}
