using UnityEngine.Audio;
using UnityEngine;
using System;

public class Game_Over_2_AudioManager : MonoBehaviour
{
    [SerializeField] private Game_Over_2_Sound[] _sounds;
    [SerializeField] private Game_Over_2_Sound[] _feridSounds;

    public static Game_Over_2_AudioManager audioManInstance;

    private Game_Over_2_Ferid _ferid;

    private void Awake()
    {
        if (audioManInstance == null)
        {
            audioManInstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Game_Over_2_Sound s in _sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Game_Over_2_Sound s in _feridSounds)
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
        Play_Music("Music");
    }

    public void Ferid_Talking(string name)
    {
        _ferid = FindObjectOfType<Game_Over_2_Ferid>();
        if (!Game_Over_2_OptionPanel.sfxMuted)
        {
            Game_Over_2_Sound s = Array.Find(_feridSounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound " + name + " not Found!");
            }

            if (_ferid != null)
                _ferid.Talk(s.clip);
        }
    }

    public void Play_Sfx(string name)
    {
        Game_Over_2_Sound s = Array.Find(_sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not Found!");
        }
        s.source.Play();
    }

    public void Mute_Sfx(bool m)
    {
        foreach (Game_Over_2_Sound s in _sounds)
        {
            if (s.name != "Music")
            {
                s.source.mute = m;
            }
        }
        foreach (Game_Over_2_Sound sound in _feridSounds)
        {
            sound.source.mute = m;
        }
    }

    private void Play_Music(string name)
    {
        Game_Over_2_Sound s = Array.Find(_sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not Found!");
        }
        s.source.Play();
    }

    public void Mute_Music(bool m)
    {
        foreach (Game_Over_2_Sound s in _sounds)
        {
            if (s.name == "Music")
            {
                s.source.mute = m;
                break;
            }
        }
    }
}

[Serializable]
public class Game_Over_2_Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 1f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

[Serializable]
public struct Toggles
{
    public GameObject onSwitch;
    public GameObject offSwitch;
}