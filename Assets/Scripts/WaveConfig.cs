using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Allows customization of each wave, including enemy type, spawn rate, num of enemies,
 *  movespee dof enemies, along with other fields that allow multiple waves to spawn
 *  synchronously and customize a delay in between wave spawns */
[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject {

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] double spawnInterval;
    [SerializeField] int numOfEnemies;
    [SerializeField] float movespeed;
    [SerializeField] double delay;
    [SerializeField] bool skipDelay = false;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }

    public List<Transform> GetWaypoints()
    {
        List<Transform> waveWaypoints = new List<Transform>();
        foreach (Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }
    public double GetSpawnInterval() { return spawnInterval; }
    public int GetNumOfEnemies() { return numOfEnemies; }
    public float GetMovespeed() { return movespeed; }
    public double GetDelay() { return delay; }
    public bool GetSkipDelay() { return skipDelay; }
}
