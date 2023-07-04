using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 500f;
    public Transform playerBody;
    public bool inAir = false;
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        inAir = !GetComponentInParent<PlayerMovement>().grounded;
        if (inAir)
        {
            HandleInAir();
        }
        else
        {
            HandleOnGround();
        }

    }

    void HandleInAir()
    {
        xRotation = 0f;

        //if camera is not at 0,0,0 roation, reset it with lerp
        if (transform.localRotation != Quaternion.Euler(0f, 0f, 0f))
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0f, 0f, 0f), 0.01f);
        }

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.right * -mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void HandleOnGround()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
