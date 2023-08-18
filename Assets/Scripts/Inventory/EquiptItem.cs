using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquiptItem : MonoBehaviour
{
    public Transform hand;
    public Item equiptedItem;

    public void Equipt(Item item) {
        if (hand.childCount > 0) {
            Destroy(hand.GetChild(0).gameObject);
        }
        GameObject itemObject = Instantiate(item.handheld, hand);
        equiptedItem = item;
        // itemObject.transform.localPosition = Vector3.zero;
        // itemObject.transform.localRotation = Quaternion.identity;
    }

    public void Dequipt() {
        if (hand.childCount > 0) {
            Destroy(hand.GetChild(0).gameObject);
            equiptedItem = null;
        }
    }
}
