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

    private IUIColorManager uiColorManager;
    private const string colorschemeTexturePath = "Assets/Textures/Colorscheme/Colorscheme_";
    private const string skyboxMaterialPath = "Assets/Materials/Skybox/Skybox_";

    public void Init(SavedData savedData) {
        uiColorManager = GameObject.Find("GUI").GetComponent<IUIColorManager>();

        baseMaterial.sharedMaterial.mainTexture = (Texture2D) AssetDatabase.LoadAssetAtPath(colorschemeTexturePath + (int)savedData.GetSelectedColorType() + ".png", typeof(Texture2D));
        planetSkybox.material = (Material) AssetDatabase.LoadAssetAtPath(skyboxMaterialPath + (int)savedData.GetSelectedColorType() + ".mat", typeof(Material));
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
