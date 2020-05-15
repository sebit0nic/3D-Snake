using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Handles the coloring of materials according to the selected style.
/// </summary>
public class StyleManager : MonoBehaviour {

    public MeshRenderer baseMaterial;
    public Skybox planetSkybox;
    public Skybox planetSkyboxPortrait;
    public MeshRenderer planetMaterial;
    public ParticleSystem playerParticleSystem;
    public SpriteRenderer[] flowerPrefabs;

    [Header("Base-Textures & Skyboxes")]
    public Texture2D[] baseTextures;
    public Material[] skyboxes;

    private IUIColorManager uiColorManager;
    private const string guiGameobjectTag = "GUI";

    public void Init( SavedData savedData ) {
        uiColorManager = GameObject.Find( guiGameobjectTag ).GetComponent<IUIColorManager>();

        baseMaterial.sharedMaterial.mainTexture = baseTextures[(int) savedData.GetSelectedColorType()];
        planetSkybox.material = skyboxes[(int) savedData.GetSelectedColorType()];
        planetMaterial.sharedMaterial.color = savedData.GetColorByPurchaseableColorType( PurchaseableColorType.PLANET );

        if( playerParticleSystem != null ) {
            ParticleSystem.MainModule mainModule = playerParticleSystem.main;
            mainModule.startColor = savedData.GetColorByPurchaseableColorType( PurchaseableColorType.PARTICLE );
        }

        if( planetSkyboxPortrait != null ) {
            planetSkyboxPortrait.material = skyboxes[(int) savedData.GetSelectedColorType()];
        }

        uiColorManager.SetUIColor( savedData.GetColorByPurchaseableColorType( PurchaseableColorType.BASE ) );

        if( flowerPrefabs.Length > 0 ) {
            foreach( SpriteRenderer spre in flowerPrefabs ) {
                spre.color = savedData.GetColorByPurchaseableColorType( PurchaseableColorType.PARTICLE );
            }
        }
    }

    /// <summary>
    /// Init all materials and colors again after player previews another style in the shop scene.
    /// </summary>
    public void InitByIndex( SavedData savedData, int index ) {
        baseMaterial.sharedMaterial.mainTexture = baseTextures[index];
        planetSkybox.material = skyboxes[index];
        planetMaterial.sharedMaterial.color = savedData.GetColorByPurchaseableColorIndex( PurchaseableColorType.PLANET, index );

        if( playerParticleSystem != null ) {
            ParticleSystem.MainModule mainModule = playerParticleSystem.main;
            mainModule.startColor = savedData.GetColorByPurchaseableColorIndex( PurchaseableColorType.PARTICLE, index );
        }

        uiColorManager.SetUIColor( savedData.GetColorByPurchaseableColorIndex( PurchaseableColorType.BASE, index ) );

        if( flowerPrefabs.Length > 0 ) {
            foreach( SpriteRenderer spre in flowerPrefabs ) {
                spre.color = savedData.GetColorByPurchaseableColorIndex( PurchaseableColorType.PARTICLE, index );
            }
        }
    }
}
