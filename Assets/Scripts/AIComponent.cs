using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIComponent : MonoBehaviour {

    /*
    AIs:
        Aggression -1: Walk (fast) in other direction away from Target
        Aggression 0: Idle, WalkRandom, if Zombie approaches, decrease aggresion
        Aggression 1: WalkRandom, if Target in range, attack
    */

    const int NavhMeshWalkLayer = 0;

    // Attack ranges for different weapon types. Maybe move somewhere else
    const float AttackRangeMeele = 2.5f;
    const float AttackRangeExploder = 3f;
    const float AttackRangeRanged = 15f;

    const float SpeedAttacking = 7.5f;
    const float SpeedIdle = 3.5f;
    const float SpeedFleeing = 5.0f;

    public enum AIState
    {
        Idle = 0,
        Walking,
        Fleeing,
        Attacking
    }

    public enum WeaponTypes
    {
        Meele,
        Exploder,
        Ranged,
    }


    public AIState CurrentAIState;

    public WeaponTypes WeaponType;

    public bool IsHuman = true;

    // -1 = Panic
    // 0 = Passive 
    // 1 = Aggressive
    public int AgressionValue = 0;

    public float ActionTimer = 1.0f;

    // Range to look for random points to walk to
    public float WalkLookAtRange = 50.0f;

    // Range to look for enemies
    public float ViewRange = 25.0f;

    private List<AIComponent> otherActors;

    public GameObject Target = null;

    public Vector3 WalkTarget;

    public float AttackRange = 0.2f;

    private float CurrentActionTimer = 0.0f;

    private NavMeshAgent agent;

    public Color HumanColor;
    public Color ZombieColor;


	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        otherActors = new List<AIComponent>();
        UpdateAiActors();
        UpdateMaterial();
	}

    void UpdateMaterial()
    {
        Material mat = GetComponent<MeshRenderer>().materials[0];
        mat.color = IsHuman ? HumanColor : ZombieColor;
    }
	
    void UpdateAiActors()
    {
        otherActors.AddRange(FindObjectsOfType<AIComponent>());
        // ignore self
        otherActors.RemoveAll(actor => actor.gameObject == this.gameObject);
    }

	// Update is called once per frame
	void Update () {
		if (CurrentActionTimer <= 0.0f)
        {
            CurrentActionTimer = ActionTimer;

            switch (CurrentAIState)
            {
                case AIState.Idle:
                    ProcessIdle();
                    break;
                case AIState.Walking:
                    ProcessWalking();
                    break;
                case AIState.Attacking:
                    ProcessAttacking();
                    break;
                case AIState.Fleeing:
                    ProcessFleeing();
                    break;
            }
        }
        else
        {
            CurrentActionTimer -= Time.deltaTime;
        }
	}

    GameObject GetClosestEntityOfType(bool human)
    {
        GameObject closestEntity = null;
        float closestEntityDist = float.MaxValue;
        foreach (AIComponent others in otherActors)
        {
            float dist = Vector3.Distance(transform.position, others.transform.position);
            if (others.IsHuman == human && dist < ViewRange && dist < closestEntityDist)
            {
                closestEntity = others.gameObject;
                closestEntityDist = dist;
            }
        }
        return closestEntity;
    }

    public void Infect()
    {
        IsHuman = false;
        UpdateMaterial();
        AgressionValue = 1;
        CurrentAIState = AIState.Idle;
    }
    /// <summary>
    /// Idle is the go-to state after most actions
    /// </summary>
    void ProcessIdle()
    {
        agent.speed = SpeedIdle;
        // Passive
        if (AgressionValue == 0)
        {
            // First, check if there are zombies around and who's the closest
            GameObject closestZombie = GetClosestEntityOfType(false);

            if (closestZombie != null)
            {
                // PANIC !
                CurrentAIState = AIState.Fleeing;
                AgressionValue = -1;
                Target = closestZombie;
                return;
            }
        }
        // Panic
        else if (AgressionValue == -1)
        {
             // Check if there is still a zombie close
            GameObject closestZombie = GetClosestEntityOfType(false);

            if (closestZombie != null)
            {
                // Run again
                CurrentAIState = AIState.Fleeing;
                Target = closestZombie;
                return;
            }
            else
            {
                AgressionValue = 0;
                // Fall through and maybe walk somewhere
            }
        }
        // Agressive
        else if (AgressionValue == 1)
        {
            // Check if there is a target to attack
            GameObject closestVictim = GetClosestEntityOfType(true);

            if (closestVictim != null)
            {
                CurrentAIState = AIState.Attacking;
                Target = closestVictim;
                return;
            }
            // Walk somewhere
        }

        // No danger, just randomly walk somewhere, maybe.
        if (Random.Range(0f, 1f) > 0.75f)
        {
            WalkToRandomMapPoint();
        }
    }

    void ProcessWalking()
    {
        if (agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            CurrentAIState = AIState.Idle;
        }
    }


    void ProcessAttacking()
    {
        agent.speed = SpeedAttacking;
        float dist = Vector3.Distance(transform.position, Target.transform.position);
        float walkRangeToTarget = 1.0f;
        AIComponent targetComp = Target.GetComponent<AIComponent>();

        // Check if our target is still valid
        if (targetComp.IsHuman == IsHuman)
        {
            CurrentAIState = AIState.Idle;
            return;
        }

        switch (WeaponType)
        {
            case WeaponTypes.Meele:
                if (dist < AttackRangeMeele)
                {
                    // ATTACK !
                    if (!IsHuman)
                    {
                        targetComp.Infect();
                    }
                    CurrentAIState = AIState.Idle;
                    return;
                }
                walkRangeToTarget = AttackRangeMeele;
                break;
            case WeaponTypes.Exploder:
                if (dist < AttackRangeExploder)
                {
                    // ATTACK !
                    if (!IsHuman)
                    {
                        targetComp.Infect();
                    }
                    CurrentAIState = AIState.Idle;
                    return;
                }
                walkRangeToTarget = AttackRangeExploder;
                break;
            case WeaponTypes.Ranged:
                if (dist < AttackRangeRanged)
                {
                    // ATTACK !
                    if (!IsHuman)
                    {
                        targetComp.Infect();
                    }
                    CurrentAIState = AIState.Idle;
                    return;
                }
                walkRangeToTarget = AttackRangeRanged;
                break;
        }
        // Not close enough, chase target
        WalkToTargetPoint(Target.transform.position);
    }

    void ProcessFleeing()
    {
        agent.speed = SpeedFleeing;
        Vector3 dir = Target.transform.position - transform.position;
        Vector3 targetPoint = transform.position - (dir * 5f);

        WalkToTargetPoint(targetPoint);
    }

    void WalkToTargetPoint(Vector3 targetPoint, float distance = 250.0f)
    {
        NavMeshHit hitResult;
        int walkMask = 1 << NavMesh.GetAreaFromName("Walkable");
        if (NavMesh.SamplePosition(targetPoint, out hitResult, distance, walkMask))
        {
            WalkTarget = hitResult.position;
            CurrentAIState = AIState.Walking;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(WalkTarget, path);        

            agent.SetPath(path);
        }

        // If there is no point on the navmesh to reach the target, this is probably bad..
    }

    void WalkToRandomMapPoint()
    {
        Vector3 randomPos = transform.position + (Random.insideUnitSphere * WalkLookAtRange);
        WalkToTargetPoint(randomPos);
    }
}
