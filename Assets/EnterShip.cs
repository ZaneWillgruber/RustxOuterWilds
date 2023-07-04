using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterShip : MonoBehaviour
{

    public float rayDistance;
    public LayerMask whatIsShip;
    public bool inShip = false;
    GameObject player;
    SpaceShip ship;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject;

        //lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inShip == false)
        {
            HandleEnteringShip();
        }
        else if (Input.GetKeyDown(KeyCode.F) && inShip == true)
        {
            HandleExitingShip();
        }
    }

    void HandleEnteringShip()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance, whatIsShip))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            ship = hit.collider.gameObject.GetComponentInParent<SpaceShip>();
            inShip = true;
            player.transform.position = ship.seat.transform.position + new Vector3(0, player.transform.localScale.y, 0);
            player.transform.rotation = ship.seat.transform.rotation;
            player.GetComponent<Rigidbody>().isKinematic = true;
            hit.collider.gameObject.GetComponentInParent<SpaceShip>().manned = true;
            hit.collider.gameObject.GetComponentInParent<SpaceShip>().player = player;
            player.transform.parent = ship.seat.transform;
            GetComponentInParent<GravityController>().enabled = false;
            GetComponentInParent<PlayerMovement>().enabled = false;
            GetComponent<MouseLook>().enabled = false;
        }
    }

    void HandleExitingShip()
    {
        inShip = false;
        player.transform.parent = null;
        player.GetComponent<Rigidbody>().isKinematic = false;
        ship.manned = false;
        GetComponentInParent<GravityController>().enabled = true;
        GetComponentInParent<PlayerMovement>().enabled = true;
        GetComponent<MouseLook>().enabled = true;
        player.GetComponent<Rigidbody>().velocity = ship.GetComponent<Rigidbody>().velocity;
    }
}
