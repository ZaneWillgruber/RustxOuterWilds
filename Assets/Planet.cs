using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    public SphereCollider gravityCollider;
    public float gravity = 0f;
    public float mass = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //calculate gravity based off of mass
        gravity = mass * UniverseSimulator.GRAVITATIONAL_CONSTANT;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
