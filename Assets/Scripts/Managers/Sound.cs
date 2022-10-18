using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound 
{
    public enum AudioTypes { sfx, music };
    public AudioTypes audioType;

    public string name;
    public AudioClip clip;
    public bool loop;

    [Range(0f, 2f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;

    [HideInInspector]
    public AudioSource source;
}
