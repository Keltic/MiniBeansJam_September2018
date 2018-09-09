using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public enum SfxTypes
    {
        ButtonClick,
        Brains1,
        Brains2,
        Moan1,
        Moan2,
        Moan3,
        Shot,
        NpcInfected,
        FreshMeat
    }

    [SerializeField]
    private AudioSource sourceBgm;
    [SerializeField]
    private AudioSource prefabSourceSfx;
    [SerializeField]
    private AudioClip[] clips;

    public void Start()
    {
        if (this.sourceBgm != null && !this.sourceBgm.isPlaying)
        {
            GameObject.DontDestroyOnLoad(this.sourceBgm.gameObject);
            this.sourceBgm.Play();
        }
    }

    public void PlaySfx(SfxTypes type, float volume)
    {
        AudioSource source = GameObject.Instantiate(this.prefabSourceSfx);

        switch (type)
        {
            case SfxTypes.ButtonClick:
                source.clip = this.clips[0];
                break;
            case SfxTypes.Brains1:
                source.clip = this.clips[1];
                break;
            case SfxTypes.Brains2:
                source.clip = this.clips[2];
                break;
            case SfxTypes.Moan1:
                source.clip = this.clips[3];
                break;
            case SfxTypes.Moan2:
                source.clip = this.clips[4];
                break;
            case SfxTypes.Moan3:
                source.clip = this.clips[5];
                break;
            case SfxTypes.Shot:
                source.clip = this.clips[6];
                break;
            case SfxTypes.NpcInfected:
                source.clip = this.clips[7];
                break;
            case SfxTypes.FreshMeat:
                source.clip = this.clips[8];
                break;
        }

        source.volume = volume;
        source.Play();
    }

    public void PlayButtonClickSound()
    {
        this.PlaySfx(SfxTypes.ButtonClick, 1.0f);
    }

    public void PlayRandomMoan()
    {
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                this.PlaySfx(SfxTypes.Moan1, 1.0f);
                break;
            case 1:
                this.PlaySfx(SfxTypes.Moan2, 1.0f);
                break;
            case 2:
                this.PlaySfx(SfxTypes.Moan3, 1.0f);
                break;
        }
    }
}
