using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public static ObjectPool sharedInstance;

    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    private GameObject instantiatedObjects;

    private void Awake() {
        sharedInstance = this;
    }

    private void Start() {
        instantiatedObjects = GameObject.Find("Instantiated Objects");
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++) {
            GameObject obj = (GameObject) Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.parent = instantiatedObjects.transform;
        }
    }

    public GameObject GetPooledObject() {
        for (int i = 0; i< pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeInHierarchy) {
                return pooledObjects[i];
            }
        }

        GameObject obj = (GameObject) Instantiate(objectToPool);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        obj.transform.parent = instantiatedObjects.transform;
        return obj;
    }
}
