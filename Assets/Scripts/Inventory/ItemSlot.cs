using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    public Item item;

    private int _amount;
    public int amount
    {
        get { return _amount; }
        set
        {
            if (item == null) _amount = 0;
            else if (value > item.maxStack) _amount = item.maxStack;
            else if (value < 1) _amount = 0;
            else _amount = value;
            RefreshUISlot();
        }
    }

    private int _condition;
    public int condition
    {
        get { return _condition; }
        set
        {

            if (item == null) _condition = 0;
            else if (value > item.maxCondition) _condition = item.maxCondition;
            else if (value < 0) condition = 0;
            else _condition = value;
            RefreshUISlot();
        }
    }

    private UIItemSlot uiItemSlot;
    public void AttachUI(UIItemSlot uiSlot)
    {
        uiItemSlot = uiSlot;
        uiItemSlot.itemSlot = this;
        RefreshUISlot();
    }
    public void DetachUI()
    {
        uiItemSlot.ClearSlot();
        uiItemSlot = null;
    }

    public bool isAttachedToUI { get { return uiItemSlot != null; } }

    public void RefreshUISlot()
    {
        if (!isAttachedToUI) return;

        uiItemSlot.RefreshSlot();
    }

    public static bool Compare(ItemSlot slotA, ItemSlot slotB)
    {
        if (slotA == null || slotB == null) return false;
        if (slotA.item == null || slotB.item == null) return false;
        if (slotA.item != slotB.item) return false;
        if (slotA.condition != slotB.condition) return false;
        return true;
    }

    public static void Swap(ItemSlot slotA, ItemSlot slotB)
    {
        Item _item = slotA.item;
        int _amount = slotA.amount;
        int _condition = slotA.condition;

        slotA.item = slotB.item;
        slotA.amount = slotB.amount;
        slotA.condition = slotB.condition;

        slotB.item = _item;
        slotB.amount = _amount;
        slotB.condition = _condition;

        slotA.RefreshUISlot();
        slotB.RefreshUISlot();
    }

    public void Clear()
    {
        item = null;
        amount = 0;
        condition = 0;
        RefreshUISlot();
    }

    public bool hasItem { get { return (item != null); } }

    private Item FindByName(string itemName)
    {
        itemName = itemName.ToLower();
        Item _item = Resources.Load<Item>(string.Format("Items/{0}", itemName));

        if (_item == null)
        {
            Debug.LogWarning(string.Format("Item \"{0}\" not found! Make sure it is a lowercase file name!", itemName));
        }

        return _item;
    }

    public ItemSlot(string itemName, int _amount = 1, int _condition = 0)
    {
        Item _item = FindByName(itemName);

        if (_item == null)
        {
            item = null;
            amount = 0;
            condition = 0;
            return;
        }
        else
        {
            item = _item;
            amount = _amount;
            condition = _condition;
        }
    }

    public ItemSlot()
    {
        item = null;
        amount = 0;
        condition = 0;
    }

}
