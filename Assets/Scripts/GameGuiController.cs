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

    private int npcCount = 0;
    private int zombieCount = 0;
    private int pointsCount = 0;

    public int NpcCount { get { return this.npcCount; } }
    public int ZombieCount { get { return this.zombieCount; } }
    public int PointsCount { get { return this.pointsCount; } }

    private void Awake()
    {
        EventController.EventNpcSpawned += this.OnNpcSpawned;
        EventController.EventNpcInfected += this.OnNpcInfected;

        this.textValueNpcCount.text = "0";
        this.textValueZombieCount.text = "0";
        this.textValuePoints.text = "0";
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
}
