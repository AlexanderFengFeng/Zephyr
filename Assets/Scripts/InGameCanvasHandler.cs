using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles menu switching and transitioning into a game complete screen */
public class InGameCanvasHandler : MonoBehaviour {

    [SerializeField] CanvasGroup menu;
    [SerializeField] CanvasGroup gameOver;
    [SerializeField] CanvasGroup stageComplete;
    [SerializeField] CanvasGroup helpMenu;
    [SerializeField] CanvasGroup pauseOverlay;
    [SerializeField] double gameOverDelay;
    [SerializeField] double stageCompleteDelay;
    [SerializeField] double nextStageDelay;

    private void Start()
    {
        ShowGame();
    }

    public void ShowGame()
    {
        Hide(menu);
        Hide(gameOver);
        Hide(stageComplete);
        Hide(helpMenu);
        Hide(pauseOverlay);
    }

    private void Hide(CanvasGroup canvas)
    {
        canvas.alpha = 0f;
        canvas.blocksRaycasts = false;
    }

    private void Show(CanvasGroup canvas)
    {
        canvas.alpha = 1f;
        canvas.blocksRaycasts = true;
    }

    public void ShowMenu()
    {
        Show(menu);
        Hide(helpMenu);
        Show(pauseOverlay);
    }

    public void MenuToHelp()
    {
        Hide(menu);
        Show(helpMenu);
    }

    public void HelpToMenu()
    {
        Hide(helpMenu);
        ShowMenu();
    }

    public void Resume()
    {
        Timer.paused = false;
        ShowGame();
    }

    public IEnumerator ShowGameOverDelayed()
    {
        yield return StartCoroutine(Timer.Delay(gameOverDelay));
        Show(gameOver);
        Timer.NoPause();
        FindObjectOfType<Player>().DestroyPlayer();
    }

    public IEnumerator ShowStageCompleteDelayed()
    {
        yield return StartCoroutine(Timer.Delay(stageCompleteDelay));
        FindObjectOfType<EnemySpawner>().bossAudioPlayer.PlayWinJingle();
        Timer.NoPause();
        Show(stageComplete);
        yield return StartCoroutine(Timer.Delay(nextStageDelay));
        FindObjectOfType<Player>().DestroyPlayer();
        FindObjectOfType<Level>().LoadStart();
    }
}
