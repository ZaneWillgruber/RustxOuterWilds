using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    public float speed;
    public float jumpForce;
    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    GameObject referencePlanet;
    Vector3 oldVelocity;
    bool moving = false;
    public ForceMode forceMode;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        referencePlanet = GetComponent<GravityController>().referencePlanet;

        //if player presses space, jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        }

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }

        if (moving == true && (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)) {
            // rb.AddForce(referencePlanet.GetComponent<Rigidbody>().velocity, forceMode);
            rb.velocity = referencePlanet.GetComponent<Rigidbody>().velocity;
            moving = false;
        } else if (moving == false && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)) {
            moving = true;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Vector3 moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
            oldVelocity = moveDirection;
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }
    }

    //detect if the player collided with the ground and set grounded to true
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            grounded = true;
            speed = speed * referencePlanet.GetComponent<Planet>().surfaceGravity;
        }
    }

    //detect if the player left the ground and set grounded to false
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            grounded = false;
            speed = speed / referencePlanet.GetComponent<Planet>().surfaceGravity;
        }
    }
}
