using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAudioSourceOnFinishPlay : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    public void Update()
    {
        if (!this.source.isPlaying)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
