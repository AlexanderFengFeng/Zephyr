using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows shredder object to destroy objects it collides with (primarily projectiles)
public class Shredder : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
}
