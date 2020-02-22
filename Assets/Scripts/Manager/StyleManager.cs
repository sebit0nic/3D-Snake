using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleManager : MonoBehaviour {

    public MeshRenderer baseMaterial;
    public Skybox planetSkybox;
    public MeshRenderer planetMaterial;

    [Header("Textures, Colors, etc...")]
    public Texture[] baseTextures;
    public Color[] baseColors;
    public Color[] planetColors;
    public Material[] baseSkybox;

    public void Init(SavedData savedData) {
        baseMaterial.sharedMaterial.mainTexture = baseTextures[(int) savedData.GetSelectedColorType()];
        planetSkybox.material = baseSkybox[(int) savedData.GetSelectedColorType()];
        planetMaterial.sharedMaterial.color = planetColors[(int) savedData.GetSelectedColorType()];
    }
}
