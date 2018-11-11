using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Contains functions that keep track of the position and visual state of the powerUp object */
public class PowerUp : MonoBehaviour {

    Vector3 direction;
    Vector3 playerPos;

    public float projectileSpeed;
    public double lifespan;
    public double lifespanToFlash;
    public double flashRate;
    public double powerDuration;

    public GameObject powerUpSFX;

    bool paused = false;
    bool pausedRender = false;

    private void Start()
    {
        if (FindObjectOfType<Player>() == null)
        {
            GenerateVector();
        }
        else
        {
            GetPlayerVector();
        }
    }

    private void Update()
    {
        Move();
        PausePowerUp();
    }

    private void GenerateVector()
    {
        float rangeOfX = UnityEngine.Random.Range(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x,
                                                    Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x);
        Vector3 newDirection = new Vector3(rangeOfX, Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y, 0);
        direction = Vector3.Normalize(newDirection);
    }

    private void GetPlayerVector()
    {
        direction = Vector3.Normalize(FindObjectOfType<Player>().transform.position - transform.position);
    }

    private void Move()
    {
        if (!paused)
        {
            DecreaseLifespan();
            transform.position += direction * projectileSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ActivatePowerUp(other.gameObject);
    }

    private void ActivatePowerUp(GameObject player)
    {
        player.GetComponent<Player>().PowerUp(gameObject.GetComponent<PowerUp>());
        Destroy(gameObject);
        PlaySound();
    }

    private void PlaySound()
    {
        Instantiate(powerUpSFX, Camera.main.transform.position, Quaternion.identity);
    }

    private void DecreaseLifespan()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= lifespanToFlash)
        {
            Flash();
            if (lifespan <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Flash()
    {
        bool rendered = GetComponent<Renderer>().enabled;
        if (rendered && lifespan % (flashRate * 2) >= flashRate)
        {
            GetComponent<Renderer>().enabled = false;
        }
        else if (!rendered && lifespan % (flashRate * 2) <= flashRate)
        {
            GetComponent<Renderer>().enabled = true;
        }
    }

    protected void PausePowerUp()
    {
        if (!paused && Timer.IsPaused())
        {
            paused = true;
            bool rendered = GetComponent<Renderer>().enabled;
            if (!rendered)
            {
                GetComponent<Renderer>().enabled = true;
                pausedRender = true;
            }
        }
        else if (paused && !(Timer.IsPaused()))
        {
            if (pausedRender)
            {
                GetComponent<Renderer>().enabled = false;
                pausedRender = false;
            }
            paused = false;
        }
    }
}
