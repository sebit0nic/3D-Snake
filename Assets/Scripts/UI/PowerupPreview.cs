using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPreview : MonoBehaviour {

    public Renderer[] snakeTailRenderer;
    public GameObject magnetParticles;
    public Animator[] thinAnimators;
    public GameObject[] thinTails;
    public Material normalSnakeMaterial, invincibiltySnakeMaterial;

    public void Show(PlayerPowerupTypes powerupType) {
        switch(powerupType) {
            case PlayerPowerupTypes.INVINCIBILTY:
                foreach(Renderer renderer in snakeTailRenderer) {
                    renderer.material = invincibiltySnakeMaterial;
                }
                break;
            case PlayerPowerupTypes.MAGNET:
                magnetParticles.SetActive(true);
                break;
            case PlayerPowerupTypes.THIN:
                foreach(Animator animator in thinAnimators) {
                    animator.ResetTrigger("ThinOff");
                    animator.SetTrigger("ThinOn");
                }
                foreach ( GameObject go in thinTails ) {
                    go.SetActive(true);
                }
                break;
        }
    }

    public void DisableAllPreviews() {
        foreach (Renderer renderer in snakeTailRenderer) {
            renderer.material = normalSnakeMaterial;
        }
        magnetParticles.SetActive(false);

        foreach ( Animator animator in thinAnimators ) {
            animator.ResetTrigger("ThinOn");
            animator.SetTrigger("ThinOff");
        }
        foreach ( GameObject go in thinTails ) {
            go.SetActive(false);
        }
    }
}
