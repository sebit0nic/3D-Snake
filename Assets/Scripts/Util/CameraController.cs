﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the movement of the camera during gameplay and on menus
/// </summary>
public class CameraController : MonoBehaviour {

    public Transform target;
    public Planet planet;
    public float upDistance = 7.0f;
    public float upDistancePortrait = 25.0f;
    public float backDistance = 10.0f;
    public float trackingSpeed = 3.0f;
    public float rotationSpeed = 9.0f;
    public float secondaryRotationSpeed = 15.0f;
    public float secondaryTrackingSpeed = 10.0f;
    public float panSpeed = 3.0f;

    private Vector3 v3To;
    private Quaternion qTo;
    private CameraStatus cameraStatus;
    private bool stopped = false;
    private float currentUpDistance;

    private Quaternion targetRotation;
    private const float resumeDelay = 0.5f;

    private WaitForSeconds resumeWaitForSeconds;

    public void Init( CameraStatus cameraStatus, int screenOrientation ) {
        this.cameraStatus = cameraStatus;
        resumeWaitForSeconds = new WaitForSeconds( resumeDelay );

        if( screenOrientation == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
            currentUpDistance = upDistance;
        } else {
            currentUpDistance = upDistancePortrait;
        }
    }

    private void LateUpdate() {
        if( !stopped ) {
            if( cameraStatus == CameraStatus.CAMERA_NO_ROTATION ) {
                v3To = target.position - target.forward * backDistance + target.up * currentUpDistance;
                transform.position = Vector3.Lerp( transform.position, v3To, trackingSpeed * Time.deltaTime );
                qTo = Quaternion.LookRotation( target.position - transform.position, transform.up );
                transform.localRotation = Quaternion.Slerp( transform.rotation, qTo, rotationSpeed * Time.deltaTime );
            } else if( cameraStatus == CameraStatus.CAMERA_ROTATE ) {
                v3To = target.position - target.forward * backDistance + target.up * currentUpDistance;
                transform.position = Vector3.Lerp( transform.position, v3To, secondaryTrackingSpeed * Time.deltaTime );
                qTo = Quaternion.LookRotation( target.position - transform.position, transform.forward );
                transform.localRotation = Quaternion.Slerp( transform.rotation, qTo, secondaryRotationSpeed * Time.deltaTime );
            }
        } else {
            transform.rotation = Quaternion.Slerp( transform.rotation, targetRotation, Time.deltaTime * panSpeed );
        }
    }

    /// <summary>
    /// Stop everything on game over.
    /// </summary>
    public void Stop( bool stopForAd ) {
        targetRotation = Quaternion.LookRotation( Vector3.zero - transform.position, transform.up );
        stopped = true;

        if( !stopForAd ) {
            planet.SetRotating(true);
        }
    }

    /// <summary>
    /// Resume after player has watched ad to revive.
    /// </summary>
    public void Resume() {
        StartCoroutine( WaitForResumeDelay() );
    }

    /// <summary>
    /// Coroutine to wait a few seconds before resuming the camera.
    /// </summary>
    private IEnumerator WaitForResumeDelay() {
        yield return resumeWaitForSeconds;
        stopped = false;
    }
}
