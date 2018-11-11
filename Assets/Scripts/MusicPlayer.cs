using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Allows MusicPlayer object to persist between scenes and decrease at a specified rate if necessary */
public class MusicPlayer : MonoBehaviour {

    Player player;
    public bool decreasing = false;
    public int decreaseFactor;
    float volumeDecrement;

    private void Awake()
    {
        SetUpSingleton();
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        volumeDecrement = GetComponent<AudioSource>().volume / decreaseFactor;
    }

    private void Update()
    {
        if (player == null || player.destroyed)
        {
            Destroy(gameObject);
        }
        if (decreasing && !Timer.IsPaused())
        {
            GetComponent<AudioSource>().volume -= volumeDecrement;
            if (GetComponent<AudioSource>().volume <= 0)
            {
                Destroy(gameObject);
            }
        }
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
