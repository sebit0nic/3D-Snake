using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all incoming collisions of the snake object.
/// </summary>
public class SnakeCollision : MonoBehaviour {

    public float powerupCourtesyDelay;
    public ParticleSystem fruitCollectParticle, magnetPowerupParticle;
    public Animator snakeHeadAnimator;

    private SnakeCollider magnetCollider;
    private Snake snake;
    private bool invincible = false;
    private bool stopped = false;
    private const string fruitTag = "Fruit";
    private const string powerupTag = "Powerup";
    private const string snakeTailTag = "Snake Tail";
    private const string onEatTrigger = "OnEat";

    public void Init( Snake snake ) {
        this.snake = snake;

        SnakeCollider[] colliderList = GetComponentsInChildren<SnakeCollider>();
        foreach( SnakeCollider col in colliderList ) {
            if( col.GetColliderType() == SnakeColliderType.MAGNET ) {
                magnetCollider = col;
                break;
            }
        }
    }

    /// <summary>
    /// Player has collected an INVINCIBILITY powerup.
    /// </summary>
    public void InvincibilityPowerupActive( float duration ) {
        StartCoroutine( WaitForInvincibilityPowerupDuration( duration ) );
    }

    /// <summary>
    /// Player has collected a MAGNET powerup.
    /// </summary>
    public void MagnetPowerupActive( float duration ) {
        StartCoroutine( WaitForMagnetPowerupDuration( duration ) );
    }

    /// <summary>
    /// One of the snake collider has collided with something. Do something about it.
    /// </summary>
    public void Collide( Collider other, SnakeColliderType snakeColliderType ) {
        if( !stopped ) {
            switch( snakeColliderType ) {
                case SnakeColliderType.COLLECTIBLE:
                    if( other.gameObject.tag.Equals( fruitTag ) ) {
                        snake.NotifyFruitEaten();
                        fruitCollectParticle.Stop();
                        fruitCollectParticle.Play();
                        snakeHeadAnimator.SetTrigger( onEatTrigger );
                    }
                    if( other.gameObject.tag.Equals( powerupTag ) ) {
                        snake.NotifyPowerupCollected();
                    }
                    break;
                case SnakeColliderType.TAIL:
                    if( other.gameObject.tag.Equals( snakeTailTag ) && !invincible ) {
                        snake.NotifyTailTouched();
                    }
                    break;
                case SnakeColliderType.MAGNET:
                    if( other.gameObject.tag.Equals( fruitTag ) ) {
                        snake.NotifyFruitTouchedByMagnet();
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Stop everything on game over.
    /// </summary>
    public void Stop() {
        stopped = true;
        invincible = false;
        magnetPowerupParticle.gameObject.SetActive( false );
        magnetCollider.OnMagnetPowerupEnd();
        snake.NotifyPowerupWoreOff( false );
        StopAllCoroutines();
    }

    /// <summary>
    /// Resume after player has watched ad to revive.
    /// </summary>
    public void Resume() {
        stopped = false;
    }

    /// <summary>
    /// INVINCIBILITY powerup is active for the given duration.
    /// </summary>
    private IEnumerator WaitForInvincibilityPowerupDuration( float duration ) {
        invincible = true;
        yield return new WaitForSeconds( duration );
        snake.NotifyPowerupWoreOff( true );
        StartCoroutine( WaitForCourtesyPowerupDuration() );
    }

    /// <summary>
    /// Keep the invincibility going even after it has ended just a few milliseconds
    /// so it doesn't seem unfair if the player touches the tail the last second.
    /// </summary>
    private IEnumerator WaitForCourtesyPowerupDuration() {
        yield return new WaitForSeconds( powerupCourtesyDelay );
        invincible = false;
    }

    /// <summary>
    /// MAGNET powerup is active for the given duration.
    /// </summary>
    private IEnumerator WaitForMagnetPowerupDuration( float duration ) {
        magnetCollider.OnMagnetPowerupStart();
        magnetPowerupParticle.gameObject.SetActive( true );

        yield return new WaitForSeconds( duration );

        magnetPowerupParticle.gameObject.SetActive( false );
        magnetCollider.OnMagnetPowerupEnd();
        snake.NotifyPowerupWoreOff( true );
    }
}
