using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuGuiController : MonoBehaviour
{
    public enum WindowTypes
    {
        MainMenu,
        HowToPlay,
        Credits
    }

    [SerializeField]
    private WindowMainMenu windowMainMenu;
    [SerializeField]
    private WindowHowToPlay windowHowToPlay;
    [SerializeField]
    private WindowCredits windowCredits;

    public void Start()
    {
        this.OnClickButtonBackToMainMenu();
    }

    public void ShowWindow(WindowTypes type)
    {
        this.CloseAllWindows();

        switch (type)
        {
            case WindowTypes.HowToPlay:
                this.windowHowToPlay.Show();
                break;
            case WindowTypes.Credits:
                this.windowCredits.Show();
                break;
            default:
                this.windowMainMenu.Show();
                break;
        }
    }

    public void OnClickButtonBackToMainMenu()
    {
        this.ShowWindow(WindowTypes.MainMenu);
    }

    private void CloseAllWindows()
    {
        this.windowMainMenu.Hide();
        this.windowHowToPlay.Hide();
        this.windowCredits.Hide();
    }
}
