using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility to create an object pool and fill it with gameobjects
/// </summary>
public class ObjectPool : MonoBehaviour {

    public static ObjectPool sharedInstance;

    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public GameObject instantiatedObjects;
    public int amountToPool;

    private void Awake() {
        sharedInstance = this;
    }

    private void Start() {
        pooledObjects = new List<GameObject>();
        for( int i = 0; i < amountToPool; i++ ) {
            GameObject obj = (GameObject) Instantiate( objectToPool );
            obj.SetActive( false );
            pooledObjects.Add( obj );
            obj.transform.parent = instantiatedObjects.transform;
        }
    }

    public GameObject GetPooledObject() {
        for( int i = 0; i< pooledObjects.Count; i++ ) {
            if( !pooledObjects[i].activeInHierarchy ) {
                return pooledObjects[i];
            }
        }

        GameObject obj = (GameObject) Instantiate( objectToPool );
        obj.SetActive( false );
        pooledObjects.Add( obj );
        obj.transform.parent = instantiatedObjects.transform;
        return obj;
    }
}
