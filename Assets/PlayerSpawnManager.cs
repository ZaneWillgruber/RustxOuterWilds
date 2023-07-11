using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{

    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindGameObjectsWithTag("Player").Length == 0)
        {
            SpawnOnPlanet();
            Debug.Log("Player spawned");
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnOnPlanet() {
        //pick a random planet for player to spawn on
        List<Planet> planets = UniverseSimulator.planets;
        int planetIndex = Random.Range(0, planets.Count);
        Planet planet = planets[planetIndex];

        // cast ray from center of planet to random point on surface
        Vector3 randomPointOnSurface = Random.onUnitSphere * planet.radius;
        Vector3 randomPointOnSurfaceWorldSpace = planet.transform.TransformPoint(randomPointOnSurface);
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = randomPointOnSurfaceWorldSpace + randomPointOnSurface.normalized * 2f;
    }
}
