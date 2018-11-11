using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Handles lives in top-left corner of game interface */
public class GameOverlay : MonoBehaviour {

    [SerializeField] Sprite lifeOn;
    [SerializeField] Sprite lifeOff;

    public void LifeOn(int index)
    {
        transform.GetChild(index).GetComponent<Image>().sprite = lifeOn;
    }

    public void LifeOff(int index)
    {
        transform.GetChild(index).GetComponent<Image>().sprite = lifeOff;
    }
}
