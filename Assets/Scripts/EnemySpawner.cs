using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Contains functions that automate enemy spawning and finish with a boss spawning event */
public class EnemySpawner : MonoBehaviour {

    [Header("Normal Waves")]
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;

    [SerializeField] bool looping = true;
    GameObject enemy;

    [Header("Boss")]
    [SerializeField] GameObject boss;
    [SerializeField] Transform starting;
    public BossAudioPlayer bossAudioPlayer;

    // Use this for initialization
    IEnumerator Start () {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
        if (!FindObjectOfType<Player>().destroyed)
        {
            SpawnBoss();
            FindObjectOfType<MusicPlayer>().decreasing = true;
        }

	}

    IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            WaveConfig currentWave = waveConfigs[waveIndex];
            if (FindObjectOfType<Player>() == null || FindObjectOfType<Player>().destroyed)
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
            if (currentWave.GetSkipDelay())
            {
                StartCoroutine(SpawnWave(currentWave));
                yield return null;
            }
            else
            {
                yield return StartCoroutine(SpawnWave(currentWave));
                yield return StartCoroutine(Timer.Delay(currentWave.GetDelay() * currentWave.GetMovespeed()));
            }
        }
    }
	
	IEnumerator SpawnWave(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.GetNumOfEnemies(); i++)
        {
            enemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoints()[0].transform.position,
                Quaternion.identity);
            enemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return StartCoroutine(Timer.Delay(waveConfig.GetSpawnInterval()));
        }
    }

    private void SpawnBoss()
    {
        Instantiate(boss, starting.position, Quaternion.identity);
    }
}
