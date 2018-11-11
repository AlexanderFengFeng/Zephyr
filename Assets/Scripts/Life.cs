using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Handles the rendering of each life object in the interface */
public class Life : MonoBehaviour {

    [SerializeField] Sprite lifeOn;
    [SerializeField] Sprite lifeOff;

    public void LifeOn()
    {
        GetComponent<Image>().sprite = lifeOn;
    }

    public void LifeOff()
    {
        GetComponent<Image>().sprite = lifeOff;
    }
}
