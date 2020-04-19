using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTailSpawner : MonoBehaviour {
    
    public float tailRepeatFactor = 0.1f;
    public float startLength = 0.8f;
    public float lengthIncreaseFactor = 0.4f, thinSize = 0.5f;
    public Color gameOverColor;
    public Material normalMaterial, invincibiltyMaterial;
    public MeshRenderer snakeHeadLowerMeshRenderer, snakeHeadUpperRenderer;

    private Snake snake;
    private const float lifespan = 0.1f;
    private List<SnakeTail> snakeTailList;
    private bool thinPowerupEnabled = false, currentTailThin = false, lastTailThin = false;
    private Animator snakeThinAnimator;
    private bool invincible = false;
    private Color initialColor;
    private SnakeTail newSnakeTail;
    private Vector3 thinSizeVector;

    private const string popTailKey = "PopTail";
    private const string spawnColliderKey = "SpawnCollider";
    private const string thinOnTrigger = "ThinOn";
    private const string thinOffTrigger = "ThinOff";

    public void Init( Snake snake ) {
        this.snake = snake;
        snakeTailList = new List<SnakeTail>();
        snakeThinAnimator = GetComponent<Animator>();
        initialColor = snakeHeadLowerMeshRenderer.material.color;
        thinSizeVector = new Vector3( thinSize, thinSize, thinSize );

        InvokeRepeating( spawnColliderKey, tailRepeatFactor, tailRepeatFactor );
        InvokeRepeating( popTailKey, startLength, lifespan );
    }

    /// <summary>
    /// This method is repeated every few seconds and spawns a new snake tail from the object pool.
    /// </summary>
    private void SpawnCollider() {
        newSnakeTail = ObjectPool.sharedInstance.GetPooledObject();
        newSnakeTail.transform.position = transform.position;
        newSnakeTail.transform.rotation = transform.rotation;
        newSnakeTail.gameObject.SetActive( true );
        newSnakeTail.Init( thinPowerupEnabled, invincible );
        snakeTailList.Add( newSnakeTail );
    }

    /// <summary>
    /// This method is repeated every few seconds and removes the tail at the end and moves it back to the object pool.
    /// </summary>
    private void PopTail() {
        currentTailThin = snakeTailList[0].IsInThinMode();
        snakeTailList[0].gameObject.SetActive( false );
        snakeTailList.RemoveAt( 0 );

        //Check if there was a change between normal tail and thin tail so that the speed at which tails are popped can be adjusted.
        if( currentTailThin != lastTailThin ) {
            if( currentTailThin ) {
                CancelInvoke( popTailKey );
                InvokeRepeating( popTailKey , 0, lifespan / 2 );
            } else {
                CancelInvoke( popTailKey );
                InvokeRepeating( popTailKey , 0, lifespan );
            }
        }

        lastTailThin = currentTailThin;
    }

    /// <summary>
    /// Increase the length of the snake by pausing the PopTail method for a given amount.
    /// </summary>
    public void IncreaseSnakeLength() {
        CancelInvoke( popTailKey );
        if( currentTailThin ) {
            InvokeRepeating( popTailKey, lengthIncreaseFactor, lifespan / 2 );
        } else {
            InvokeRepeating( popTailKey, lengthIncreaseFactor, lifespan );
        }
    }

    /// <summary>
    /// Thin powerup is active, so start a coroutine to handle that.
    /// </summary>
    public void ThinPowerupActive( float duration ) {
        StartCoroutine( WaitForThinPowerupDuration( duration ) );
    }

    /// <summary>
    /// Invincibility powerup is active, so start a coroutine to handle that.
    /// </summary>
    public void InvincibilityPowerupActive( float duration ) {
        StartCoroutine( WaitForInvincibilityPowerupDuration( duration ) );
    }

    /// <summary>
    /// Stop everything on game over.
    /// </summary>
    public void Stop() {
        snakeThinAnimator.enabled = false;
        StartGameOverAnimation();
        CancelInvoke( spawnColliderKey );
        CancelInvoke( popTailKey );
        snakeThinAnimator.SetTrigger( thinOffTrigger );
        snake.NotifyPowerupWoreOff( false );
        thinPowerupEnabled = false;
        StopAllCoroutines();
    }

    /// <summary>
    /// Resume after player has watched ad to revive.
    /// </summary>
    public void Resume() {
        snakeThinAnimator.enabled = true;
        CancelInvoke( spawnColliderKey );
        CancelInvoke( popTailKey );
        InvokeRepeating( spawnColliderKey, 0, tailRepeatFactor );
        InvokeRepeating( popTailKey, 0, lifespan );
        transform.localScale = Vector3.one;

        snakeHeadLowerMeshRenderer.material.color = initialColor;
        snakeHeadUpperRenderer.material.color = initialColor;
        foreach( SnakeTail snakeTail in snakeTailList ) {
            snakeTail.StartGameOverAnimation( initialColor );
        }
    }

    public bool IsThinPowerupEnabled() {
        return thinPowerupEnabled;
    }

    /// <summary>
    /// Return the tail transform at the end of the snake.
    /// </summary>
    public Transform GetLastTailTransform() {
        return snakeTailList[0].transform;
    }

    /// <summary>
    /// Game over for player, so notify each snake tail to change color.
    /// </summary>
    private void StartGameOverAnimation() {
        snakeHeadLowerMeshRenderer.material.color = gameOverColor;
        snakeHeadUpperRenderer.material.color = gameOverColor;
        foreach( SnakeTail snakeTail in snakeTailList ) {
            snakeTail.StartGameOverAnimation( gameOverColor );
        }
    }

    /// <summary>
    /// Coroutine to do everything necessary before and after thin powerup is active.
    /// </summary>
    private IEnumerator WaitForThinPowerupDuration( float duration ) {
        thinPowerupEnabled = true;
        snakeThinAnimator.ResetTrigger( thinOffTrigger );
        snakeThinAnimator.SetTrigger( thinOnTrigger );
        transform.localScale = thinSizeVector;
        CancelInvoke( spawnColliderKey );
        InvokeRepeating( spawnColliderKey, 0, tailRepeatFactor / 2 );

        yield return new WaitForSeconds( duration );

        CancelInvoke( spawnColliderKey );
        InvokeRepeating( spawnColliderKey, 0, tailRepeatFactor );
        snakeThinAnimator.SetTrigger( thinOffTrigger );
        thinPowerupEnabled = false;
        transform.localScale = Vector3.one;
        snake.NotifyPowerupWoreOff( true );
    }

    /// <summary>
    /// Coroutine to do everything necessary before and after invincibility powerup is active.
    /// </summary>
    private IEnumerator WaitForInvincibilityPowerupDuration( float duration ) {
        invincible = true;
        snakeHeadUpperRenderer.material = invincibiltyMaterial;
        snakeHeadLowerMeshRenderer.material = invincibiltyMaterial;
        foreach( SnakeTail snakeTail in snakeTailList ) {
            snakeTail.StartInvincibilityMaterial();
        }

        yield return new WaitForSeconds( duration );

        foreach( SnakeTail snakeTail in snakeTailList ) {
            snakeTail.StopInvincibilityMaterial();
        }
        snakeHeadLowerMeshRenderer.material = normalMaterial;
        snakeHeadUpperRenderer.material = normalMaterial;
        invincible = false;
    }
}
