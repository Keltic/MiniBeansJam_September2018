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
    [SerializeField]
    private Text valueVillagerCount;
    [SerializeField]
    private Slider sliderVillagerCount;

    public void Start()
    {
        this.OnClickButtonBackToMainMenu();
        this.sliderVillagerCount.value = 200;
        PlayerPrefs.SetInt("VillagerCount", 200);
        AudioController audio = Camera.main.GetComponent<AudioController>();
        if(audio != null)
        {
            audio.PlaySfx(AudioController.SfxTypes.Brains1, 0.2f);
        }
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

    public void OnValueChangedVillagerSlider(float value)
    {
        value = (int)value;
        this.valueVillagerCount.text = value.ToString();
        PlayerPrefs.SetInt("VillagerCount", (int)value);
    }

    private void CloseAllWindows()
    {
        this.windowMainMenu.Hide();
        this.windowHowToPlay.Hide();
        this.windowCredits.Hide();
    }
}
