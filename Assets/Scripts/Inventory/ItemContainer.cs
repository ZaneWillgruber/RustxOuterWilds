using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemContainer : MonoBehaviour
{
    public GameObject parentWindow;
    public Transform contentWindow;
    public TextMeshProUGUI title;

    public string containerName;
    GameObject SlotPrefab;
    public bool isOpen = false;

    public List<ItemSlot> items = new List<ItemSlot>();

    private void Start()
    {

        SlotPrefab = Resources.Load<GameObject>("Prefabs/UIItemSlot");

        #region Demo Code
        Debug.Log("Loading Demo Items");

        Item[] tempItems = new Item[3];
        tempItems[0] = Resources.Load<Item>("Items/axe");
        tempItems[1] = Resources.Load<Item>("Items/test cube");
        tempItems[2] = Resources.Load<Item>("Items/test sphere");

        for(int i = 0; i < 30; i++) {
            int index = Random.Range(0, 3);
            int amount = Random.Range(1, tempItems[index].maxStack);
            int condition = tempItems[index].maxCondition;

            items.Add(new ItemSlot(tempItems[index].name, amount, condition));
        }

        #endregion

    }

    List<UIItemSlot> UISlots = new List<UIItemSlot>();

    public void OpenContainer() {
        OpenContainer(items);
    }

    public void OpenContainer(List<ItemSlot> slots) {
        Debug.Log("Opening Container");
        parentWindow.SetActive(true);

        title.text = containerName.ToUpper();

        for(int i = 0; i < slots.Count; i++) {
            GameObject newSlot = Instantiate(SlotPrefab, contentWindow);

            newSlot.name = i.ToString();

            UISlots.Add(newSlot.GetComponent<UIItemSlot>());

            slots[i].AttachUI(UISlots[i]);
        }
    }

    public void CloseContainer () {
        foreach(UIItemSlot slot in UISlots) {
            slot.itemSlot.DetachUI();
            Destroy(slot.gameObject);
        }

        UISlots.Clear();
        parentWindow.SetActive(false);
    }
}
