using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGuiController : MonoBehaviour
{
    [SerializeField]
    private Text textValueNpcCount;
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

    private int npcCount = 0;
    private int zombieCount = 0;
    private int pointsCount = 0;

    private Coroutine coroutinePlayerAlert = null;
    private float coroutineTimer = 0;
    private bool alertFadeOutRunning = false;

    public int NpcCount { get { return this.npcCount; } }
    public int ZombieCount { get { return this.zombieCount; } }
    public int PointsCount { get { return this.pointsCount; } }


    public void Awake()
    {
        EventController.EventNpcSpawned += this.OnNpcSpawned;
        EventController.EventNpcInfected += this.OnNpcInfected;
        EventController.EventZombieKilled += this.OnZombieKilled;

        this.textValueNpcCount.text = "0";
        this.textValueZombieCount.text = "0";
        this.textValuePoints.text = "0";

        this.ShowPlayerAlert("Click on a citizen to spawn your first Zombie!", 4);

        this.ShowViewRadiusMarker(false);
        this.ShowShootRadiusMarker(false);
    }

    public void ShowPlayerAlert(string message, float showForSeconds)
    {
        if(this.coroutinePlayerAlert != null)
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
        if (this.viewRadiusMarker == null || this.viewRadiusMarker.gameObject == null)
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
    }

    public void ShowShootRadiusMarker(bool value, AIComponent ai = null)
    {
        if (this.shootRadiusMarker == null || this.shootRadiusMarker.gameObject == null)
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
    }
    
    private void OnNpcSpawned()
    {
        this.npcCount++;
        this.textValueNpcCount.text = this.npcCount.ToString();
    }

    private void OnNpcInfected()
    {
        this.npcCount--;
        this.textValueNpcCount.text = this.npcCount.ToString();

        this.zombieCount++;
        this.textValueZombieCount.text = this.zombieCount.ToString();

        this.pointsCount++;
        this.textValuePoints.text = this.pointsCount.ToString();

        if (this.npcCount <= 0)
        {
            //TODO: Win condition
        }
    }

    private void OnZombieKilled(GameObject zombie)
    {
        this.zombieCount--;
        this.textValueZombieCount.text = this.zombieCount.ToString();

        if(this.zombieCount <= 0)
        {
            //TODO: lose condition
        }
    }

    private IEnumerator CoroutineDisablePlayerAlert(float secondsUntilDisable)
    {
        this.alertFadeOutRunning = false;
        while(this.coroutineTimer < secondsUntilDisable)
        {
            this.coroutineTimer += Time.deltaTime;

            if(!this.alertFadeOutRunning && this.coroutineTimer > secondsUntilDisable - 1)
            {
                this.alertFadeOutRunning = true;
                this.animatorPlayerAlert.SetTrigger("FadeOut");
            }

            yield return null;
        }

        this.textPlayerAlert.gameObject.SetActive(false);
    }
}
