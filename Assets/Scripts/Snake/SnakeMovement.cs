using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour {

    public float playerVelocity, playerTurnVelocity;
    public float playerHoverOffset = 0.5f;
    public Planet planet;
    public SnakeTail snakeTailPrefab;
    public AnimationCurve steeringCurve;

    private Snake snake;
    private Rigidbody thisRigidbody;
    private float steerMultiplier;
    private bool leftDown, rightDown;
    private bool stopped = false;
    private GameObject instantiatedObjects;

    public void Init(Snake snake) {
        this.snake = snake;
        thisRigidbody = GetComponent<Rigidbody>();
        instantiatedObjects = GameObject.Find("Instantiated Objects");
    }

    private void FixedUpdate() {
        if (!stopped) {
            float distance = Vector3.Distance(planet.transform.position, transform.position);
            Vector3 surfaceNorm = Vector3.zero;

            RaycastHit hit;
            if ( Physics.Raycast(transform.position, -transform.up, out hit, distance) ) {
                surfaceNorm = hit.normal;
            }

            transform.localRotation = Quaternion.FromToRotation(transform.up, surfaceNorm) * thisRigidbody.rotation;
            transform.localPosition = surfaceNorm * ( ( planet.transform.localScale.x / 2 ) + playerHoverOffset );
            thisRigidbody.velocity = thisRigidbody.transform.forward * playerVelocity;

            //Keyboard Input
            //--------------
            float evaluatedInput = 0;
            if ( Input.GetAxisRaw("Horizontal") != 0 ) {
                if ( Input.GetAxis("Horizontal") < 0 ) {
                    evaluatedInput = steeringCurve.Evaluate(-Input.GetAxis("Horizontal"));
                    transform.Rotate(0, -evaluatedInput * Time.deltaTime * playerTurnVelocity, 0);
                } else {
                    evaluatedInput = steeringCurve.Evaluate(Input.GetAxis("Horizontal"));
                    transform.Rotate(0, evaluatedInput * Time.deltaTime * playerTurnVelocity, 0);
                }
            } else {
                transform.Rotate(Vector3.zero);
            }

            //Touch Input
            //-----------
            if ( steerMultiplier != 0 ) {
                if ( steerMultiplier < 0 ) {
                    evaluatedInput = steeringCurve.Evaluate(-steerMultiplier);
                    transform.Rotate(0, -evaluatedInput * Time.deltaTime * playerTurnVelocity, 0);
                } else {
                    evaluatedInput = steeringCurve.Evaluate(steerMultiplier);
                    transform.Rotate(0, evaluatedInput * Time.deltaTime * playerTurnVelocity, 0);
                }
            } else {
                transform.Rotate(Vector3.zero);
            }
        }
    }

    private void Update() {
        if (leftDown && rightDown) {
            steerMultiplier = 0;
        } if (rightDown) {
            steerMultiplier += Time.deltaTime;
            steerMultiplier = Mathf.Clamp(steerMultiplier, 0, 1);
        } else if (leftDown) {
            steerMultiplier -= Time.deltaTime;
            steerMultiplier = Mathf.Clamp(steerMultiplier, -1, 0);
        } else {
            steerMultiplier = 0;
        }
    }

    public void Stop() {
        stopped = true;
        thisRigidbody.velocity = Vector3.zero;
        transform.parent = planet.transform;
        instantiatedObjects.transform.parent = planet.transform;
        planet.SetRotating(true);
    }

    public void Resume() {
        stopped = false;
        thisRigidbody.velocity = thisRigidbody.transform.forward * playerVelocity;
        transform.parent = null;
        instantiatedObjects.transform.parent = null;
        planet.SetRotating(false);
    }

    public void MoveRight() {
        rightDown = true;
    }

    public void MoveLeft() {
        leftDown = true;
    }

    public void MoveRelease(int direction) {
        if (direction < 0) {
            leftDown = false;
        } else if (direction > 0) {
            rightDown = false;
        } 
    }

    public Transform GetCurrentPosition() {
        return gameObject.transform;
    }
}
