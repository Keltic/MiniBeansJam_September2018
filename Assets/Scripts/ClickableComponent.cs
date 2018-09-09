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
            AudioController audio = Camera.main.GetComponent<AudioController>();
            if (guiController != null && audio != null)
            {
                if(guiController.ZombieCount == 0)
                {
                    //This is the click for the initial zombie spawn
                    ai.Infect();
                    audio.PlayRandomMoan(0.5f);
                }
                else
                {
                    //It is a click during normal gameplay

                    switch (mode)
                    {
                        case InputController.UpgradeModes.Bomber:
                            if (!ai.IsHuman && guiController.SubtractPoints(5))
                            {
                                ai.ChangeWeaponType(AIComponent.WeaponTypes.Exploder);
                                audio.PlayRandomMoan(0.5f);
                            }
                            break;
                        case InputController.UpgradeModes.Runner:
                            if (!ai.IsHuman && guiController.SubtractPoints(10))
                            {
                                ai.ChangeWeaponType(AIComponent.WeaponTypes.Runner);
                                audio.PlayRandomMoan(0.5f);
                            }
                            break;
                        case InputController.UpgradeModes.Shooter:
                            if (!ai.IsHuman && guiController.SubtractPoints(15))
                            {
                                ai.ChangeWeaponType(AIComponent.WeaponTypes.Ranged);
                                audio.PlayRandomMoan(0.5f);
                            }
                            break;
                        case InputController.UpgradeModes.Trickster:
                            if (!ai.IsHuman && guiController.SubtractPoints(5))
                            {
                                ai.ChangeWeaponType(AIComponent.WeaponTypes.Trickster);
                                audio.PlayRandomMoan(0.5f);
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
