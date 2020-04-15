using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Puts the right hat on the snake according to the currently selected one.
/// </summary>
public class SnakeHatChooser : MonoBehaviour {
    
    public GameObject[] hats;

    public void Init( SavedData savedData ) {
        hats[(int) savedData.GetSelectedHatType()].SetActive( true );
    }
}
