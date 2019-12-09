using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    public Transform planet;
    public Transform gameOverCameraPosition1, gameOverCameraPosition2;
    public float upDistance = 7.0f;
    public float backDistance = 10.0f;
    public float trackingSpeed = 3.0f;
    public float rotationSpeed = 9.0f;

    private Vector3 v3To;
    private Quaternion qTo;
    private bool stopped = false, animationStarted = false;

    private void Update() {
        if (!stopped) {
            v3To = target.position - target.forward * backDistance + target.up * upDistance;
            transform.position = Vector3.Lerp(transform.position, v3To, trackingSpeed * Time.deltaTime);
            qTo = Quaternion.LookRotation(target.position - transform.position, transform.up);
            transform.localRotation = Quaternion.Slerp(transform.rotation, qTo, rotationSpeed * Time.deltaTime);
        }
        
        if (animationStarted) {
            if (target.position.z <= 0) {
                transform.position = Vector3.Lerp(transform.position, gameOverCameraPosition1.position, Time.deltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, gameOverCameraPosition1.rotation, Time.deltaTime * 5);
            } else {
                transform.position = Vector3.Lerp(transform.position, gameOverCameraPosition2.position, Time.deltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, gameOverCameraPosition2.rotation, Time.deltaTime * 5);
            }
        }
    }

    public void Stop() {
        stopped = true;
    }

    public void OnGameOverAnimation() {
        animationStarted = true;
    }
}
