using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the movement of the snake according to the inputs given.
/// </summary>
public class SnakeMovement : MonoBehaviour {

    public float playerVelocity, playerTurnVelocity;
    public float playerHoverOffset = 0.5f;
    public Planet planet;
    public SnakeTail snakeTailPrefab;
    public AnimationCurve steeringCurve;
    public GameObject instantiatedObjects;

    private Snake snake;
    private Rigidbody thisRigidbody;
    private float steerMultiplier;
    private bool leftDown, rightDown;
    private bool stopped = false;
    private RaycastHit downHit;
    private Vector3 surfaceNorm;
    private float evaluatedInput;
    private const string horizontalAxisKey = "Horizontal";

    public void Init( Snake snake ) {
        this.snake = snake;
        thisRigidbody = GetComponent<Rigidbody>();
        surfaceNorm = Vector3.zero;
    }

    private void Update() {
        if( !stopped ) {
            float distance = Vector3.Distance( planet.transform.position, transform.position );

            if( Physics.Raycast( transform.position, -transform.up, out downHit, distance ) ) {
                surfaceNorm = downHit.normal;
            }

            transform.localRotation = Quaternion.FromToRotation( transform.up, surfaceNorm ) * thisRigidbody.rotation;
            transform.localPosition = surfaceNorm * ( ( planet.transform.localScale.x / 2 ) + playerHoverOffset );
            transform.Translate( transform.forward * Time.deltaTime * playerVelocity, Space.World );

            // Evaluate Keyboard Input
            // -----------------------
            if( Input.GetAxisRaw( horizontalAxisKey ) != 0 ) {
                if( Input.GetAxis( horizontalAxisKey ) < 0 ) {
                    evaluatedInput = steeringCurve.Evaluate( -Input.GetAxis( horizontalAxisKey ) );
                    transform.Rotate( 0, -evaluatedInput * Time.deltaTime * playerTurnVelocity, 0 );
                } else {
                    evaluatedInput = steeringCurve.Evaluate( Input.GetAxis( horizontalAxisKey ) );
                    transform.Rotate( 0, evaluatedInput * Time.deltaTime * playerTurnVelocity, 0 );
                }
            } else {
                transform.Rotate( Vector3.zero );
            }

            // Evaluate Touch Input
            // --------------------
            if( leftDown && rightDown ) {
                steerMultiplier = 0;
            } else if( rightDown ) {
                steerMultiplier += Time.deltaTime;
                steerMultiplier = Mathf.Clamp( steerMultiplier, 0, 1 );
            } else if( leftDown ) {
                steerMultiplier -= Time.deltaTime;
                steerMultiplier = Mathf.Clamp( steerMultiplier, -1, 0 );
            } else {
                steerMultiplier = 0;
            }

            if( steerMultiplier != 0 ) {
                if( steerMultiplier < 0 ) {
                    evaluatedInput = steeringCurve.Evaluate( -steerMultiplier );
                    transform.Rotate( 0, -evaluatedInput * Time.deltaTime * playerTurnVelocity, 0 );
                } else {
                    evaluatedInput = steeringCurve.Evaluate( steerMultiplier );
                    transform.Rotate( 0, evaluatedInput * Time.deltaTime * playerTurnVelocity, 0 );
                }
            } else {
                transform.Rotate( Vector3.zero );
            }
        }
    }

    /// <summary>
    /// Stop everything on game over.
    /// </summary>
    public void Stop() {
        stopped = true;
        transform.parent = planet.transform;
        instantiatedObjects.transform.parent = this.transform;

        leftDown = false;
        rightDown = false;
    }

    /// <summary>
    /// Resume after player has watched ad to revive.
    /// </summary>
    public void Resume() {
        stopped = false;
        transform.parent = null;
        instantiatedObjects.transform.parent = null;
    }

    /// <summary>
    /// Player has touched move right button.
    /// </summary>
    public void MoveRight() {
        rightDown = true;
    }

    /// <summary>
    /// Player has touched move left button.
    /// </summary>
    public void MoveLeft() {
        leftDown = true;
    }

    /// <summary>
    /// Player has stopped touching either left or right move button.
    /// </summary>
    public void MoveRelease( int direction ) {
        if( direction < 0 ) {
            leftDown = false;
        } else if( direction > 0 ) {
            rightDown = false;
        } 
    }

    public Transform GetCurrentPosition() {
        return gameObject.transform;
    }
}
