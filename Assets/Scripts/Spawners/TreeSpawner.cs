using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TreeSpawner : MonoBehaviour
{
    public int numTrees;
    public GameObject treePrefab;

    public LayerMask surfaceLayerMask;
    public bool debugMode;
    Vector3 debugRaycastDirection;
    Vector3 debugRaycastOrigin;
    public Transform testCube;

    public bool run = false;
    // Start is called before the first frame update
    void Start()
    {
        // //get all planets
        // List<Planet> planets = UniverseSimulator.planets;
        // //get only planets with body type as planet
        // List<Planet> planetsWithTrees = new List<Planet>();
        // foreach (Planet planet in planets)
        // {
        //     if (planet.bodyType == Planet.BodyType.Planet)
        //     {
        //         planetsWithTrees.Add(planet);
        //     }
        // }

        // //spawn trees on each planet
        // foreach (Planet planet in planetsWithTrees)
        // {
        //     SpawnTrees(planet);
        //     // for (int i = 0; i < numTrees; i++)
        //     // {
        //     //     Debug.Log("Tree spawned");
        //     //     // cast ray from center of planet to random point on surface
        //     //     Vector3 randomPointOnSurface = Random.onUnitSphere * planet.radius;
        //     //     Vector3 randomPointOnSurfaceWorldSpace = planet.transform.TransformPoint(randomPointOnSurface);
        //     //     GameObject tree = Instantiate(treePrefab) as GameObject;
        //     //     tree.transform.position = randomPointOnSurfaceWorldSpace + randomPointOnSurface.normalized * 2f;
        //     //     tree.transform.parent = planet.transform;

        //     //     //rotate tree to be perpendicular to planet surface
        //     //     Vector3 dir = (tree.transform.position - planet.transform.position).normalized;
        //     //     Quaternion toRotation = Quaternion.FromToRotation(tree.transform.up, dir) * tree.transform.rotation;
        //     //     tree.transform.rotation = toRotation;

        //     // }
        // }
    }

    void OnValidate()
    {
        if (run)
        {
            SpawnTrees();
            run = false;
        }
    }

    void Update()
    {
        if (debugMode)
        {
            Debug.DrawRay(debugRaycastOrigin, debugRaycastDirection, Color.red);
            testCube.position = debugRaycastOrigin;
            Debug.Log("Origin" + debugRaycastOrigin);
            Debug.Log("Direction" + debugRaycastDirection);
        }
    }

    public void SpawnTrees()
    {
        StartCoroutine(SpawnTreesCoroutine());

    }

    IEnumerator SpawnTreesCoroutine()
    {
        Debug.Log("Spawning trees");
        yield return new WaitForSeconds(.5f);

        List<Planet> planets = UniverseSimulator.planets;
        //get only planets with body type as planet
        List<Planet> planetsWithTrees = new List<Planet>();
        foreach (Planet planet in planets)
        {
            if (planet.bodyType == Planet.BodyType.Planet)
            {
                planetsWithTrees.Add(planet);
            }
        }

        //spawn trees on each planet
        foreach (Planet planet in planetsWithTrees)
        {
            SpawnTrees(planet);
        }
        Debug.Log("Trees spawned");
    }

    void SpawnTrees(Planet planet)
    {
        for (int i = 0; i < numTrees; i++)
        {
            float latitude = Random.Range(0f, 360f);
            float longitude = Random.Range(0f, 360f);



            (Vector3 spawnPosition, bool canSpawn) = CalculateSpawnPosition(latitude, longitude, planet);
            if (!canSpawn)
            {
                continue;
            }

            GameObject tree = Instantiate(treePrefab);
            tree.transform.position = spawnPosition;
            Debug.Log(spawnPosition);
            tree.transform.parent = planet.transform;



            Vector3 dir = (tree.transform.position - planet.transform.position).normalized;
            Quaternion toRotation = Quaternion.FromToRotation(tree.transform.up, dir) * tree.transform.rotation;
            tree.transform.rotation = toRotation;
        }
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
        Debug.DrawRay(origin, -(origin - planetCenter.position), Color.red, 1000f);

        if (spawnPositionFound)
        {
            debugRaycastDirection = -((spawnPosition * 1.05f) - planetCenter.position);
            debugRaycastOrigin = spawnPosition * 1.05f;
        }


        return (spawnPosition, spawnPositionFound);
    }
}
