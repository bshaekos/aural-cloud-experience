using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public float volume;
    public bool loop;
    public int spatialBlend;
    public float dopplerLevel;
    public float minDistance;
    public float maxDistance;

    public Sound(string _name, AudioClip _clip)
    {
        name = _name;
        clip = _clip;
    }
}

