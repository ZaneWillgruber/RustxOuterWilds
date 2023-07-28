using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    GraphicRaycaster raycaster;
    PointerEventData pointer;
    EventSystem eventSystem;
    UIItemSlot startDragSlot;
    UIItemSlot endDragSlot;

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

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
        // pointer = new PointerEventData(eventSystem);
        // pointer.position = Input.mousePosition;

        // List<RaycastResult> results = new List<RaycastResult>();

        // raycaster.Raycast(pointer, results);

        // if (results.Count > 0 && results[0].gameObject.tag == "UIItemSlot")
        // {
        //     ProcessClick(results[0].gameObject.GetComponent<UIItemSlot>());
        // }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        pointer = new PointerEventData(eventSystem);
        pointer.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointer, results);

        if (results.Count > 0 && results[0].gameObject.tag == "UIItemSlot")
        {
            startDragSlot = results[0].gameObject.GetComponent<UIItemSlot>();
            ProcessClick(startDragSlot);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        Debug.Log("Begin Drag");
        pointer = new PointerEventData(eventSystem);
        pointer.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointer, results);

        if (results.Count > 0 && results[0].gameObject.tag == "UIItemSlot")
        {
            endDragSlot = results[0].gameObject.GetComponent<UIItemSlot>();
            ProcessClick(endDragSlot);
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
            ItemSlot.Swap(startDragSlot.itemSlot, clicked.itemSlot);
            ItemSlot.Swap(cursor.itemSlot, clicked.itemSlot);
            cursor.RefreshSlot();
            return;
        }
        else
        {

            if (!cursor.itemSlot.hasItem)
                return;
            if (!cursor.itemSlot.item.isStackable)
            {
                ItemSlot.Swap(startDragSlot.itemSlot, clicked.itemSlot);
                ItemSlot.Swap(cursor.itemSlot, clicked.itemSlot);
                cursor.RefreshSlot();
                return;
            }

            if (clicked.itemSlot.amount == clicked.itemSlot.item.maxStack)
                return;

            //add amounts
            int total = cursor.itemSlot.amount + clicked.itemSlot.amount;
            int maxStack = cursor.itemSlot.item.maxStack;

            if (total <= maxStack)
            {
                clicked.itemSlot.amount = total;
                cursor.itemSlot.Clear();
            }
            else
            {
                clicked.itemSlot.amount = maxStack;
                cursor.itemSlot.amount = total - maxStack;
                ItemSlot.Swap(cursor.itemSlot, startDragSlot.itemSlot);
            }

            cursor.RefreshSlot();

        }

    }

}
