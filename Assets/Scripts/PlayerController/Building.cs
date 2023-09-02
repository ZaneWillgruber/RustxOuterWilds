using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    public bool buildMode = false;

    public List<GameObject> buildings = new List<GameObject>();

    GameObject currentBuilding;
    GameObject previewBuilding;

    // Start is called before the first frame update
    void Start()
    {
        currentBuilding = buildings[0];
    }

    // Update is called once per frame
    void Update()
    {
        BuildModeSwitch();
        
        if(!buildMode) return;
        
        SetCurrentBuilding();
        BuildMode();
    }

    void BuildModeSwitch()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            if(buildMode)
            {
                buildMode = false; 
                return;
            }
            else
            {
                buildMode = true;
                return;
            }
        }
    }

    void SetCurrentBuilding(int index = 0)
    {
        if(buildings.Count == 0) return;
        if(index > buildings.Count - 1) return;

        currentBuilding = buildings[index];
        previewBuilding = currentBuilding.GetComponent<BuildObject>().previewObject;
    }

    void BuildMode()
    {
        if(currentBuilding == null) {
            Debug.LogWarning("No building selected");
            return;
        }

        GameObject preview = Instantiate(previewBuilding, transform.forward * 5, Quaternion.identity);

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Building");
        }

        Destroy(preview);
    }
}
