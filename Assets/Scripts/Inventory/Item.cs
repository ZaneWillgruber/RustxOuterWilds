using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Item Info")]
    //name of the item
    public string itemName;

    //description of the item
    [TextArea]
    public string itemDescription;

    //icon of the item that shows in inventory slot
    public Sprite icon;

    //max stack size of the item
    public int maxStack;

    //max connection of the item, if item does not degrade, set to 0
    public int maxCondition;

    //check if the item is stackable or degradable
    public bool isStackable { get { return maxStack > 1; } }
    public bool isDegradable { get { return maxCondition > -1; } }

    [Header("Item Type")]
    public GameObject interactableItem;

    [Header("Equiptable Informaton")]
    public bool isEquipable = false;
    public Vector3 equiptableOffsets;
    public Vector3 equiptableRotation;
    public GameObject handheld;
    public AudioClip swingSound;
    public AudioClip hitSound;

}
