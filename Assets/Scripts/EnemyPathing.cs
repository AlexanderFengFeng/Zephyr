using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Moves enemies through each wavepoint on their path and allows them to wait and change speeds depending on a waypoint */
public class EnemyPathing : MonoBehaviour {

    WaveConfig waveConfig;

    List<Transform> waypoints;
    int waypointIndex = 0;
    bool paused = false;
    bool waiting = false;

	// Use this for initialization
	void Start () {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
        waypointIndex++;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Pause();
        if (!paused && !waiting)
            StartCoroutine(Move());
    }


    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void Pause()
    {
        paused = Timer.IsPaused();
    }

    IEnumerator Move()
    {

        if (waypointIndex < waypoints.Count)
        {
            Waypoint waypoint = waypoints[waypointIndex].GetComponent<Waypoint>();
            Vector3 targetPos = waypoints[waypointIndex].transform.position;
            float movementThisFrame = waveConfig.GetMovespeed() * waypoint.GetSpeedMod() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(
                transform.position, targetPos, movementThisFrame);
            if (transform.position == targetPos)
            {
                waiting = true;
                yield return StartCoroutine(Timer.Delay(waypoint.GetWait()));
                waiting = false;
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
            yield return null;
        }
    }
}
