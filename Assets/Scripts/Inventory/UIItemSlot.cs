using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public bool isCursor;

    public ItemSlot itemSlot;
    public RectTransform slotRect;

    public Image icon;
    public TextMeshProUGUI amount;
    public Image condition;

    private void Awake()
    {

        itemSlot = new ItemSlot();

    }

    private void Update() {
        if(!isCursor) return;

        transform.position = Input.mousePosition;
    }

    public void RefreshSlot()
    {
        UpdateIcon();
        UpdateAmount();

        if (!isCursor)
        {
            UpdateConditionBar();
        }
    }

    public void ClearSlot() {
        itemSlot = new ItemSlot();
        RefreshSlot();
    }

    private void UpdateIcon()
    {
        if (itemSlot == null || !itemSlot.hasItem)
        {
            icon.enabled = false;
            return;
        }
        else icon.enabled = true;
        icon.sprite = itemSlot.item.icon;
    }

    private void UpdateAmount()
    {
        if (itemSlot == null || !itemSlot.hasItem || itemSlot.amount < 2)
        {
            amount.text = "";
            return;
        }
        else amount.text = itemSlot.amount.ToString();
    }
    private void UpdateConditionBar()
    {
        //condition bar is not needed
        if (itemSlot == null || !itemSlot.hasItem || !itemSlot.item.isDegradable)
        {
            condition.enabled = false;
            return;
        }
        else
        {
            condition.enabled = true;
            float conditionPercentage = (float)itemSlot.condition / (float)itemSlot.item.maxCondition;

            float barWidth = slotRect.rect.width * conditionPercentage;

            //set width of condition bar
            condition.rectTransform.sizeDelta = new Vector2(barWidth, condition.rectTransform.sizeDelta.y);

            //lerp color from green to red as condition decreases
            condition.color = Color.Lerp(Color.red, Color.green, conditionPercentage);
        }
    }
}
