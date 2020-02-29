using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StyleManager : MonoBehaviour {

    public MeshRenderer baseMaterial;
    public Skybox planetSkybox;
    public MeshRenderer planetMaterial;
    public ParticleSystem playerParticleSystem;
    public SpriteRenderer[] flowerPrefabs;

    [Header("Base-Textures & Skyboxes")]
    public Texture2D[] baseTextures;
    public Material[] skyboxes;

    private IUIColorManager uiColorManager;

    public void Init(SavedData savedData) {
        uiColorManager = GameObject.Find("GUI").GetComponent<IUIColorManager>();

        baseMaterial.sharedMaterial.mainTexture = baseTextures[(int) savedData.GetSelectedColorType()];
        planetSkybox.material = skyboxes[(int) savedData.GetSelectedColorType()];
        planetMaterial.sharedMaterial.color = savedData.GetColorByPurchaseableColorType(PurchaseableColorType.PLANET);

        if (playerParticleSystem != null) {
            ParticleSystem.MainModule mainModule = playerParticleSystem.main;
            mainModule.startColor = savedData.GetColorByPurchaseableColorType(PurchaseableColorType.PARTICLE);
        }

        uiColorManager.SetUIColor(savedData.GetColorByPurchaseableColorType(PurchaseableColorType.BASE));

        if (flowerPrefabs.Length > 0) {
            foreach(SpriteRenderer spre in flowerPrefabs) {
                spre.color = savedData.GetColorByPurchaseableColorType(PurchaseableColorType.PARTICLE);
            }
        }
    }
}
