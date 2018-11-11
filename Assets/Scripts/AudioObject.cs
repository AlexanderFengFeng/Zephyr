using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Allows easier instantiation of audio objects that can be paused */
public class AudioObject : MonoBehaviour {

    AudioSource audioSource;
    bool paused = false;

	void Start () {
        audioSource = GetComponent<AudioSource>();
        transform.position = Camera.main.transform.position;
        audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
        Pause();
        // Kills object when it finishes playing
        if (!paused && !audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
	}

    void Pause()
    {
        if (!paused && Timer.IsPaused())
        {
            paused = true;
            audioSource.Pause();
        }
        else if (paused && !(Timer.IsPaused()))
        {
            paused = false;
            audioSource.UnPause();
        }
    }
}
