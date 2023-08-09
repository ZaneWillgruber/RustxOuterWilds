using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour
{
    public GameObject parentWindow;
    public Transform contentWindow;
    public TextMeshProUGUI title;
    public int maxSlots;

    public string containerName;
    GameObject SlotPrefab;
    public bool isOpen = false;
    public bool isHotbar = false;

    public Color defaultColor;
    Color selectedColor = Color.green;
    int selectedIndex = -1;
    EquiptItem equiptItemManager;

    public List<ItemSlot> items = new List<ItemSlot>();

    private void Start()
    {
        equiptItemManager = FindObjectOfType<EquiptItem>();
        SlotPrefab = Resources.Load<GameObject>("Prefabs/UIItemSlot");

        for (int i = 0; i < maxSlots; i++)
        {
            Debug.Log("Adding Empty Slot");

            items.Add(new ItemSlot());
        }

        if (isHotbar)
        {
            OpenContainer();
        }

        // #region Demo Code
        // Debug.Log("Loading Demo Items");

        // Item[] tempItems = new Item[3];
        // tempItems[0] = Resources.Load<Item>("Items/axe");
        // tempItems[1] = Resources.Load<Item>("Items/test cube");
        // tempItems[2] = Resources.Load<Item>("Items/test sphere");

        // for(int i = 0; i < 30; i++) {
        //     int index = Random.Range(0, 3);
        //     int amount = Random.Range(1, tempItems[index].maxStack);
        //     int condition = tempItems[index].maxCondition;

        //     items.Add(new ItemSlot(tempItems[index].name, amount, condition));
        // }

        // #endregion

    }

    List<UIItemSlot> UISlots = new List<UIItemSlot>();

    void Update()
    {
        if (!isHotbar) { return; }
        //set cell active based on hotbar index

        //on scroll up increase index
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedIndex++;
            if (selectedIndex > maxSlots - 1)
            {
                selectedIndex = 0;
                UISlots[maxSlots - 1].GetComponent<Image>().color = defaultColor;
            }

            if (selectedIndex > 0)
            {
                UISlots[selectedIndex - 1].GetComponent<Image>().color = defaultColor;
            }

            if (UISlots[selectedIndex].itemSlot.hasItem)
            {
                Debug.Log("Equipting Item");
                EquiptItem();
            }
            else
            {
                DequiptItem();
            }

            UISlots[selectedIndex].GetComponent<Image>().color = selectedColor;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = maxSlots - 1;
                UISlots[0].GetComponent<Image>().color = defaultColor;
            }

            if (selectedIndex < maxSlots - 1)
            {
                UISlots[selectedIndex + 1].GetComponent<Image>().color = defaultColor;
            }

            if (UISlots[selectedIndex].itemSlot.hasItem && items[selectedIndex].item.isEquipable)
            {
                Debug.Log("Equipting Item");
                EquiptItem();
            }
            else
            {
                DequiptItem();
            }

            UISlots[selectedIndex].GetComponent<Image>().color = selectedColor;
        }

    }

    void EquiptItem()
    {
        equiptItemManager.Equipt(items[selectedIndex].item);
    }

    void DequiptItem()
    {
        equiptItemManager.Dequipt();
    }

    public void OpenContainer()
    {
        OpenContainer(items);
    }

    public void OpenContainer(List<ItemSlot> slots)
    {
        Debug.Log("Opening Container");
        parentWindow.SetActive(true);

        title.text = containerName.ToUpper();

        for (int i = 0; i < slots.Count; i++)
        {
            GameObject newSlot = Instantiate(SlotPrefab, contentWindow);

            newSlot.name = i.ToString();

            UISlots.Add(newSlot.GetComponent<UIItemSlot>());

            slots[i].AttachUI(UISlots[i]);
        }
    }

    public void CloseContainer()
    {
        foreach (UIItemSlot slot in UISlots)
        {
            slot.itemSlot.DetachUI();
            Destroy(slot.gameObject);
        }

        UISlots.Clear();
        parentWindow.SetActive(false);
    }

    public void AddItem(ItemSlot item)
    {
        int index = FindNextEmptySlot();

        if (index == -1)
        {
            Debug.Log("No Empty Slots");
            return;
        }

        items[index] = item;

    }

    int FindNextEmptySlot()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (!items[i].hasItem)
                return i;
        }

        return -1;
    }
}
