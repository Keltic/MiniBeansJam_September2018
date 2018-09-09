
using System.Collections.Generic;
using UnityEngine;

public class MilitiaController : MonoBehaviour {

    public List<AIComponent> AllAIActors;

    public float ZombiePercentageTrigger = 0.25f;

    public float ReactionTimer = 2f;

    public GameObject MilitaPrefab;

    public SpawnController spawnController;

    private float updateTimer;

    public float MaxMilitias = 150;
    private float militiasSpawned = 0;

	void Start ()
    {
        AllAIActors = new List<AIComponent>();
        UpdateAiActors();
        spawnController = GetComponent<SpawnController>();    
        EventController.EventNpcSpawned += this.UpdateAiActors;
        EventController.EventZombieKilled += this.ZombieKilled;
    }

    private void ZombieKilled(GameObject zombie)
    {
        UpdateAiActors();
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

    void UpdateAiActors()
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
