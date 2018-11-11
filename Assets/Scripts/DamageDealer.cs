using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Allows objects to have store an amount of damage they would deal and destroys objects (projectiles) on contact */
public class DamageDealer : MonoBehaviour {

    [SerializeField] int damage;

    public int GetDamage() { return damage; }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
