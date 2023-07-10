using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    Rigidbody rb;
    public float orbitSpeed;
    public Planet orbitingPlanet;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = orbitingPlanet.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion q = Quaternion.AngleAxis(orbitSpeed, transform.forward);
        rb.MovePosition(q * (rb.transform.position - target.position) + target.position);
        rb.MoveRotation(rb.transform.rotation * q);
    }
}
