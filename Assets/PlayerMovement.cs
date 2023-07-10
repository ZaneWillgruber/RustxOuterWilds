using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    public float speed;
    public float spaceSpeed;
    public float groundedSpeed;
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
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;


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
            speed = groundedSpeed;
        }
        else
        {
            rb.drag = 0f;
            speed = spaceSpeed;
        }

        // if (moving == true && (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))
        // {
        //     // rb.AddForce(referencePlanet.GetComponent<Rigidbody>().velocity, forceMode);
        //     rb.velocity = referencePlanet.GetComponent<Rigidbody>().velocity;
        //     moving = false;
        // }
        // else if (moving == false && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        // {
        //     moving = true;
        // }

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
        Vector3 targetMoveAmount = moveDir * groundedSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            MovePlayer();
        }
        else
        {
            MovePlayerInAir();
        }
    }

    private void MovePlayer()
    {
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + localMove);
    }

    private void MovePlayerInAir()
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
        }
    }

    //detect if the player left the ground and set grounded to false
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            grounded = false;
        }
    }

    void SpeedControl()
    {
        Vector3 velocity = rb.velocity;

        if (referencePlanet != null)
        {
            if ((velocity - referencePlanet.GetComponent<Rigidbody>().velocity).magnitude > speed)
            {
                Vector3 limitedVelocity = velocity.normalized * speed;
                rb.velocity = limitedVelocity + referencePlanet.GetComponent<Rigidbody>().velocity;
            }
        }
    }
}
