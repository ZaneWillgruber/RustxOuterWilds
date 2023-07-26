using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public MouseLook mouseLook;
    public EnterShip enterShip;
    public ItemContainer inventory;

    bool inInventory = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inInventory = !inInventory;
            if (inInventory)
            {
                EnterInventory();
            }
            else
            {
                ExitInventory();
            }
        }
    }

    void EnterInventory()
    {
        if(inventory.isOpen) return;
        inventory.isOpen = true;
        inventory.OpenContainer();

        //unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mouseLook.enabled = false;
        enterShip.enabled = false;
    }

    void ExitInventory()
    {
        if(!inventory.isOpen) return;
        inventory.isOpen = false;
        inventory.CloseContainer();

        //lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mouseLook.enabled = true;
        enterShip.enabled = true;
    }
}
