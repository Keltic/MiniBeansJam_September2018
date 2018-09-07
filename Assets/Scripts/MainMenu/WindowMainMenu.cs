using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowMainMenu : WindowBase
{
    [SerializeField]
    private MainMenuGuiController mainMenuGuiController;

    public void OnClickButtonStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnClickButtonQuit()
    {
        Application.Quit();
    }

    public void OnClickButtonCredits()
    {
        this.mainMenuGuiController.ShowWindow(MainMenuGuiController.WindowTypes.Credits);
    }

    public void OnClickButtonHowToPlay()
    {
        this.mainMenuGuiController.ShowWindow(MainMenuGuiController.WindowTypes.HowToPlay);
    }
}
