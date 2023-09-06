using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    
    public bool foundation;
    public List<Collider> col = new List<Collider>();
    public Material valid;
    public Material invalid;
    public bool isBuildable;
    public bool RaycastSuccess = false;

    void Update() {
        ChangeColor();
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 7 && foundation) {
            col.Add(other);
            Debug.Log("Added");
        }
        
    }

    void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == 7 && foundation) {
            col.Remove(other);
        }
    }

    public void ChangeColor() {
        if(col.Count == 0 && RaycastSuccess) {
            isBuildable = true;
            GetComponent<Renderer>().material = valid;
        }
        else {
            isBuildable = false;
            GetComponent<Renderer>().material = invalid;
        }

    }

}
