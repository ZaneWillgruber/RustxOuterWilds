using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{

    public GameObject referencePlanet;
    Rigidbody rb;
    Vector3 velocity;
    bool grounded;
    public bool rotate = false;
    public LayerMask whatIsPlanet;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = GetComponent<PlayerMovement>().grounded;

        //find closest planet
        float closestDistance = Mathf.Infinity;
        foreach (GameObject planet in GameObject.FindGameObjectsWithTag("Planet"))
        {
            float distance = Vector3.Distance(transform.position, planet.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                referencePlanet = planet;
            }
        }

        //rotate to land on planet
        if (rotate)
        {
            Vector3 dir = (transform.position - referencePlanet.transform.position).normalized;
            Quaternion toRotation = Quaternion.FromToRotation(transform.up, dir) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, .01f);
        }


        //apply gravity if in gravity collider
        if (referencePlanet != null)
        {
            Vector3 gravityDirection = (transform.position - referencePlanet.transform.position).normalized;
            //float gravity = ((referencePlanet.GetComponent<Planet>().mass * GetComponent<Rigidbody>().mass)) / Mathf.Pow(closestDistance, 2);
            float sqrDist = (transform.position - referencePlanet.transform.position).sqrMagnitude;

            Vector3 gravity = gravityDirection * Universe.gravitationalConstant * referencePlanet.GetComponent<Rigidbody>().mass / sqrDist;
            GetComponent<Rigidbody>().AddForce(-gravity, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            rotate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            rotate = false;
        }
    }
}
