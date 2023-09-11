using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    public List<buildObjects> objects = new List<buildObjects>();
    public buildObjects currentObject;
    private Vector3 currentPos = Vector3.zero;
    private Quaternion currentRot = Quaternion.identity;
    public Transform currentPreview;
    public Transform cam;
    public RaycastHit hit;
    public LayerMask layerMask;
    public bool isBuilding;

    GravityController gravHandler;
    GameObject refPlanet;

    void Start()
    {
        currentObject = objects[0];
        ChangeCurrentBuilding();
        gravHandler = GetComponent<GravityController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuilding = !isBuilding;
        }

        if (isBuilding)
        {
            refPlanet = gravHandler.referencePlanet;
            StartPreview();
            if (Input.GetMouseButtonDown(0))
            {
                Build();
            }
        }
    }

    public void ChangeCurrentBuilding()
    {
        GameObject curprev = Instantiate(currentObject.preview, currentPos, currentRot) as GameObject;
        currentPreview = curprev.transform;
    }

    public void StartPreview()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, 10, layerMask))
        {
            if (hit.transform != this.transform)
            {
                currentPreview.GetComponent<PreviewObject>().RaycastSuccess = true;
                Transform closest = ClosestBuilding(hit.point, 10);
                if(closest != null)
                {
                    ShowSnapPreview(closest, hit);
                }
                else
                {
                    ShowPreview(hit);
                }
            }
        }
        else
        {
            currentPreview.GetComponent<PreviewObject>().RaycastSuccess = false;
            ShowPreviewFail(cam.position + (cam.forward * 10));
        }

    }

    public void ShowSnapPreview(Transform closest, RaycastHit hit2)
    {
        Vector3 norm = (hit2.point - closest.position).normalized;
        //take to normalized vector and round it to the nearest local axis of closest building
        if (Mathf.Abs(norm.x) > Mathf.Abs(norm.y) && Mathf.Abs(norm.x) > Mathf.Abs(norm.z))
        {
            norm = new Vector3(Mathf.Round(norm.x), 0, 0);
        }
        else if (Mathf.Abs(norm.y) > Mathf.Abs(norm.x) && Mathf.Abs(norm.y) > Mathf.Abs(norm.z))
        {
            norm = new Vector3(0, Mathf.Round(norm.y), 0);
        }
        else if (Mathf.Abs(norm.z) > Mathf.Abs(norm.x) && Mathf.Abs(norm.z) > Mathf.Abs(norm.y))
        {
            norm = new Vector3(0, 0, Mathf.Round(norm.z));
        }
        
        currentPos = closest.position + (norm * 2);
        currentPreview.position = currentPos;
    }
    
    public void ShowPreview(RaycastHit hit2)
    {
        currentPos = hit2.point;
        Vector3 dir = (currentPreview.position - refPlanet.transform.position).normalized;
        currentRot = Quaternion.FromToRotation(currentPreview.up, dir) * currentPreview.rotation;
        currentPreview.rotation = currentRot;
        currentPreview.position = currentPos;
    }

    public void ShowPreviewFail(Vector3 hit2)
    {
        currentPos = hit2;
        Vector3 dir = (currentPreview.position - refPlanet.transform.position).normalized;
        currentRot = Quaternion.FromToRotation(currentPreview.up, dir) * currentPreview.rotation;
        currentPreview.rotation = currentRot;
        currentPreview.position = currentPos;
    }

    public void Build()
    {
        PreviewObject PO = currentPreview.GetComponent<PreviewObject>();
        if (PO.isBuildable)
        {
            GameObject obj = Instantiate(currentObject.prefab, currentPos, currentRot);
            obj.transform.parent = refPlanet.transform;
        }
    }

    public Transform ClosestBuilding(Vector3 pos, float radius)
    {
        //find closest building in radius
        Collider[] hitColliders = Physics.OverlapSphere(pos, radius);
        Transform closest = null;
        float closestDist = Mathf.Infinity;
        foreach (Collider c in hitColliders)
        {
            if (c.transform != this.transform && c.transform.tag == "Building")
            {
                float dist = Vector3.Distance(pos, c.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = c.transform;
                }
            }
        }
        return closest;
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

