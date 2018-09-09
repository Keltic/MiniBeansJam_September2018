
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
    const float AttackRangeMeele = 1.5f;
    const float AttackRangeExploder = 1.5f;
    const float AttackRangeRanged = 5f;

    const float SpeedAttacking = 6f;
    const float SpeedIdle = 3.5f;
    const float SpeedFleeing = 5.0f;

    public enum AIState
    {
        Idle = 0,
        Walking,
        Fleeing,
        Attacking,
        Dead
    }

    public enum WeaponTypes
    {
        Meele,
        Exploder,
        Ranged,
        Runner,
    }


    public AIState CurrentAIState;

    public WeaponTypes WeaponType;

    public bool IsHuman = true;

    // -1 = Panic
    // 0 = Passive 
    // 1 = Aggressive
    public int AgressionValue = 0;

    public float ActionTimer = 0.1f;

    // Range to look for random points to walk to
    public float WalkLookAtRange = 50.0f;

    // Range to look for enemies
    public float ViewRange = 25.0f;

    public GameObject Target = null;

    public Vector3 WalkTarget;
    
    private float CurrentActionTimer = 0.0f;

    public NavMeshAgent Agent;
    public SpriteRenderer Renderer;
    public MilitiaController MilitaController;

    public RuntimeAnimatorController AnimControllerBasic;
    public RuntimeAnimatorController AnimControllerBomber;
    public RuntimeAnimatorController AnimControllerRunner;

    // Use this for initialization
    void Start () {
        Agent.autoBraking = false;
        Renderer = GetComponentInChildren<SpriteRenderer>();
        MilitaController = Camera.allCameras[0].GetComponent<MilitiaController>();
    }
    
    public float GetAttackRange()
    {
        switch (WeaponType)
        {
            case WeaponTypes.Exploder:
                return AttackRangeExploder;
            case WeaponTypes.Meele:
                return AttackRangeMeele;
            case WeaponTypes.Ranged:
                return AttackRangeRanged;
        }
        return -1f;
    }
    

	// Update is called once per frame
	void Update ()
    {
        Debug.DrawLine(transform.position, WalkTarget, Color.red);

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

    GameObject GetClosestEnemy()
    {
        float viewDist = IsHuman ? ViewRange : ViewRange * 1.2f;
        return MilitaController.GetClosestActor(transform.position, viewDist, !IsHuman);
    }

    public void ChangeWeaponType(WeaponTypes newType)
    {
        WeaponType = newType;
        Animator animator = this.GetComponent<Animator>();
        
        switch (newType)
        {
            case WeaponTypes.Exploder:
                animator.runtimeAnimatorController = AnimControllerBomber;
                break;
            case WeaponTypes.Meele:
                animator.runtimeAnimatorController = AnimControllerBasic;
                break;
            case WeaponTypes.Runner:
                animator.runtimeAnimatorController = AnimControllerRunner;
                break;
        }
    }

    public void Infect()
    {
        IsHuman = false;
        AgressionValue = 1;
        CurrentAIState = AIState.Idle;
        WeaponType = WeaponTypes.Meele;
        EventController.ReportNpcInfected();
        Renderer.color = Color.red;
    }

    /// <summary>
    /// Idle is the go-to state after most actions
    /// </summary>
    void ProcessIdle()
    {
        Agent.speed = SpeedIdle;
        // Passive
        if (AgressionValue == 0)
        {
            // First, check if there are zombies around and who's the closest
            GameObject closestZombie = GetClosestEnemy();

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
            GameObject closestZombie = GetClosestEnemy();

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
            GameObject closestVictim = GetClosestEnemy();

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
        // if we are passive, always check for zombies while
        // walking
        if (AgressionValue == 0)
        {
            // First, check if there are zombies around and who's the closest
            GameObject closestZombie = GetClosestEnemy();

            if (closestZombie != null)
            {
                // PANIC !
                CurrentAIState = AIState.Fleeing;
                AgressionValue = -1;
                Target = closestZombie;
                return;
            }
        }
        // if we are agressive, always check for humans while
        // walking
        else if (AgressionValue == 1)
        {
            // Check if there is a target to attack
            GameObject closestVictim = GetClosestEnemy();

            if (closestVictim != null)
            {
                CurrentAIState = AIState.Attacking;
                Target = closestVictim;
                return;
            }
            // Walk somewhere
        }

        if (Agent.remainingDistance < 0.5f)
        {
            CurrentAIState = AIState.Idle;
        }        
    }


    void ProcessAttacking()
    {
        Agent.speed = SpeedAttacking;

        // Check if our target unit is still alive 
        if (Target == null)
        {
            CurrentAIState = AIState.Idle;
            return;
        }

        float dist = Vector3.Distance(transform.position, Target.transform.position);

        AIComponent targetComp = Target.GetComponent<AIComponent>();

        // Check if our target is still valid
        if (targetComp == null || targetComp.IsHuman == IsHuman)
        {
            CurrentAIState = AIState.Idle;
            return;
        }

        switch (WeaponType)
        {
            case WeaponTypes.Meele:
                if (dist < AttackRangeMeele)
                {
                    if (!IsHuman)
                    {
                        targetComp.Infect();
                    }
                    else
                    {
                        targetComp.CurrentAIState = AIState.Dead;
                        EventController.ReportZombieKilled(Target);
                    }
                    Target = null;
                    CurrentAIState = AIState.Idle;
                    return;
                }
                break;
            case WeaponTypes.Exploder:
                if (dist < AttackRangeExploder)
                {
                    if (!IsHuman)
                    {
                        targetComp.Infect();
                    }
                    else
                    {
                        targetComp.CurrentAIState = AIState.Dead;
                        EventController.ReportZombieKilled(Target);
                    }
                    Target = null;
                    CurrentAIState = AIState.Idle;
                    return;
                }
                break;
            case WeaponTypes.Ranged:
                if (dist < AttackRangeRanged)
                {
                    if (!IsHuman)
                    {
                        targetComp.Infect();
                    }
                    else
                    {
                        targetComp.CurrentAIState = AIState.Dead;
                        EventController.ReportZombieKilled(Target);
                    }
                    Target = null;
                    CurrentAIState = AIState.Idle;
                    return;
                }
                break;
        }
        // Not close enough, chase target
        WalkToTargetPoint(Target.transform.position);
    }

    void ProcessFleeing()
    {
        Agent.speed = SpeedFleeing;
        if (Target == null)
        {
            CurrentAIState = AIState.Idle;
            return;
        }
        Vector3 dir = Target.transform.position - transform.position;
        Vector3 targetPoint = (transform.position - ((dir * 2.5f)) + (Random.insideUnitSphere * 2.5f));
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
            Agent.CalculatePath(WalkTarget, path);

            Agent.SetPath(path);
        }

        // If there is no point on the navmesh to reach the target, this is probably bad..
    }

    void WalkToRandomMapPoint()
    {   
        Vector3 randomPos = transform.position;

        float dist = Vector3.Distance(randomPos, transform.position);

        while (dist < WalkLookAtRange * 0.5f)
        {            
            randomPos = transform.position + (Random.insideUnitSphere * WalkLookAtRange);
            dist = Vector3.Distance(randomPos, transform.position);
        }

        WalkToTargetPoint(randomPos);
    }
}
