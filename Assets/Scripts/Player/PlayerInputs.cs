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
    public GameObject ItemDescriptionWindow;
    public GameObject TeleportWindow;
    public Gun02 gun02;


    private bool isCursorLocked = true;
    private bool setCursorActive = false;
    private bool setCursorHidden = true;

    void Start()
    {
        //gun02 = GetComponent<Gun02>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
            ToggleWindow(InventoryWindow);

        if (Input.GetKeyDown(KeyCode.C))
            ToggleWindow(EquipmentWindow);

        if (Input.GetKeyDown(KeyCode.Y))
            ToggleWindow(TeleportWindow);

        if (Input.GetKey(KeyCode.Q))
            gun02.ShootQ();

        if (Input.GetKeyDown(KeyCode.F8))
        {
            Vector3 pos = transform.position + transform.up * 1.5f;
            ItemDropDirector.instance.SpawnItem(pos);
        }



        if (Input.GetKeyDown(KeyCode.Escape))
            CloseLastWindow();

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ToggleCursorLock();
        }
    }

    private void ToggleWindow(GameObject window)
    {
        bool isActive = window.activeSelf;
        window.SetActive(!isActive);

        if (!isActive)
        {
            // Opening window
            UiWindows.Add(window);

            SetCursorState(setCursorActive);
        }
        else
        {
            // Closing window
            UiWindows.Remove(window);

            // If no more windows, restore previous cursor state
            if (UiWindows.Count == 0)
                SetCursorState(setCursorHidden);
        }
    }

    private void CloseLastWindow()
    {
        if (UiWindows.Count > 0)
        {
            GameObject lastWindow = UiWindows[UiWindows.Count - 1];
            lastWindow.SetActive(false);
            UiWindows.RemoveAt(UiWindows.Count - 1);

            if (UiWindows.Count == 0)
                SetCursorState(setCursorHidden);
        }
        else
        {
            Debug.Log("No more windows to close");
        }
    }

    private void ToggleCursorLock()
    {
        isCursorLocked = !isCursorLocked;
        SetCursorState(isCursorLocked);
        Debug.Log("Cursor is now " + (isCursorLocked ? "Locked and Invisible" : "Unlocked and Visible"));
    }

    private void SetCursorState(bool locked)
    {
        isCursorLocked = locked;
        Cursor.visible = !locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
