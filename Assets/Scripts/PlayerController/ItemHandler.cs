using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public Transform camera;
    public ItemContainer playerInventory;

    void Update() {
        //shoot ray from camera, if it hits an InteractableItem, pick it up
        if(Input.GetKeyDown(KeyCode.E)) {
            RaycastHit hit;
            if(Physics.Raycast(camera.position, camera.forward, out hit, 5)) {
                if(hit.collider.tag == "InteractableItem") {
                    PickUpItem(hit.collider.gameObject);
                }
            }
        }
    }

    public void DropItem(ItemSlot slot) {
        Debug.Log("Dropping Item: " + slot.item.name);

        GameObject droppedItem = Instantiate(slot.item.interactableItem, camera.position, Quaternion.identity);
        
        //apply correct values to the InteractableItem
        InteractableItem item = droppedItem.GetComponent<InteractableItem>();
        item.item = slot.item;
        item.amount = slot.amount;
        item.condition = slot.condition;

        droppedItem.GetComponent<Rigidbody>().AddForce(camera.forward * 5, ForceMode.Impulse);
    }

    public void PickUpItem(GameObject item) {
        Debug.Log("Picking Up Item: " + item.GetComponent<InteractableItem>().item.name);

        //add item to inventory
        InteractableItem tempItem = item.GetComponent<InteractableItem>();
        ItemSlot itemToAdd = new ItemSlot(tempItem.item.itemName, tempItem.amount, tempItem.condition);

        playerInventory.AddItem(itemToAdd);

        Destroy(item);
    }
}
