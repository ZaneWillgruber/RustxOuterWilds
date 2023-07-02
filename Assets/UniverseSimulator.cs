using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseSimulator : MonoBehaviour
{

    public List<GameObject> planets;
    public const float GRAVITATIONAL_CONSTANT = .001f;

    // Start is called before the first frame update
    void Start()
    {
        planets = new List<GameObject>();
        foreach (GameObject planet in GameObject.FindGameObjectsWithTag("Planet"))
        {
            planets.Add(planet);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
