using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Used to allow enemies to wait and change speeds at a particular waypoint */
public class Waypoint : MonoBehaviour {

    [SerializeField] double wait = 0d;
    [SerializeField] float speedMod = 1f;

    public double GetWait() { return wait; }

    public float GetSpeedMod() { return speedMod; }
}
