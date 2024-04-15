using System;
using UnityEngine;
using UnityEngine.Audio;



[System.Serializable]
public class Trainee_Horizon_FeridSpaceShip_Sound
{

    [Header("Sound Settings")]
    public AudioClip clip;

    public string name;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;


    [HideInInspector]
    public AudioSource source;
    

}
