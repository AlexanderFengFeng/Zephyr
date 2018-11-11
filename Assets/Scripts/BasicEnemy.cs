using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyBase;

public class BasicEnemy : Enemy {

    [Header("Position")]
    public float xPadding;
    public float yPadding;
    private float xMin, xMax, yMin, yMax;

    [Header("Firing")]
    public float minShotInterval;
    public float maxShotInterval;

    protected override void InitializeEnemy()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
        SetUpBoundaries();
        PrepFirstRandomShot();
    }

    private void SetUpBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yPadding;
    }

    private void PrepFirstRandomShot()
    {
        shotCounter = UnityEngine.Random.Range(minShotInterval, maxShotInterval);
    }

    protected override void Routine()
    {
        if (transform.position.x > xMin && transform.position.x < xMax &&
            transform.position.y < yMax && transform.position.y > yMin)
            CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minShotInterval, maxShotInterval);
        }
    }

    private void Fire()
    {
        Instantiate(
            projectile,
            new Vector2(transform.position.x, transform.position.y - laserOffset),
            Quaternion.identity);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer)
            ProcessHit(damageDealer);
    }

    protected override void Die()
    {
        Destroy(gameObject);
        Instantiate(
            deathVFX,
            transform.position,
            Quaternion.identity);
        Instantiate(deathSFX, Camera.main.transform.position, Quaternion.identity);
    }
}
