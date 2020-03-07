using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchIndicator : MonoBehaviour {

    public float minTouchdownTime;

    private RectTransform fillLevelTransform;

    private void Start() {
        fillLevelTransform = transform.Find("Fill Level").GetComponent<RectTransform>();
    }
}
