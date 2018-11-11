using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles menu switching in the start menu only */
public class StartMenuCanvasHandler : MonoBehaviour
{

    [SerializeField] CanvasGroup menu;
    [SerializeField] CanvasGroup helpMenu;

    private void Start()
    {
        Hide(helpMenu);
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

    public void ShowHelp()
    {
        Hide(menu);
        Show(helpMenu);
    }

    public void HideHelp()
    {
        Hide(helpMenu);
        Show(menu);
    }
}