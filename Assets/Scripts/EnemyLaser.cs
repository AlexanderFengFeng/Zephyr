using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectileBase;

/* Handles movement and state of enemy projectile (see player projectile and projectile classes) */
public class EnemyLaser : Projectile
{
    Vector2 pausedVelocity = new Vector2(0, 0);

    protected override void Fire()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
    }

    protected override void Move() { }

    protected override void PauseMovement()
    {
        pausedVelocity = GetComponent<Rigidbody2D>().velocity;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    protected override void ResumeMovement()
    {
        GetComponent<Rigidbody2D>().velocity = pausedVelocity;
        pausedVelocity = new Vector2(0, 0);
    }
}