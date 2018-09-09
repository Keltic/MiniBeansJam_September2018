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
    private GameObject npcPrefab;

    float leftBorder;
    float rightBorder;
    float topBorder;
    float bottomBorder;

    public void Start()
    {
        leftBorder = (levelWidth / 2) - 2;
        rightBorder = leftBorder * -1;
        topBorder = (levelWidth / 2) - 2;
        bottomBorder = topBorder * -1;

        int count = PlayerPrefs.HasKey("VillagerCount") ? PlayerPrefs.GetInt("VillagerCount") : 200;

        for (int i = 0; i < count; i++)
        {
            SpawnNPC(npcPrefab);
        }
    }

    public void SpawnNPC(GameObject prefab)
    {
        GameObject temp;
        temp = GameObject.Instantiate(prefab);
        Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(rightBorder, leftBorder), 0.0f, UnityEngine.Random.Range(bottomBorder, topBorder));
        NavMeshHit hit;
        int walkMask = 1 << NavMesh.GetAreaFromName("Walkable");
        if (NavMesh.SamplePosition(spawnPos, out hit, 250.0f, walkMask))
        {
            temp.transform.position = hit.position;
            EventController.ReportNpcSpawned(temp, temp.GetComponent<AIComponent>().WeaponType == AIComponent.WeaponTypes.Ranged);
        }
        else
        {
            DestroyImmediate(temp);
        }
    }

}
