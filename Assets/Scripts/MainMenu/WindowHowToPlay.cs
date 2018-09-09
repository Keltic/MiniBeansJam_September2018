using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowHowToPlay : WindowBase
{
    [SerializeField]
    private GameObject[] pages;


    override public void  OnShow()
    {
        this.SwitchToPage(0);
    }

    public void SwitchToPage(int index)
    {
        for(int i = 0; i < this.pages.Length; i++)
        {
            this.pages[i].SetActive(false);
        }

        this.pages[index].SetActive(true);
    }
}
