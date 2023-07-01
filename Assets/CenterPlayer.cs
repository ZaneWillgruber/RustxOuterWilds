using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPlayer : MonoBehaviour
{
    public Transform player;
    public List<GameObject> allObjects;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] tempObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        tempObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach(GameObject obj in tempObjects) {
            GameObject temp = FindParent(obj);
            if(!allObjects.Contains(temp)) {
                allObjects.Add(temp);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y < -1000 || player.transform.position.y > 1000) {
            CenterWorld();
        }
        if(player.transform.position.x < -1000 || player.transform.position.x > 1000) {
            CenterWorld();
        }
        if(player.transform.position.z < -1000 || player.transform.position.z > 1000) {
            CenterWorld();
        }
    }

    GameObject FindParent(GameObject go) {
        if(go.transform.parent) {
            return FindParent(go.transform.parent.gameObject);
        } else {
            return go;
        }
    }

    void CenterWorld() {
        Vector3 distanceToCenter = player.transform.position;
        foreach(GameObject obj in allObjects) {
            obj.transform.position -= distanceToCenter;
        }
    }
}
