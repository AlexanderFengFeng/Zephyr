using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Contains the necessary audio objects that play when the boss online */
public class BossAudioPlayer : MonoBehaviour {

    public GameObject bossIntro;
    public MusicPlayer bossTheme;
    private MusicPlayer themePlaying;
    public GameObject winJingle;

    public void PlayBossIntro()
    {
        Instantiate(bossIntro, Camera.main.transform.position, Quaternion.identity);
    }

    public void PlayBossTheme()
    {
        themePlaying = Instantiate(bossTheme, Camera.main.transform.position, Quaternion.identity);
    }

    public void PlayWinJingle()
    {
        Instantiate(winJingle, Camera.main.transform.position, Quaternion.identity);
    }

    public void DestroyBossTheme()
    {
        Destroy(themePlaying.gameObject);
    }

}
