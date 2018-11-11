using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Allows background to persist across scenes */
public class Background : MonoBehaviour {

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
