using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBase
{
    public abstract class Enemy : MonoBehaviour
    {

        [Header("Health")]
        [SerializeField] protected float health;
        [SerializeField] protected Color hitColor;
        [SerializeField] protected double flashDuration;
        protected SpriteRenderer spriteRenderer;
        protected Color baseColor;

        [Header("Laser")]
        [SerializeField] protected float laserOffset;
        [SerializeField] protected GameObject projectile;
        protected double shotCounter;

        [Header("Death")]
        [SerializeField] protected GameObject deathVFX;
        [SerializeField] protected GameObject deathSFX;

        protected bool paused = false;

        // Use this for initialization
        protected void Start()
        {
            InitializeEnemy();
        }

        protected abstract void InitializeEnemy();


        // Update is called once per frame
        protected void Update()
        {
            Pause();
            if (!paused)
            {   
                Routine();
            }
        }

        protected void Pause()
        {
            if (!paused && Timer.IsPaused())
            {
                paused = true;
            }
            else if (paused && !(Timer.IsPaused()))
            {
                paused = false;
            }
        }

        // Fire
        protected abstract void Routine();

        // Hit by player
        protected abstract void OnTriggerEnter2D(Collider2D other);

        protected void ProcessHit(DamageDealer damageDealer)
        {
            health -= damageDealer.GetDamage();
            damageDealer.Hit();
            if (health <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(HitFlash());
            }
        }

        protected IEnumerator HitFlash()
        {
            spriteRenderer.color = hitColor;
            yield return StartCoroutine(Timer.Delay(flashDuration));
            spriteRenderer.color = baseColor;
        }

        protected abstract void Die();
    }
}
