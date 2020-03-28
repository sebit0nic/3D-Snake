using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollision : MonoBehaviour {

    public float powerupCourtesyDelay;

    private SnakeCollider magnetCollider;
    private ParticleSystem fruitCollectParticle, magnetPowerupParticle;
    private Animator snakeHeadAnimator;
    private Snake snake;
    private bool invincible = false;
    private bool stopped = false;

    public void Init(Snake snake) {
        this.snake = snake;
        fruitCollectParticle = transform.Find("Fruit Collect Particle").GetComponent<ParticleSystem>();
        magnetPowerupParticle = transform.Find("Magnet Powerup Particle").GetComponent<ParticleSystem>();
        snakeHeadAnimator = transform.Find("Head").GetComponent<Animator>();

        SnakeCollider[] colliderList = GetComponentsInChildren<SnakeCollider>();
        foreach(SnakeCollider col in colliderList) {
            if (col.GetColliderType() == SnakeColliderType.MAGNET) {
                magnetCollider = col;
                break;
            }
        }
    }

    public void InvincibilityPowerupActive(float duration) {
        StartCoroutine(WaitForInvincibilityPowerupDuration(duration));
    }

    public void MagnetPowerupActive(float duration) {
        StartCoroutine(WaitForMagnetPowerupDuration(duration));
    }

    public void Collide(Collider other, SnakeColliderType snakeColliderType) {
        if (!stopped) {
            switch ( snakeColliderType ) {
                case SnakeColliderType.COLLECTIBLE:
                    if ( other.gameObject.tag.Equals("Fruit") ) {
                        snake.NotifyFruitEaten();
                        fruitCollectParticle.Stop();
                        fruitCollectParticle.Play();
                        snakeHeadAnimator.SetTrigger("OnEat");
                    }
                    if ( other.gameObject.tag.Equals("Powerup") ) {
                        snake.NotifyPowerupCollected();
                    }
                    break;
                case SnakeColliderType.TAIL:
                    if ( other.gameObject.tag.Equals("Snake Tail") && !invincible ) {
                        snake.NotifyTailTouched();
                    }
                    break;
                case SnakeColliderType.MAGNET:
                    if ( other.gameObject.tag.Equals("Fruit") ) {
                        snake.NotifyFruitTouchedByMagnet();
                    }
                    break;
            }
        }
    }

    public void Stop() {
        stopped = true;
        invincible = false;
        magnetPowerupParticle.gameObject.SetActive(false);
        magnetCollider.OnMagnetPowerupEnd();
        snake.NotifyPowerupWoreOff(false);
        StopAllCoroutines();
    }

    public void Resume() {
        stopped = false;
    }

    private IEnumerator WaitForInvincibilityPowerupDuration(float duration) {
        invincible = true;
        yield return new WaitForSeconds(duration);
        snake.NotifyPowerupWoreOff(true);
        StartCoroutine(WaitForCourtesyPowerupDuration());
    }

    private IEnumerator WaitForCourtesyPowerupDuration() {
        yield return new WaitForSeconds(powerupCourtesyDelay);
        invincible = false;
    }

    private IEnumerator WaitForMagnetPowerupDuration(float duration) {
        magnetCollider.OnMagnetPowerupStart();
        magnetPowerupParticle.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        magnetPowerupParticle.gameObject.SetActive(false);
        magnetCollider.OnMagnetPowerupEnd();
        snake.NotifyPowerupWoreOff(true);
    }
}
