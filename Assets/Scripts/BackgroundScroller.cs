using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Allows background to scroll infinitely */
public class BackgroundScroller : MonoBehaviour {

    [SerializeField] float backgroundScrollerSpeed;
    Material myMaterial;
    Vector2 offset;

	// Use this for initialization
	void Start () {
        myMaterial = GetComponent<Renderer>().material;
        offset = new Vector2(0f, backgroundScrollerSpeed);
	}
	
	// Update is called once per frame
	void Update () {
        if (!Timer.IsPaused())
            myMaterial.mainTextureOffset += offset * Time.deltaTime;
	}
}
