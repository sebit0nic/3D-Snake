using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages every interaction with the different snake components.
/// </summary>
public class Snake : MonoBehaviour {

    private SnakeMovement snakeMovement;
    private SnakeCollision snakeCollision;
    private SnakeTailSpawner snakeTailSpawner;
    private SnakeHatChooser snakeHatChooser;

    private const float waitToReviveDelay = 0.5f;
    private const float reviveInvincibilityDuration = 3f;

    [Header("Debug Settings")]
    public bool invincible = false;

    private void Start() {
        snakeMovement = GetComponent<SnakeMovement>();
        snakeCollision = GetComponent<SnakeCollision>();
        snakeTailSpawner = GetComponent<SnakeTailSpawner>();
        snakeHatChooser = GetComponentInChildren<SnakeHatChooser>();

        snakeMovement.Init( this );
        snakeCollision.Init( this );
        snakeTailSpawner.Init( this );
        snakeHatChooser.Init( GameManager.instance.GetSavedData() );
    }

    /// <summary>
    /// SnakeCollider has touched a fruit.
    /// </summary>
    public void NotifyFruitEaten() {
        GameManager.instance.PlayerCollectedFruit();
        snakeTailSpawner.IncreaseSnakeLength();
    }

    /// <summary>
    /// SnakeCollider has touched a tail.
    /// </summary>
    public void NotifyTailTouched() {
        if ( !invincible ) {
            GameManager.instance.PlayerTouchedTail();
        }
    }

    /// <summary>
    /// SnakeCollider has touched a powerup.
    /// </summary>
    public void NotifyPowerupCollected() {
        switch( GameManager.instance.PlayerCollectedPowerup() ) {
            case PlayerPowerupTypes.INVINCIBILTY:
                snakeTailSpawner.InvincibilityPowerupActive( GameManager.instance.GetPowerupDuration() );
                snakeCollision.InvincibilityPowerupActive( GameManager.instance.GetPowerupDuration() );
                break;
            case PlayerPowerupTypes.MAGNET:
                snakeCollision.MagnetPowerupActive( GameManager.instance.GetPowerupDuration() );
                break;
            case PlayerPowerupTypes.THIN:
                snakeTailSpawner.ThinPowerupActive( GameManager.instance.GetPowerupDuration() );
                break;
        }
    }

    /// <summary>
    /// SnakeCollider of type MAGNET has touched a fruit.
    /// </summary>
    public void NotifyFruitTouchedByMagnet() {
        GameManager.instance.PlayerMagnetTouchedFruit();
    }

    /// <summary>
    /// Powerup has worn off. If it hasn't worn off because of a game over, set resumeSpawning to true.
    /// </summary>
    public void NotifyPowerupWoreOff( bool resumeSpawning ) {
        GameManager.instance.PowerupWoreOff( resumeSpawning );
    }

    /// <summary>
    /// Stop everything on game over.
    /// </summary>
    public void Stop() {
        snakeMovement.Stop();
        snakeCollision.Stop();
        snakeTailSpawner.Stop();
    }

    /// <summary>
    /// Resume after player has watched ad to revive.
    /// </summary>
    public void Resume() {
        StartCoroutine( WaitAfterResume() );
    }

    public Transform GetCurrentPosition() {
        return snakeMovement.GetCurrentPosition();
    }

    public Transform GetLastTailTransform() {
        return snakeTailSpawner.GetLastTailTransform();
    }

    /// <summary>
    /// Wait a few seconds after game over to resume.
    /// </summary>
    private IEnumerator WaitAfterResume() {
        yield return new WaitForSeconds( waitToReviveDelay );
        snakeTailSpawner.InvincibilityPowerupActive( reviveInvincibilityDuration );
        snakeCollision.InvincibilityPowerupActive( reviveInvincibilityDuration );
        snakeMovement.Resume();
        snakeCollision.Resume();
        snakeTailSpawner.Resume();
    }
}
