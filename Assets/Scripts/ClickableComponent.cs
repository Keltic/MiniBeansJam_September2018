using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableComponent : MonoBehaviour
{
    public void OnClick()
    {
        AIComponent ai = this.GetComponent<AIComponent>();
        if(ai != null)
        {
            //It is an NPC or a Zombie
            GameGuiController guiController = GameObject.Find("Canvas_Game").GetComponent<GameGuiController>();
            if(guiController != null)
            {
                if(guiController.ZombieCount == 0)
                {
                    //This is the click for the initial zombie spawn
                    ai.Infect();
                }
                else
                {
                    //It is a click during normal gameplay
                }
            }
        }
    }
}
