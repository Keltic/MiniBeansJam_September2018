using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private float levelWidth;
    [SerializeField]
    private float levelHeight;
    [SerializeField]
    private int npcStartCount;
    [SerializeField]
    private GameObject npcPrefab;

    public void Start()
    {
        GameObject temp;
        float leftBorder = (levelWidth / 2) - 2;
        float rightBorder = leftBorder * -1;
        float topBorder = (levelWidth / 2) - 2;
        float bottomBorder = topBorder * -1;

        for (int i = 0; i < this.npcStartCount; i++)
        {
            temp = GameObject.Instantiate(this.npcPrefab);
            temp.transform.position = new Vector3(UnityEngine.Random.Range(rightBorder, leftBorder), 0.0f, UnityEngine.Random.Range(bottomBorder, topBorder));
        }
    }

}
