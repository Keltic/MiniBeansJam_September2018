using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProjectile : MonoBehaviour {

    public Vector3 Target;
    public AIComponent TargetComponent;
    public float ProjectileSpeed = 250f;
    public bool IsHumanProjectile;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ( Vector3.Distance(transform.position, Target) < 0.2f)
        {
            if (TargetComponent != null && TargetComponent.gameObject != null)
            {
                TargetComponent.HitFromProjectile(IsHumanProjectile);
            }
            DestroyImmediate(gameObject);
        }
        else
        {
            float step = Time.deltaTime * ProjectileSpeed;
            transform.position =  Vector3.MoveTowards(transform.position, Target, step);
        }
	}
}
