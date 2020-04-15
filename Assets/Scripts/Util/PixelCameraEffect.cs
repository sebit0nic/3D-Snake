using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility script for the pixel camera material
/// </summary>
public class PixelCameraEffect : MonoBehaviour {

    public Material material;
    public int pixelDensity = 300;

    private Vector2 aspectRatioData;
    private const string aspectRatioString = "_AspectRatioMultiplier";
    private const string pixelDensityString = "_PixelDensity";

    private void OnRenderImage( RenderTexture source, RenderTexture destination ) {
        if ( Screen.height > Screen.width ) {
            aspectRatioData = new Vector2( (float) Screen.width / Screen.height, 1 );
        } else {
            aspectRatioData = new Vector2( 1, (float) Screen.height / Screen.width );
        }
        material.SetVector( aspectRatioString, aspectRatioData );
        material.SetInt( pixelDensityString, pixelDensity );
        Graphics.Blit( source, destination, material );
    }
}
