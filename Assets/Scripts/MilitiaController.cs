
using System;
using System.Collections.Generic;
using UnityEngine;

public class MilitiaController : MonoBehaviour {

    public List<AIComponent> AllAIActors;

    public float ZombiePercentageTrigger = 0.25f;

    public float ReactionTimer = 2f;

    public GameObject MilitaPrefab;

    public SpawnController spawnController;

    public GameObject ExplosionPrefab;
    public GameObject MilitaryProjectile;
    public GameObject ZombieProjectile;

    public GameObject BloodEffect;

    private float updateTimer;

    public float MaxMilitias = 150;
    private float militiasSpawned = 0;

	void Start ()
    {
        AllAIActors = new List<AIComponent>();
        UpdateAiActors(null, false);
        spawnController = GetComponent<SpawnController>();    
        EventController.EventNpcSpawned += this.UpdateAiActors;
        EventController.EventZombieKilled += this.ZombieKilled;
        EventController.EventNpcInfected += this.WasInfected;
    }

    private void WasInfected(GameObject infected, bool isMilitary)
    {
        if(isMilitary)
        {
            militiasSpawned--;
        }
    }

    private void ZombieKilled(GameObject zombie)
    {
        UpdateAiActors(null, false);
    }

    public List<GameObject> GetAllActorsInRange(Vector3 position, float range, bool findHuman)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (AIComponent others in AllAIActors)
        {
            float dist = Vector3.Distance(position, others.transform.position);

            if (others.IsHuman == findHuman && dist < range)
            {
                result.Add(others.gameObject);
            }
        }

        return result;
    }
    public GameObject GetClosestActor(Vector3 position, float viewRange, bool findHuman)
    {
        GameObject closestEntity = null;
        float closestEntityDist = float.MaxValue;
        foreach (AIComponent others in AllAIActors)
        {
            float dist = Vector3.Distance(position, others.transform.position);
            float adjustedViewRange = viewRange;
            if (!findHuman && others.WeaponType == AIComponent.WeaponTypes.Trickster)
            {
                adjustedViewRange *= 0.5f;
            }
            if (others.IsHuman == findHuman && dist < viewRange && dist < closestEntityDist)
            {
                closestEntity = others.gameObject;
                closestEntityDist = dist;
            }
        }
        return closestEntity;
    }

    void UpdateAiActors(GameObject spawned, bool isMilitary)
    {
        AllAIActors.Clear();
        AllAIActors.AddRange(FindObjectsOfType<AIComponent>());
    }

    float GetZombiePercentage()
    {
        float zombieCount = AllAIActors.FindAll(actor => !actor.IsHuman).Count;
        return zombieCount / AllAIActors.Count;
    }

    void SpawnMilitia()
    {
        if (MilitaPrefab)
        {
            if(this.militiasSpawned == 0)
            {
                GameObject.Find("Canvas_Game").GetComponent<GameGuiController>().ShowPlayerAlert("The military has arrived! Watch out!", 4.0f);
            }

            spawnController.SpawnNPC(MilitaPrefab);
            militiasSpawned++;
        }
    }

    // Update is called once per frame
    void Update () {
		if (updateTimer <= 0.0f)
        {
            updateTimer = ReactionTimer;
            if (militiasSpawned < MaxMilitias && GetZombiePercentage() > ZombiePercentageTrigger)
            {
                SpawnMilitia();
            }
        }
        else
        {
            updateTimer -= Time.deltaTime;
        }
	}
}
