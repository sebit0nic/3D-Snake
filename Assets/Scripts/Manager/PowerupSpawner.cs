using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Does all logic for powerup spawning and powerup handling.
/// </summary>
public class PowerupSpawner : MonoBehaviour {

    public GameObject powerupPrefab;
    public int minimumCollectedFruitToSpawn;
    public float minSpawnDelay, maxSpawnDelay, unspawnDelay;
    
    private GameObject powerupGameObject;
    private Powerup powerup;
    private bool canSpawn = false;
    private List<PowerupObject> unlockedPowerups;

    private const string waitForSpawnDelayMethod = "WaitForSpawnDelay";
    private const string waitForUnspawnDelayMethod = "WaitForUnspawnDelay";

    private WaitForSeconds unspawnWaitForSeconds;

    public void Init( SavedData savedData ) {
        powerupGameObject = Instantiate( powerupPrefab, Vector3.zero, Quaternion.identity );
        powerup = powerupGameObject.GetComponent<Powerup>();
        powerupGameObject.SetActive( false );
        unlockedPowerups = new List<PowerupObject>();
        unspawnWaitForSeconds = new WaitForSeconds( unspawnDelay );

        int unlockedPowerupCount = 0;
        foreach( PowerupObject powerupObject in savedData.GetUnlockedPowerups() ) {
            if( powerupObject.GetCurrentLevel() > 0 ) {
                unlockedPowerups.Add( powerupObject );
                unlockedPowerupCount++;
            }
        }

        if( unlockedPowerupCount > 0 ) {
            canSpawn = true;
        }
    }

    /// <summary>
    /// Update amount of collected fruit and check if powerups can be spawned.
    /// </summary>
    public void UpdateActualCollectedFruit( int actualCollectedFruit ) {
        if( actualCollectedFruit == minimumCollectedFruitToSpawn && canSpawn ) {
            ResumeSpawning();
        }
    }

    /// <summary>
    /// Resume the spawning of powerups after the current powerup has worn off.
    /// </summary>
    public void ResumeSpawning() {
        if( canSpawn ) {
            StartCoroutine(waitForSpawnDelayMethod);
        }
    }

    /// <summary>
    /// Player collected powerup so check which kind it was.
    /// </summary>
    public PlayerPowerupTypes CollectPowerup( SoundManager soundManager ) {
        PlayerPowerupTypes currentType = powerup.GetCurrentType();
        switch( currentType ) {
            case PlayerPowerupTypes.INVINCIBILTY:
                soundManager.PlaySound( SoundEffectType.SOUND_INVINCIBILITY, false );
                break;
            case PlayerPowerupTypes.MAGNET:
                soundManager.PlaySound( SoundEffectType.SOUND_MAGNET, false );
                break;
            case PlayerPowerupTypes.THIN:
                soundManager.PlaySound( SoundEffectType.SOUND_THIN, false );
                break;
        }

        powerupGameObject.SetActive( false );
        StopAllCoroutines();
        return currentType;
    }

    /// <summary>
    /// Stop everything on game over.
    /// </summary>
    public void Stop() {
        StopAllCoroutines();
        powerupGameObject.SetActive( false );
    }

    public float GetPowerupDuration() {
        return powerup.GetDuration();
    }

    /// <summary>
    /// Coroutine to wait a certain amount of time before spawning another powerup.
    /// </summary>
    private IEnumerator WaitForSpawnDelay() {
        yield return new WaitForSeconds( Random.Range( minSpawnDelay, maxSpawnDelay ) );
        powerupGameObject.transform.position = GameManager.instance.GetLastTailTransform().position;
        powerupGameObject.transform.rotation = GameManager.instance.GetLastTailTransform().rotation;

        powerupGameObject.SetActive( true );
        powerup.Respawn( unlockedPowerups );
        StopAllCoroutines();
        StartCoroutine( waitForUnspawnDelayMethod );
    }

    /// <summary>
    /// Coroutine to wait a certain amount of time before unspawning the current powerup.
    /// </summary>
    private IEnumerator WaitForUnspawnDelay() {
        yield return unspawnWaitForSeconds;
        powerupGameObject.SetActive( false );
        StartCoroutine( waitForSpawnDelayMethod );
    }
}
