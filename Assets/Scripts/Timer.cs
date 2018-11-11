using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Keeps track of the paused state of the game and also contains
 * a general delay function which simulates the pausing of coroutines */
public class Timer : MonoBehaviour {

    private static double gameTimeElapsed;
    private static bool readyToPause = false;
    public static bool paused = false;

    private void Update()
    {
        if (readyToPause)
        {
            Pause();
        }
        if (!paused)
        {
            gameTimeElapsed += Time.deltaTime;
        }
    }

    public static void SetReadyToPause()
    {
        readyToPause = true;
    }

    public static void NoPause()
    {
        readyToPause = false;
    }

    private void Pause()
    {
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Submit"))
        {
            InGameCanvasHandler canvasHandler = FindObjectOfType<InGameCanvasHandler>();
            if (!paused)
            {
                paused = true;
                canvasHandler.ShowMenu();
            }
            else
            {
                canvasHandler.Resume();
            }
        }
    }

    public static bool IsPaused()
    {
        return paused;
    }

    public static IEnumerator Delay(double seconds)
    {
        if (!paused)
        {
            var releaseTime = gameTimeElapsed + seconds;
            while (gameTimeElapsed < releaseTime)
            {
                yield return null;
            }
        }
    }
}