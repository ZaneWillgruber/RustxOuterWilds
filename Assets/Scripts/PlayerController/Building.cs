using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    public List<buildObjects> objects = new List<buildObjects>();
    public buildObjects currentObject;
    private Vector3 currentPos;
    public Transform currentPreview;
    public Transform cam;
    public RaycastHit hit;
    public LayerMask layerMask;
    public bool isBuilding;

    void Start()
    {
        currentObject = objects[0];
        ChangeCurrentBuilding();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuilding = !isBuilding;
        }

        if (isBuilding)
        {
            StartPreview();
            if(Input.GetMouseButtonDown(0)) {
                Build();
            }
        }
    }

    public void ChangeCurrentBuilding()
    {
        GameObject curprev = Instantiate(currentObject.preview, currentPos, Quaternion.identity) as GameObject;
        currentPreview = curprev.transform;
    }

    public void StartPreview()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, 10, layerMask))
        {
            if (hit.transform != this.transform)
            {
                ShowPreview(hit);
            }
        }
        else
        {
            ShowPreviewFail(cam.position + (cam.forward * 10));
        }

    }

    public void ShowPreview(RaycastHit hit2)
    {
        currentPos = hit2.point;
        currentPreview.position = currentPos;
    }

    public void ShowPreviewFail(Vector3 hit2)
    {
        currentPos = hit2;
        currentPreview.position = currentPos;
    }

    public void Build() {
        PreviewObject PO = currentPreview.GetComponent<PreviewObject>();
        if(PO.isBuildable) {
            Instantiate(currentObject.prefab, currentPos, Quaternion.identity);
        }
    }


}

[System.Serializable]
public class buildObjects
{
    public string name;
    public GameObject prefab;
    public GameObject preview;
    public Item material;
    public int cost;
}

