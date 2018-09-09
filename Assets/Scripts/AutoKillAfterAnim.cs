using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKillAfterAnim : MonoBehaviour {


    float timer = 0.0f;
    float maxLength;
    // Use this for initialization
    void Start () {
        Animator anim = GetComponent<Animator>();
        maxLength = anim.runtimeAnimatorController.animationClips[0].length;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > maxLength)
        {
            Destroy(this);
        }
        else
        {
            timer += Time.deltaTime;
        }
	}
}
