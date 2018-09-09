using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableComponent : MonoBehaviour
{
    public void OnClick(InputController.UpgradeModes mode)
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
                    switch (mode)
                    {
                        case InputController.UpgradeModes.Bomber:
                            if (!ai.IsHuman)
                            {
                                ai.ChangeWeaponType(AIComponent.WeaponTypes.Exploder);
                            }
                            break;
                        case InputController.UpgradeModes.Runner:
                            if (!ai.IsHuman)
                            {
                                ai.ChangeWeaponType(AIComponent.WeaponTypes.Runner);
                            }
                            break;
                        case InputController.UpgradeModes.Shooter:
                            if (!ai.IsHuman)
                            {
                                ai.ChangeWeaponType(AIComponent.WeaponTypes.Ranged);
                            }
                            break;
                        case InputController.UpgradeModes.Trickster:
                            if (!ai.IsHuman)
                            {
                                ai.ChangeWeaponType(AIComponent.WeaponTypes.Trickster);
                            }
                            break;
                        default:
                            guiController.ShowViewRadiusMarker(true, ai);
                            guiController.ShowShootRadiusMarker(true, ai);
                            break;
                    }
                }
            }
        }
    }
}
