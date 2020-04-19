using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the logic behind the spawning of new fruit objects.
/// </summary>
public class FruitSpawner : MonoBehaviour {

    public GameObject fruitPrefab;
    public float moveTowardsPlayerSpeed;

    [Header("Normal Random Rotation")]
    public float minRandomRotation;
    public float maxRandomRotation;
    public float minRandomRotationClamp;
    public float maxRandomRotationClamp;

    [Header("Small Random Rotation")]
    public float minRandomRotationSmall;
    public float maxRandomRotationSmall;
    public float minRandomRotationSmallClamp;
    public float maxRandomRotationSmallClamp;

    [Header("Correction Random Rotation")]
    public float minRandomRotationCorrection;
    public float maxRandomRotationCorrection;

    [Header("Difficulty")]
    public int increaseDifficultyFrequency;
    public float increaseDifficultyRate;

    private GameObject fruitGameobject;
    private Fruit fruit;
    private int collectedFruit;
    private bool moveFruitTowardsPlayer = false;
    private bool stopped = false, initiated = false;
    private int randomDirection;
    private float randomRotation;
    private float randomRotationSmall;

    private void Awake() {
        Vector3 fruitRotation = new Vector3();
        fruitRotation.Set( Random.Range( minRandomRotation, maxRandomRotation ), Random.Range( minRandomRotation, maxRandomRotation ), Random.Range( minRandomRotation, maxRandomRotation ) );
        fruitGameobject = Instantiate( fruitPrefab, Vector3.zero, Quaternion.Euler( fruitRotation ) );
        fruit = fruitGameobject.GetComponent<Fruit>();
        fruitGameobject.SetActive( false );
    }

    public void Init() {
        fruitGameobject.SetActive( true );
        initiated = true;
    }

    private void Update() {
        if( initiated ) {
            if( !stopped ) {
                if( moveFruitTowardsPlayer ) {
                    fruitGameobject.transform.rotation = Quaternion.Lerp( fruitGameobject.transform.rotation, GameManager.instance.GetCurrentSnakePosition().rotation, Time.deltaTime * moveTowardsPlayerSpeed );
                }
            }
        }
    }

    /// <summary>
    /// Spawn new fruit at random position, set "correction" if the fruit spawned inside the player to correct the position a little bit.
    /// </summary>
    public void SpawnNewFruit( bool correction ) {
        if( !stopped ) {
            if( correction ) {
                fruitGameobject.transform.Rotate( Random.Range( minRandomRotationCorrection, maxRandomRotationCorrection ), 0, Random.Range( minRandomRotationCorrection, maxRandomRotationCorrection ) );
            } else {
                randomDirection = Random.Range( 0, 2 );
                randomRotation = Random.Range( minRandomRotation, maxRandomRotation );
                randomRotationSmall = Random.Range( minRandomRotationSmall, maxRandomRotationSmall );

                // Logic: either X or Z axis is randomly selected for a larger random rotation, the other axis for a smaller random rotation.
                switch( randomDirection ) {
                    case 0:
                        fruitGameobject.transform.Rotate( randomRotation, 0, randomRotationSmall );
                        break;
                    case 1:
                        fruitGameobject.transform.Rotate( randomRotationSmall, 0, randomRotation );
                        break;
                }

                collectedFruit++;

                // Logic: every "increaseDifficultyFrequency" the random rotation values are increased so that the fruit spawns further and further away from the player, making it harder.
                if( collectedFruit % increaseDifficultyFrequency == 0 ) {
                    minRandomRotation += increaseDifficultyRate;
                    maxRandomRotation += increaseDifficultyRate;
                    minRandomRotationSmall -= increaseDifficultyRate;
                    maxRandomRotationSmall += increaseDifficultyRate;

                    minRandomRotation = Mathf.Clamp( minRandomRotation, 0, minRandomRotationClamp );
                    maxRandomRotation = Mathf.Clamp( maxRandomRotation, 0, maxRandomRotationClamp );
                    minRandomRotationSmall = Mathf.Clamp( minRandomRotationSmall, minRandomRotationSmallClamp, 0 );
                    maxRandomRotationSmall = Mathf.Clamp( maxRandomRotationSmall, 0, maxRandomRotationSmallClamp );
                }
            }

            fruit.Respawn( correction );
        }
    }

    /// <summary>
    /// Stop everything on game over.
    /// </summary>
    public void Stop() {
        stopped = true;
        fruit.gameObject.SetActive( false );
    }

    /// <summary>
    /// Resume after player has watched ad to revive.
    /// </summary>
    public void Resume() {
        stopped = false;
        fruit.gameObject.SetActive( true );
        SpawnNewFruit( false );
    }

    public void SetMoveFruitTowardsPlayer( bool value ) {
        moveFruitTowardsPlayer = value;
        fruit.SetIgnoreSnakeTailCollision( value );
    }
}
