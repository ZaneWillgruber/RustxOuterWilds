using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UniverseSimulator : MonoBehaviour
{

    public List<Planet> planets;
    public const float GRAVITATIONAL_CONSTANT = .001f;

    // Start is called before the first frame update
    void Start()
    {
        planets = new List<Planet>();
        foreach (GameObject planet in GameObject.FindGameObjectsWithTag("Planet"))
        {
            planets.Add(planet.GetComponent<Planet>());
        }    
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Planet planet in planets)
        {
            planet.CalculatePlanetGravityForce(planets);
        }
    }
}
