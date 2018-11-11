using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectileBase;

/* Controls movement and state of player laser */
public class PlayerLaser : Projectile
{
    Vector2 pausedVelocity = new Vector2(0, 0);

    protected override void Fire()
    {
        DontDestroyOnLoad(gameObject);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
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
