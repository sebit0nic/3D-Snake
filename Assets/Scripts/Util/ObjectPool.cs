using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility to create an object pool and fill it with gameobjects
/// </summary>
public class ObjectPool : MonoBehaviour {

    public static ObjectPool sharedInstance;

    public List<SnakeTail> pooledObjects;
    public SnakeTail objectToPool;
    public GameObject instantiatedObjects;
    public int amountToPool;

    private void Awake() {
        sharedInstance = this;
    }

    private void Start() {
        pooledObjects = new List<SnakeTail>();
        for( int i = 0; i < amountToPool; i++ ) {
            SnakeTail snakeTail = Instantiate( objectToPool );
            snakeTail.gameObject.SetActive( false );
            pooledObjects.Add( snakeTail );
            snakeTail.transform.parent = instantiatedObjects.transform;
        }
    }

    public SnakeTail GetPooledObject() {
        for( int i = 0; i< pooledObjects.Count; i++ ) {
            if( !pooledObjects[i].gameObject.activeInHierarchy ) {
                return pooledObjects[i];
            }
        }

        SnakeTail snakeTail = Instantiate( objectToPool );
        snakeTail.gameObject.SetActive( false );
        pooledObjects.Add( snakeTail );
        snakeTail.transform.parent = instantiatedObjects.transform;
        return snakeTail;
    }
}
