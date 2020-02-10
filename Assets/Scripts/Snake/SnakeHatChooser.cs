using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHatChooser : MonoBehaviour {
    
    public GameObject[] hats;

    public void Init(SavedData savedData) {
        hats[(int) savedData.GetSelectedHatType()].SetActive(true);
    }
}
