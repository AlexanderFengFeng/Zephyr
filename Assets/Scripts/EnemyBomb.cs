using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectileBase;

/* Controls movement state of enemy bomb projectile */
public class EnemyBomb : Projectile
{

    [SerializeField] float rotation;
    public float stopFollowDistance;

    float distance;
    Vector3 playerPos;
    Vector3 direction;

    protected override void Fire()
    {
        if (FindObjectOfType<Player>() != null)
        {
            distance = Vector3.Distance(transform.position, FindObjectOfType<Player>().transform.position);
        }
        else
        {
            float rangeOfX = UnityEngine.Random.Range(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x,
                                                      Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x);
            Vector3 newDirection = new Vector3(rangeOfX, Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y, 0);
            direction = Vector3.Normalize(newDirection);
        }
    }

    protected override void Move()
    {
        if (!paused)
        {
            if (distance > 0 && FindObjectOfType<Player>() != null)
            {
                playerPos = FindObjectOfType<Player>().transform.position;

                if (Vector3.Distance(playerPos, transform.position) < stopFollowDistance)
                {
                    distance = 0;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerPos, projectileSpeed * Time.deltaTime);
                    direction = Vector3.Normalize(playerPos - transform.position);
                    distance -= projectileSpeed * Time.deltaTime;
                }
            }
            else
            {
                transform.position += direction * projectileSpeed * Time.deltaTime;
            }
            transform.Rotate(0, 0, rotation * Time.deltaTime);
        }
    }

    protected override void PauseMovement() { }

    protected override void ResumeMovement() { }

}
