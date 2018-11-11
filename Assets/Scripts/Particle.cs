using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles behavior of all particles */
public class Particle : MonoBehaviour {

    [SerializeField] double particleDelay;
    bool paused = false;

    private void Start()
    {
        if (tag != "UI Particle")
            StartCoroutine(KillParticle());
    }

    void Update ()
    {
        PauseParticle();
    }

    private void PauseParticle()
    {
        if (!paused && Timer.IsPaused())
        {
            GetComponent<ParticleSystem>().Pause();
            paused = true;
        }
        else if (paused && !(Timer.IsPaused()))
        {
            GetComponent<ParticleSystem>().Play();
            paused = false;
        }
    }

    // Used to kill specifically game particles. UI particles will automatically be destroyed
    IEnumerator KillParticle()
    {
        yield return StartCoroutine(Timer.Delay(particleDelay));
        Destroy(gameObject);
    }
}
