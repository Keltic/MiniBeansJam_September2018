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

        this.textValueNpcCount.text = "0";
        this.textValueZombieCount.text = "0";
        this.textValuePoints.text = "0";

        this.ShowPlayerAlert("Click on a citizen to spawn your first Zombie!", 4);
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

    private void OnZombieKilled()
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
