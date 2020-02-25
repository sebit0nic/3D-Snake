using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleManager : MonoBehaviour {

    public MeshRenderer baseMaterial;
    public Skybox planetSkybox;
    public MeshRenderer planetMaterial;
    public ParticleSystem playerParticleSystem;
    public SpriteRenderer[] flowerPrefabs;

    [Header("Textures, Colors, etc...")]
    public Texture[] baseTextures;
    public Color[] baseColors;
    public Color[] planetColors;
    public Color[] particleColors;
    public Material[] baseSkybox;

    private IUIColorManager uiColorManager;

    public void Init(SavedData savedData) {
        uiColorManager = GameObject.Find("GUI").GetComponent<IUIColorManager>();

        baseMaterial.sharedMaterial.mainTexture = baseTextures[(int) savedData.GetSelectedColorType()];
        planetSkybox.material = baseSkybox[(int) savedData.GetSelectedColorType()];
        planetMaterial.sharedMaterial.color = planetColors[(int) savedData.GetSelectedColorType()];

        if (playerParticleSystem != null) {
            ParticleSystem.MainModule mainModule = playerParticleSystem.main;
            mainModule.startColor = particleColors[(int) savedData.GetSelectedColorType()];
        }

        uiColorManager.SetUIColor(baseColors[(int) savedData.GetSelectedColorType()]);

        if (flowerPrefabs.Length > 0) {
            foreach(SpriteRenderer spre in flowerPrefabs) {
                spre.color = particleColors[(int) savedData.GetSelectedColorType()];
            }
        }
    }
}
