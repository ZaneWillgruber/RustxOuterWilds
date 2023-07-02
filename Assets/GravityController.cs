using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{

    public GameObject referencePlanet;
    CharacterController controller;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
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

        //rotate to land on planet
        Vector3 dir = (transform.position - referencePlanet.transform.position).normalized;
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, dir) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.1f);


        //apply gravity if in gravity collider
        SphereCollider gravityCollider = referencePlanet.GetComponent<Planet>().gravityCollider;
        if (gravityCollider.bounds.Contains(transform.position))
        {
            Vector3 gravityDirection = (transform.position - referencePlanet.transform.position).normalized;
            float gravity = ((referencePlanet.GetComponent<Planet>().mass * GetComponent<Rigidbody>().mass)) / Mathf.Pow(closestDistance, 2);
            Debug.Log(gravity);
            //GetComponent<Rigidbody>().AddForce(gravityDirection * 10, ForceMode.Acceleration);
            velocity += gravityDirection * -gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
