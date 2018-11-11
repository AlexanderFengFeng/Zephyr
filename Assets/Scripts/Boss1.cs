using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyBase;
using System;

/* Contains functions specific to this boss, including its unique entry
 * behavior and firing patterns */
public class Boss1 : Enemy
{
    [Header("Boss 1")]
    public GameObject path;
    public double fireTiming;
    public float preReadySpeed;
    public float normalSpeed;
    public double readyDelay;
    public double bossDestroyDelay;
    public double destroyInterval;
    public BossAudioPlayer bossAudioPlayer;

    List<Transform> waypoints;
    bool ready = false;
    bool waiting = false;
    bool destroyed = false;
    int pathIndex = 0;
    double fireCounter = 0;


    protected override void InitializeEnemy()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
        GetWayPoints();
    }

    private void GetWayPoints()
    {
        waypoints = new List<Transform>();
        foreach (Transform child in path.transform)
        {
            waypoints.Add(child);
        }
    }

    protected override void Routine()
    {
        if (!waiting && !destroyed)
            StartCoroutine(BossRoutine());
    }

    private IEnumerator BossRoutine()
    {
        if (!ready)
        {
            // Move to starting point slowly, then stop
            yield return StartCoroutine(MoveToPos(waypoints[pathIndex], preReadySpeed));
        }
        if (ready)
        {
            // Move normally towards points
            yield return StartCoroutine(MoveToPos(waypoints[pathIndex], normalSpeed));
            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0)
            {
                Fire();
                fireCounter = fireTiming;
            }
        }
    }

    private IEnumerator MoveToPos(Transform waypoint, float speed)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            waypoint.position,
            preReadySpeed * Time.deltaTime);

        if (transform.position == waypoint.position)
        {
            if (!ready)
            {
                waiting = true;
                bossAudioPlayer.PlayBossIntro();
                yield return StartCoroutine(Timer.Delay(readyDelay));
                waiting = false;
                ready = true;
                bossAudioPlayer.PlayBossTheme();
            }

            pathIndex++;
            if (pathIndex >= waypoints.Count)
            {
                pathIndex = 0;
            }
        }

        yield return null;
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
        if (!destroyed)
        {
            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            if (ready)
            {
                if (damageDealer)
                    ProcessHit(damageDealer);
            }
            else
            {
                if (damageDealer)
                    damageDealer.Hit();
            }
        }
    }

    protected override void Die()
    {
        destroyed = true;
        FindObjectOfType<Player>().endStage = true;
        bossAudioPlayer.DestroyBossTheme();

        StartCoroutine(DestroyBoss());
        StartCoroutine(FindObjectOfType<InGameCanvasHandler>().ShowStageCompleteDelayed());
        Instantiate(
            deathVFX,
            transform.position,
            Quaternion.identity);
        Instantiate(deathSFX, Camera.main.transform.position, Quaternion.identity);
    }

    private IEnumerator DestroyBoss() {
        Destroy(FindObjectOfType<BossAudioPlayer>());
        Color bossColor = spriteRenderer.color;
        float decrementOpacity = (bossColor.a) * (float)(destroyInterval / bossDestroyDelay);
        for (int i = 0; i < bossDestroyDelay; i++)
        {
            bossColor.a -= decrementOpacity;
            spriteRenderer.color = bossColor;

            yield return StartCoroutine(Timer.Delay(destroyInterval));
        }
    }
}
