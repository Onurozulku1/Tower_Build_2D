using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private float basePitch;
    private float currentPitch;

    private Sound blockSound;

    public static AudioManager instance;
    private void Awake()
    {
        instance = this;
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        PlaySound("music");

        blockSound = Array.Find(sounds, sound => sound.name == "block");
        basePitch = blockSound.source.pitch;
        currentPitch = basePitch;

    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    private void SetBlockPitch()
    {

        if (BlockManager.instance.perfectBlock)
        {
            currentPitch += 0.1f;
        }
        else
        {
            currentPitch = basePitch;
        }
        blockSound.source.pitch = currentPitch;
        PlaySound("block");

    }

    public void ToggleAudio(int on_off)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * on_off;
        }
    }

    private void OnEnable()
    {
        GameController.NewBlock += SetBlockPitch;
    }

    private void OnDisable()
    {
        GameController.NewBlock -= SetBlockPitch;
    }
}