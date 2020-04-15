using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles incoming request of the Shop Scene to show certain powerup previews.
/// </summary>
public class PowerupPreview : MonoBehaviour {

    public Renderer[] snakeTailRenderer;
    public GameObject magnetParticles;
    public Animator[] thinAnimators;
    public GameObject[] thinTails;
    public Material normalSnakeMaterial, invincibiltySnakeMaterial;

    private const string thinOffAnimationKey = "ThinOff";
    private const string thinOnAnimationKey = "ThinOn";

    /// <summary>
    /// Show powerup preview according to currently selected powerupType.
    /// </summary>
    public void Show( PlayerPowerupTypes powerupType ) {
        switch( powerupType ) {
            case PlayerPowerupTypes.INVINCIBILTY:
                foreach( Renderer renderer in snakeTailRenderer ) {
                    renderer.material = invincibiltySnakeMaterial;
                }
                break;
            case PlayerPowerupTypes.MAGNET:
                magnetParticles.SetActive( true );
                break;
            case PlayerPowerupTypes.THIN:
                foreach( Animator animator in thinAnimators ) {
                    animator.ResetTrigger( thinOffAnimationKey );
                    animator.SetTrigger( thinOnAnimationKey );
                }
                foreach ( GameObject go in thinTails ) {
                    go.SetActive( true );
                }
                break;
        }
    }

    /// <summary>
    /// Disable every preview after powerup section has been left.
    /// </summary>
    public void DisableAllPreviews() {
        foreach( Renderer renderer in snakeTailRenderer ) {
            renderer.material = normalSnakeMaterial;
        }
        magnetParticles.SetActive( false );

        foreach ( Animator animator in thinAnimators ) {
            animator.ResetTrigger( thinOnAnimationKey );
            animator.SetTrigger( thinOffAnimationKey );
        }
        foreach ( GameObject go in thinTails ) {
            go.SetActive( false );
        }
    }
}
