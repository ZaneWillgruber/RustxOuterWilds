using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GravityObject : MonoBehaviour
{

    public GameObject referencePlanet;
    Rigidbody rb;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
}
