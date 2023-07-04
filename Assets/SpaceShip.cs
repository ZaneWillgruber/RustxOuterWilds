using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public bool manned;
    public GameObject player;
    public GameObject seat;
    Rigidbody rb;
    
    [Header("Movement")]
    public float speed;
    public float torqueSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(manned) {
            HandleMovement();
            HandleTorque();
        }
    }

    void HandleMovement() {
        Vector3 moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

        if(Input.GetKey(KeyCode.Space)) {
            rb.AddForce(transform.up * speed * 10f, ForceMode.Force);
        }
        if(Input.GetKey(KeyCode.LeftShift)) {
            rb.AddForce(-transform.up * speed * 10f, ForceMode.Force);
        }
    }

    void HandleTorque() {
        Vector3 torque = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        rb.AddTorque(transform.TransformDirection(torque * torqueSpeed));
    }
}
