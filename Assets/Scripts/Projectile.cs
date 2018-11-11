using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectileBase
{
    /* Parent class for projectiles of all sorts */
    public abstract class Projectile : MonoBehaviour
    {

        [SerializeField] protected float projectileSpeed;
        [SerializeField] protected GameObject projectileSFX;

        protected bool paused = false;

        private void Start()
        {
            Fire();
            PlaySound();
        }

        protected void Update()
        {
            Move();
            PauseProjectile();
        }

        abstract protected void Fire();

        abstract protected void Move();

        private void PlaySound()
        {
            Instantiate(projectileSFX, Camera.main.transform.position, Quaternion.identity);
        }

        protected void PauseProjectile()
        {
            if (!paused && Timer.IsPaused())
            {
                PauseMovement();
                paused = true;

            }
            else if (paused && !(Timer.IsPaused()))
            {
                ResumeMovement();
                paused = false;
            }
        }

        abstract protected void PauseMovement();

        abstract protected void ResumeMovement();
    }
}
