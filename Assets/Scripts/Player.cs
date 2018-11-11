using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Contains functions belonging to the player ship, including movement, firing,
 * commands to instantiate player-originating objects (like projectiles and some sounds) */
public class Player : MonoBehaviour {

    [Header("Position")]
    [SerializeField] float movespeed;
    [SerializeField] float xPadding;
    [SerializeField] float yPadding;
    float xMin, xMax, yMin, yMax;
    public bool destroyed;

    [Header("Jet")]
    [SerializeField] GameObject playerJet;
    [SerializeField] float jetOffset;
    GameObject jet;
    JetAnimator jetAnimator;

    [Header("Health")]
    [SerializeField] int health;
    [SerializeField] GameObject hitSFX;
    [SerializeField] float invulnDuration;
    [SerializeField] float flashPeriod;

    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float cannonOffset;
    [SerializeField] float laserOffset;
    [SerializeField] float projectileSpeed;
    [SerializeField] double firingPeriod;
    Coroutine firingCoroutine;
    bool shooting = false;

    [Header("Death")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject deathSFX;

    [Header("UI")]
    [SerializeField] int maxLives;
    bool paused = false;
    bool pausedRender = false;
    bool loaded = false;
    GameOverlay gameOverlay;

    [Header("Conditional Fields")]
    public GameObject pPowerUpProjectile;
    public float centerLaserOffset;

    /* Timing statuses */

    // Invulnerability
    bool invuln = false;
    public bool endStage = false;
    float invulnCounter;

    // Fire PowerUp
    bool pPowerUp = false;
    double firePowerUpCounter = 0d;

    // Shield PowerUp
    double shieldCounter = 0d;

    private void Awake()
    {
        int playerCount = FindObjectsOfType<Player>().Length;
        if (playerCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            CreateJet();
            DontDestroyOnLoad(jet);
        }
    }

    // Use this for initialization
    void Start ()
    {
        InitializeStart();
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
        if (!paused && !destroyed)
        {
            InitializeGame();
            UpdateJet();

            Move();
            FireContinuously();
            TimeDown();
        }
    }


    // Start menu initialization
    private void InitializeStart()
    {
        SetUpBoundaries();
    }

    private void SetUpBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yPadding;
    }

    // Core game initialization
    private void InitializeGame()
    {
        if (!loaded && SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
            SetUpLives();
            Timer.SetReadyToPause();
            loaded = true;
        }
    }

    // Pause
    private void Pause()
    {
        if (!paused && Timer.IsPaused())
        {
            bool rendered = GetComponent<Renderer>().enabled;
            if (!rendered)
            {
                Render();
                pausedRender = true;
            }
            paused = true;
        }
        else if (paused && !(Timer.IsPaused()))
        {
            if (pausedRender)
            {
                Unrender();
                pausedRender = false;
            }
            paused = false;
        }
    }

    // Visualization
    private void Render()
    {
        GetComponent<Renderer>().enabled = true;
        jet.GetComponent<Renderer>().enabled = true;
    }

    private void Unrender()
    {
        GetComponent<Renderer>().enabled = false;
        jet.GetComponent<Renderer>().enabled = false;
    }


