using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;


public class Trainee_Horizon_FeridSpaceShip_SoundManager : MonoBehaviour
{
    public Trainee_Horizon_FeridSpaceShip_Sound[] sounds;

    public static Trainee_Horizon_FeridSpaceShip_SoundManager instance;
    void Awake()
    {
        instance = this;

        foreach (Trainee_Horizon_FeridSpaceShip_Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip= s.clip;
            s.source.volume= s.volume;
            s.source.pitch= s.pitch;
            s.source.loop= s.loop;
        }
    }

    private void Start()
    {
        play(Trainee_Horizon_FeridSpaceShip_Constants.AMBIENT_SOUNDS);
        play(Trainee_Horizon_FeridSpaceShip_Constants.BG_M);
    }

    public void play(string name)
    {
        Trainee_Horizon_FeridSpaceShip_Sound s = Array.Find(sounds, sound => sound.name==name);
        if (s == null)
        {
            return;
        }
        s.source.Play();
    }

    public void playOneShot(string name)
    {
        Trainee_Horizon_FeridSpaceShip_Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.PlayOneShot(s.clip);
    }

}
