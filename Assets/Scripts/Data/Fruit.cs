using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all logic for the fruit prefab.
/// </summary>
public class Fruit : MonoBehaviour {

    public float rendererShowDelay = 0.25f;
    public GameObject fruitRenderer, indicatorRenderer;

    private FruitSpawner fruitSpawner;
    private bool ignoreSnakeTailCollision = false;
    private const string gameManagerKey = "Game Manager";
    private const string snakeTailTag = "Snake Tail";

    private void Awake() {
        fruitSpawner = GameObject.Find( gameManagerKey ).GetComponentInChildren<FruitSpawner>();
    }

    private void OnTriggerStay( Collider other ) {
        if( other.gameObject.tag.Equals( snakeTailTag ) && !ignoreSnakeTailCollision ) {
            fruitSpawner.SpawnNewFruit(true);
        }
    }

    /// <summary>
    /// FruitSpawner has put the fruit to a new place so do all respawn logic (if it wasn't a position correction).
    /// </summary>
    public void Respawn( bool correction ) {
        if( !correction ) {
            StartCoroutine( WaitForShowDelay() );
        }
    }

    /// <summary>
    /// Coroutine which waits for a few milliseconds before showing the newly spawned fruit.
    /// </summary>
    private IEnumerator WaitForShowDelay() {
        fruitRenderer.SetActive( false );
        indicatorRenderer.SetActive( false );

        yield return new WaitForSeconds( rendererShowDelay );

        fruitRenderer.SetActive( true );
        indicatorRenderer.SetActive( true );
    }
    
    /// <summary>
    /// Check if fruit should ignore collision with snake tail objects because magnet powerup is active. Otherwise it might
    /// get stuck while moving towards the player.
    /// </summary>
    public void SetIgnoreSnakeTailCollision( bool value ) {
        ignoreSnakeTailCollision = value;
    } 
}
