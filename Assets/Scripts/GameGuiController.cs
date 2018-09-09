using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGuiController : MonoBehaviour
{
    [SerializeField]
    private Text textValueVillagerCount;
    [SerializeField]
    private Text textValueMilitaryCount;
    [SerializeField]
    private Text textValueZombieCount;
    [SerializeField]
    private Text textValuePoints;
    [SerializeField]
    private Text textPlayerAlert;
    [SerializeField]
    private Animator animatorPlayerAlert;
    [SerializeField]
    private SpriteRenderer viewRadiusMarker;
    [SerializeField]
    private SpriteRenderer shootRadiusMarker;
    [SerializeField]
    private Color colorZombieRadius;
    [SerializeField]
    private Color colorNpcRadius;
    [SerializeField]
    private Text textAtMouse;
    [SerializeField]
    private InputController inputController;

    private int villagerCount = 0;
    private int militaryCount = 0;
    private int zombieCount = 0;
    private int pointsCount = 0;

    private Coroutine coroutinePlayerAlert = null;
    private float coroutineTimer = 0;
    private bool alertFadeOutRunning = false;

    public int VillagerCount { get { return this.villagerCount; } }
    public int MilitaryCount { get { return this.militaryCount; } }
    public int ZombieCount { get { return this.zombieCount; } }
    public int PointsCount { get { return this.pointsCount; } }
    public Transform RadiusMarkerParent { get { return this.viewRadiusMarker.transform.parent; } }


    public void Awake()
    {
        EventController.EventNpcSpawned += this.OnNpcSpawned;
        EventController.EventNpcInfected += this.OnNpcInfected;
        EventController.EventZombieKilled += this.OnZombieKilled;

        this.textValueVillagerCount.text = "0";
        this.textValueMilitaryCount.text = "0";
        this.textValueZombieCount.text = "0";
        this.textValuePoints.text = "0";

        this.ShowPlayerAlert("Click on a citizen to spawn your first Zombie!", 4);

        this.ShowViewRadiusMarker(false);
        this.ShowShootRadiusMarker(false);
        this.ShowTextAtMouse(false);
    }

    public void Update()
    {
        this.textAtMouse.transform.position = Input.mousePosition;
    }

    public void ShowTextAtMouse(bool value, string message = "")
    {
        this.textAtMouse.gameObject.SetActive(value);

        if (value)
        {
            this.textAtMouse.text = message;
        }
    }

    public void ShowPlayerAlert(string message, float showForSeconds)
    {
        if (this.coroutinePlayerAlert != null)
        {
            StopCoroutine(this.coroutinePlayerAlert);
        }

        this.textPlayerAlert.text = message;
        this.textPlayerAlert.gameObject.SetActive(true);
        this.coroutineTimer = 0;
        this.coroutinePlayerAlert = StartCoroutine(this.CoroutineDisablePlayerAlert(showForSeconds));
    }

    public void ShowViewRadiusMarker(bool value, AIComponent ai = null)
    {
        if(this.viewRadiusMarker == null)
        {
            return;
        }

        this.viewRadiusMarker.gameObject.SetActive(value);

        if (value && ai != null)
        {
            this.viewRadiusMarker.transform.localScale = new Vector3(ai.ViewRange, ai.ViewRange, 1.0f);
            this.viewRadiusMarker.transform.parent = ai.transform;
            this.viewRadiusMarker.transform.localRotation = Quaternion.Euler(90, 0, 0);
            this.viewRadiusMarker.transform.localPosition = Vector3.zero;

            if (ai.IsHuman)
            {
                this.viewRadiusMarker.color = this.colorNpcRadius;
            }
            else
            {
                this.viewRadiusMarker.color = this.colorZombieRadius;
            }
        }
        else
        {
            this.viewRadiusMarker.transform.SetParent(null);
        }
    }

    public void ShowShootRadiusMarker(bool value, AIComponent ai = null)
    {
        if (this.shootRadiusMarker == null)
        {
            return;
        }

        this.shootRadiusMarker.gameObject.SetActive(value);

        if (value && ai != null)
        {
            this.shootRadiusMarker.transform.localScale = new Vector3(ai.GetAttackRange(), ai.GetAttackRange(), 1.0f);
            this.shootRadiusMarker.transform.parent = ai.transform;
            this.shootRadiusMarker.transform.localRotation = Quaternion.Euler(90, 0, 0);
            this.shootRadiusMarker.transform.localPosition = Vector3.zero;

            if (ai.IsHuman)
            {
                this.shootRadiusMarker.color = this.colorNpcRadius;
            }
            else
            {
                this.shootRadiusMarker.color = this.colorZombieRadius;
            }
        }
        else
        {
            this.shootRadiusMarker.transform.SetParent(null);
        }
    }

    public void OnClickButtonUpgradeToRunner()
    {
        if (this.zombieCount > 0)
        {
            this.ShowTextAtMouse(true, "Upgrade to Runner\nFlesh: 10");
            this.inputController.SwitchToUpgradeMode(InputController.UpgradeModes.Runner);

        }
    }

    public void OnClickButtonUpgradeToShooter()
    {
        if (this.zombieCount > 0)
        {
            this.ShowTextAtMouse(true, "Upgrade to Shooter\nFlesh: 15");
            this.inputController.SwitchToUpgradeMode(InputController.UpgradeModes.Shooter);
        }
    }

    public void OnClickButtonUpgradeToBomber()
    {
        if (this.zombieCount > 0)
        {
            this.ShowTextAtMouse(true, "Upgrade to Bomber\nFlesh: 5");
            this.inputController.SwitchToUpgradeMode(InputController.UpgradeModes.Bomber);
        }
    }

    public void OnClickButtonUpgradeToTrickster()
    {
        if (this.zombieCount > 0)
        {
            this.ShowTextAtMouse(true, "Upgrade to Trickster\nFlesh: 5");
            this.inputController.SwitchToUpgradeMode(InputController.UpgradeModes.Trickster);
        }
    }

    public bool SubtractPoints(int amount)
    {
        int newCount = this.pointsCount - amount;

        if(newCount >= 0)
        {
            this.pointsCount = newCount;
            this.textValuePoints.text = this.pointsCount.ToString();
            return true;
        }

        return false;
    }

    private void OnNpcSpawned(GameObject spawned, bool isMilitary)
    {
        AIComponent ai = spawned.GetComponent<AIComponent>();
        if (ai != null)
        {
            if (isMilitary)
            {
                this.militaryCount++;
                this.textValueMilitaryCount.text = this.militaryCount.ToString();
            }
            else
            {
                this.villagerCount++;
                this.textValueVillagerCount.text = this.villagerCount.ToString();
            }
        }
    }

    private void OnNpcInfected(GameObject infected, bool isMilitary)
    {
        AIComponent ai = infected.GetComponent<AIComponent>();
        if(ai != null)
        {
            if(isMilitary)
            {
                this.militaryCount--;
                this.textValueMilitaryCount.text = this.militaryCount.ToString();
            }
            else
            {
                this.villagerCount--;
                this.textValueVillagerCount.text = this.villagerCount.ToString();
            }

            this.zombieCount++;
            this.textValueZombieCount.text = this.zombieCount.ToString();

            this.pointsCount++;
            this.textValuePoints.text = this.pointsCount.ToString();

            if (this.villagerCount <= 0 && this.militaryCount <= 0)
            {
                //TODO: Win condition
            }
        }
        
    }

    private void OnZombieKilled(GameObject zombie)
    {
        this.zombieCount--;
        this.textValueZombieCount.text = this.zombieCount.ToString();

        if (this.zombieCount <= 0)
        {
            //TODO: lose condition
        }
    }

    private IEnumerator CoroutineDisablePlayerAlert(float secondsUntilDisable)
    {
        this.alertFadeOutRunning = false;
        while (this.coroutineTimer < secondsUntilDisable)
        {
            this.coroutineTimer += Time.deltaTime;

            if (!this.alertFadeOutRunning && this.coroutineTimer > secondsUntilDisable - 1)
            {
                this.alertFadeOutRunning = true;
                this.animatorPlayerAlert.SetTrigger("FadeOut");
            }

            yield return null;
        }

        this.textPlayerAlert.gameObject.SetActive(false);
    }
}
