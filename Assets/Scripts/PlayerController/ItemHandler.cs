using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public Transform camera;
    public ItemContainer playerInventory;

    private Transform _selection;
    public Rigidbody playerRb;

    void Update()
    {
        if (_selection != null)
        {
            var outline = _selection.GetComponent<Outline>();
            outline.enabled = false;
            _selection = null;
        }

        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, 5))
        {
            if (hit.collider.tag == "InteractableItem")
            {
                var selection = hit.transform;
                var outline = selection.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                }
                _selection = selection;
                //shoot ray from camera, if it hits an InteractableItem, pick it up
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem(selection.gameObject);
                }

            }
        }


    }

    public void DropItem(ItemSlot slot)
    {
        Debug.Log("Dropping Item: " + slot.item.name);

        GameObject droppedItem = Instantiate(slot.item.interactableItem, camera.position + (camera.transform.forward * 2), Quaternion.identity);

        //apply correct values to the InteractableItem
        InteractableItem item = droppedItem.GetComponent<InteractableItem>();
        item.item = slot.item;
        item.amount = slot.amount;
        item.condition = slot.condition;

        Rigidbody itemRb = droppedItem.GetComponent<Rigidbody>();

        itemRb.velocity = playerRb.velocity;
        itemRb.AddForce(camera.transform.forward * 5, ForceMode.Impulse);

    }

    public void PickUpItem(GameObject item)
    {
        Debug.Log("Picking Up Item: " + item.GetComponent<InteractableItem>().item.name);

        //add item to inventory
        InteractableItem tempItem = item.GetComponent<InteractableItem>();
        ItemSlot itemToAdd = new ItemSlot(tempItem.item.itemName, tempItem.amount, tempItem.condition);

        playerInventory.AddItem(itemToAdd);

        Destroy(item);
    }
}
