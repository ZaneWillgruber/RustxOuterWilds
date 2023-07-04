using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{

    public SphereCollider gravityCollider;
    public float radius;
    public float surfaceGravity;
    public float mass;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector3(0, 0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculatePlanetGravityForce(List<Planet> allPlanets) {
        foreach (Planet planet in allPlanets) {
            if (planet != this) {
                float sqrDst = (planet.transform.position - transform.position).sqrMagnitude;
                Vector3 forceDir = (planet.transform.position - transform.position).normalized;

                Vector3 acceleration = forceDir * Universe.gravitationalConstant * planet.mass / sqrDst;
                rb.AddForce(acceleration, ForceMode.Acceleration);
            }
        }
    }

    void OnValidate() {
        rb = GetComponent<Rigidbody>();
        CalculateMass();
    }

    void CalculateMass() {
        mass = surfaceGravity * radius * radius / Universe.gravitationalConstant;
        rb.mass = mass;
    }
}
