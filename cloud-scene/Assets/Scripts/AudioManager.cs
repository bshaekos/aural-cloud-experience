using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip beacon;
    [SerializeField] private AudioClip onPlay;
    [SerializeField] private AudioClip onStop;

    //[HideInInspector]
    public AudioSource track01;
    //[HideInInspector]
    public AudioSource uiStopSource;

    //[HideInInspector]
    public AudioSource uiPlaySource;

    //[HideInInspector]
    public AudioSource track02;
    private bool isPlayingBeacon;
    private float currentBeaconVolume;

    private Sound beaconClip;
   
    void Awake()
    {
        
        //track01 = gameObject.GetComponent<AudioSource>();
        //track01 = gameObject.AddComponent<AudioSource>();

        //track02 = gameObject.GetComponent<AudioSource>();
        //track02 = gameObject.AddComponent<AudioSource>();

        //uiStopSource = gameObject.GetComponent<AudioSource>();
        //uiStopSource = gameObject.AddComponent<AudioSource>();

        //uiPlaySource = gameObject.GetComponent<AudioSource>();
        //uiStopSource = gameObject.AddComponent<AudioSource>();
        
    
        // track01.enabled = true;
        // track01.volume = 1.0f;
        // track02.enabled = true;
        // track02.volume = 1.0f;
        // uiStopSource.enabled = true;
    }
    void Start()
    {
        
        
        //set ui stop audio clip settings
        Sound uiStopClip = new Sound("UIStopClip", onStop);
        uiStopClip.volume = 0.2f;
        uiStopClip.spatialBlend = 0;
        SetUIStopSound(uiStopClip);


        //set ui stop audio clip settings
        Sound uiPlayClip = new Sound("UIPlayClip", onPlay);
        uiPlayClip.volume = 0.2f;
        uiPlayClip.spatialBlend = 0;
        SetUIPlaySound(uiPlayClip);

    
        //set beacon audio clip settings
        beaconClip = new Sound("BeaconClip", beacon);
        beaconClip.clip = beacon;
        beaconClip.loop = true;
        beaconClip.spatialBlend = 1;
        beaconClip.dopplerLevel = 0;
        beaconClip.minDistance = 0.1f;
        beaconClip.maxDistance = 15f;
        beaconClip.volume = 1f;
        SetBeaconSound(beaconClip);
    

        //initialize beacon sound
        isPlayingBeacon = true;
        SwapAudio(beaconClip);

    }


      public void SwapAudio(Sound newSoundClip)
    {
       StopAllCoroutines();
       StartCoroutine(FadeAudio(newSoundClip));
    }

     private IEnumerator FadeAudio(Sound newSoundClip)
    {
        float timeToFade = .75f;
        float timeElapsed = 0;

       if (isPlayingBeacon)
        {
            track02.clip = newSoundClip.clip;
            track02.loop = newSoundClip.loop;
            track02.spatialBlend = newSoundClip.spatialBlend;
            track02.dopplerLevel = newSoundClip.dopplerLevel;
            track02.maxDistance = newSoundClip.maxDistance;
            track02.minDistance = newSoundClip.minDistance; 
            track02.Play();
            
            
            while(timeElapsed < timeToFade)
            {
                track02.volume = Mathf.Lerp(0, newSoundClip.volume, timeElapsed/timeToFade);
                track01.volume = Mathf.Lerp(track01.volume, 0, timeElapsed/timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            track01.Stop();
        }
        else
        {
            track01.clip = newSoundClip.clip;
            track01.loop = newSoundClip.loop;
            track01.spatialBlend = newSoundClip.spatialBlend;
            track01.dopplerLevel = newSoundClip.dopplerLevel;
            track01.maxDistance = newSoundClip.maxDistance;
            track01.minDistance = newSoundClip.minDistance;
            track01.Play();
            

            while(timeElapsed < timeToFade)
            {
                track01.volume = Mathf.Lerp(0, newSoundClip.volume, timeElapsed/timeToFade);
                track02.volume = Mathf.Lerp(track02.volume, 0, timeElapsed/timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track02.Stop();
        }
        isPlayingBeacon = !isPlayingBeacon;
    }

    public void ReturnToDefault()
    {
        SwapAudio(beaconClip);
    }

    private void SetBeaconSound(Sound sound)
    {
        track01.clip = sound.clip;
        track01.loop = sound.loop;
        track01.spatialBlend = sound.spatialBlend;
        track01.dopplerLevel = sound.dopplerLevel;
        track01.minDistance = sound.minDistance;
        track01.maxDistance = sound.maxDistance;
        track01.volume = sound.volume;
    }

    private void SetUIStopSound(Sound sound)
    {
        uiStopSource.clip = sound.clip;
        uiStopSource.volume = sound.volume;
    }

    private void SetUIPlaySound(Sound sound)
    {
        uiPlaySource.clip = sound.clip;
        uiPlaySource.volume = sound.volume;
    }
}
