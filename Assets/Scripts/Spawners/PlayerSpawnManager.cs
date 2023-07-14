using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{

    public GameObject playerPrefab;
    public LayerMask surfaceLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
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

    public void SpawnOnPlanet()
    {
        //pick a random planet for player to spawn on
        List<Planet> planets = UniverseSimulator.planets;
        int planetIndex = Random.Range(0, planets.Count);
        Planet planet = planets[planetIndex];

        // cast ray from center of planet to random point on surface
        float latitude = Random.Range(0f, 360f);
        float longitude = Random.Range(0f, 360f);
        bool found = false;
        Vector3 pos = new Vector3(0, 0, 0);

        GameObject player = Instantiate(playerPrefab);

        (pos, found) = CalculateSpawnPosition(longitude, latitude, planet);
        

        player.transform.position =  pos;
    }

    (Vector3, bool) CalculateSpawnPosition(float latitude, float longitude, Planet planet)
    {
        bool spawnPositionFound = false;
        float planetRadius = planet.radius;
        Transform planetCenter = planet.transform;

        float latitudeRad = latitude * Mathf.Deg2Rad;
        float longitudeRad = longitude * Mathf.Deg2Rad;

        float x = planetRadius * Mathf.Sin(latitudeRad) * Mathf.Cos(longitudeRad);
        float y = planetRadius * Mathf.Cos(latitudeRad);
        float z = planetRadius * Mathf.Sin(latitudeRad) * Mathf.Sin(longitudeRad);

        Vector3 spawnPosition = planetCenter.position + new Vector3(x, y, z);
        Vector3 origin = ((spawnPosition - planetCenter.position) * 1.1f) + planetCenter.position;

        // Raycast from the planet's surface to find the correct height on the surface
        RaycastHit hit;
        if (Physics.Raycast(origin, -(origin - planetCenter.position), out hit, planetRadius * 2f, surfaceLayerMask))
        {

            Debug.Log("Tree spawn position found");
            Debug.Log(spawnPosition);

            //make sure tree is not spawned inside planet


            spawnPosition = hit.point;
            spawnPositionFound = true;
        }
        else
        {
            // Handle case where raycast doesn't hit the surface
            Debug.LogWarning("Tree spawn position not found. Adjust planetRadius or check surfaceLayerMask.");

        }

        return (spawnPosition, spawnPositionFound);
    }
}