    // Movement
    private void Move()
    {
        if (Input.GetMouseButton(1))
        {
            MoveWithMouse();
        }
        else
        {
            float deltaX = Input.GetAxisRaw("Horizontal")*movespeed*Time.deltaTime;
            float deltaY = Input.GetAxisRaw("Vertical")*movespeed*Time.deltaTime;

            float newXPos = Mathf.Clamp(transform.position.x+deltaX, xMin, xMax);
            float newYPos = Mathf.Clamp(transform.position.y+deltaY, yMin, yMax);

            float movementThisFrame = movespeed * Time.deltaTime;
  
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(newXPos, newYPos),
                movementThisFrame);
        }
    }

    private void MoveWithMouse()
    {
        Vector3 targetPos = Input.mousePosition;
        targetPos = Camera.main.ScreenToWorldPoint(targetPos);
        targetPos.x = Mathf.Clamp(targetPos.x, xMin, xMax);
        targetPos.y = Mathf.Clamp(targetPos.y, yMin, yMax);

        float movementThisFrame = movespeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            movementThisFrame);
    }

    // Fire
    private void FireContinuously()
    {

        if (Input.GetButtonDown("Fire1") && !shooting)
        {
            firingCoroutine = StartCoroutine(Fire());
            shooting = true;
        }
        else if ((Input.GetButtonUp("Fire1") && shooting) || (!(Input.GetButton("Fire1")) && shooting))
        {
            StopCoroutine(firingCoroutine);
            shooting = false;
        }
    }

    IEnumerator Fire()
    {
        while (true)
        {
            if (pPowerUp)
            {
                yield return StartCoroutine(FireCenter(pPowerUpProjectile));
            }
            else
            {
                yield return StartCoroutine(FireDefault());
            }
        }
    }

    IEnumerator FireDefault()
    {
        Instantiate(
            laserPrefab,
            new Vector2(transform.position.x - cannonOffset, transform.position.y + laserOffset),
            Quaternion.identity);

        yield return StartCoroutine(Timer.Delay(firingPeriod));

        Instantiate(
            laserPrefab,
            new Vector2(transform.position.x + cannonOffset, transform.position.y + laserOffset),
            Quaternion.identity);

        yield return StartCoroutine(Timer.Delay(firingPeriod));
    }

    IEnumerator FireCenter(GameObject projectilePrefab)
    {
        Instantiate(
            projectilePrefab,
            new Vector2(transform.position.x, transform.position.y + centerLaserOffset),
            Quaternion.identity);

        yield return StartCoroutine(Timer.Delay(firingPeriod));
    }


    // Interact with objects
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!destroyed)
        {
            if (other.gameObject.tag == "Enemy Laser" || other.gameObject.tag == "Enemy")
            {
                if (!invuln && !endStage)
                {
                    Hit(other.gameObject);
                }
            }
        }

    }

    public void PowerUp(PowerUp powerUp)
    {
        if (powerUp.tag == "P PowerUp")
        {
            pPowerUp = true;
            firePowerUpCounter = powerUp.powerDuration;
            // Assign other powerups to be false here when you make other powerups
        }
        if (powerUp.tag == "Sheild")
        {
            shieldCounter = powerUp.powerDuration;
        }
    }

    // Hit and damaged
    private void Hit(GameObject other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        health -= damageDealer.GetDamage();
        RemoveLife();

        if (other.tag == "Enemy Laser")
            damageDealer.Hit();

        // Destroys player
        if (health <= 0)
        {
            Die();
        }
        // Hit causes hit sound to play and invuln to trigger
        else
        {
            invuln = true;
            invulnCounter = invulnDuration;
            Instantiate(hitSFX, Camera.main.transform.position, Quaternion.identity);
        }
    }

    private void Die()
    {
        Instantiate(
           deathVFX,
           transform.position,
           transform.rotation);
        Instantiate(deathSFX, Camera.main.transform.position, Quaternion.identity);

        StartCoroutine(FindObjectOfType<InGameCanvasHandler>().ShowGameOverDelayed());
        DisablePlayer();
    }

    private void DisablePlayer()
    {
        destroyed = true;
        Unrender();
    }

    public void DestroyPlayer()
    {
        Destroy(gameObject);
        Destroy(jet);
    }

    // Jet
    private void CreateJet()
    {
        jet = Instantiate(
            playerJet,
            new Vector3(transform.position.x,
                transform.position.y - jetOffset,
                transform.position.z),
            Quaternion.identity);
        jetAnimator = jet.GetComponent<JetAnimator>();
    }

    private void UpdateJet()
    {
        jet.transform.position = new Vector3(
            transform.position.x,
            transform.position.y - jetOffset,
            transform.position.z);
        jetAnimator.AnimateJet();
    }


    //Lives UI
    private void SetUpLives()
    {
        gameOverlay = FindObjectOfType<GameOverlay>();
    }

    private void RemoveLife()
    {
        gameOverlay.LifeOff(health);
    }

    private void GainLife()
    {
        gameOverlay.LifeOn(health);

    }

    // Timers
    private void TimeDown()
    {
        if (invuln)
        {
            DecrementInvuln();
        }
        if (firePowerUpCounter > 0)
        {
            DecrementFirePowerUp();
        }
        if (shieldCounter > 0)
        {
            DecrementShield();
        }

    }

    private void DecrementInvuln()
    {
        invulnCounter -= Time.deltaTime;
        if (invulnCounter <= 0)
        {
            invuln = false;
        }
        else
        {
            Flash();
        }
    }

    private void DecrementFirePowerUp() 
    {
        firePowerUpCounter -= Time.deltaTime;
        if (firePowerUpCounter <= 0)
        {
            pPowerUp = false;
        }
    }

    private void DecrementShield()
    {
        shieldCounter -= Time.deltaTime;
    }

    private void Flash()
    {
        bool rendered = GetComponent<Renderer>().enabled;
        if (rendered && invulnCounter % (flashPeriod * 2) >= flashPeriod)
        {
            Unrender();
        }
        else if (!rendered && invulnCounter % (flashPeriod * 2) <= flashPeriod)
        {
            Render();
        }
    }

}
