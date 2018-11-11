using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Cycles through the three sprites of the Jet */
public class JetAnimator : MonoBehaviour
{

    [SerializeField] List<Sprite> sprites;
    [SerializeField] float animationInterval;

    float animationCounter = 0f;
    int spriteIndex = 0;

    public void AnimateJet()
    {
        animationCounter += Time.deltaTime;
        if (animationCounter >= animationInterval)
        {
            spriteIndex = (spriteIndex + 1) % sprites.Count;
            GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
            animationCounter = 0;
        }
    }
}
