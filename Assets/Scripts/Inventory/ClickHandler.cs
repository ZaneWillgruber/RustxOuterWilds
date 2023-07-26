using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    GraphicRaycaster raycaster;
    PointerEventData pointer;
    EventSystem eventSystem;

    public UIItemSlot cursor;

    private void Awake()
    {
        raycaster = GetComponentInParent<GraphicRaycaster>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointer = new PointerEventData(eventSystem);
            pointer.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            raycaster.Raycast(pointer, results);

            if (results.Count > 0 && results[0].gameObject.tag == "UIItemSlot")
            {
                ProcessClick(results[0].gameObject.GetComponent<UIItemSlot>());
            }
        }
    }

    private void ProcessClick(UIItemSlot clicked)
    {
        //catch nulls
        if (clicked == null)
        {
            Debug.LogWarning("UI Element is tagged as UIItemSlot but does not have a UIItemSlot component!");
            return;
        }

        if (!ItemSlot.Compare(cursor.itemSlot, clicked.itemSlot))
        {
            ItemSlot.Swap(cursor.itemSlot, clicked.itemSlot);
            cursor.RefreshSlot();
            return;
        }
        else
        {

            if (!cursor.itemSlot.hasItem)
                return;
            if(!cursor.itemSlot.item.isStackable)
                return;

            if(clicked.itemSlot.amount == clicked.itemSlot.item.maxStack)
                return;

            //add amounts
            int total = cursor.itemSlot.amount + clicked.itemSlot.amount;
            int maxStack = cursor.itemSlot.item.maxStack;

            if(total <= maxStack) {
                clicked.itemSlot.amount = total;
                cursor.itemSlot.Clear();
            }
            else {
                clicked.itemSlot.amount = maxStack;
                cursor.itemSlot.amount = total - maxStack;
            }

            cursor.RefreshSlot();

        }

    }

}
