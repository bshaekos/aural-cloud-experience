using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Node : MonoBehaviour
{
    public Transform player;
    public float playerDistTrig = 2f;
    [SerializeField] private List<Cloud> allClouds;
    
    private List<LineController> cloudLines;

    public ParticleSystem ripple;
    [SerializeField] private GameObject audioUIPlaying;

    [SerializeField] private LineController linePrefab;

    [SerializeField] private AudioClip audioNote;

    [SerializeField] private AudioMixerGroup audioNoteAudioMixerGroup;
    [SerializeField] private Transform cloudCluster;
    public bool audioNoteTrig; 

    public AudioManager audioManager;

    //public GameObject audioManagerPrefab;

    private Sound audioNoteClip;

    void Awake()
    {   
        //instantiate a line to connect node and cloud form(s)
        cloudLines = new List<LineController>();
        for (int i = 0; i < allClouds.Count; i++)
        {
            LineController newLine = Instantiate(linePrefab);
            cloudLines.Add(newLine);

            newLine.AssignTarget(transform.position, allClouds[i].transform);
            newLine.gameObject.SetActive(false);
        }
    }

    void Start()
    { 
        //audiomanager  gameobject created in the scene
        //var obj = Instantiate(audioManagerPrefab, transform.position, transform.rotation);
        //audioManager = obj.GetComponent<AudioManager>();
        //audioManager.transform.SetParent(cloudCluster);
        
        //ui icon for when audio note is playing
        audioUIPlaying.SetActive(false);
        audioUIPlaying.transform.localPosition = new Vector3(0, 0, 0);
        audioUIPlaying.transform.localScale = new Vector3(0, 0, 0);  

        //set audio note clip settings
        audioNoteClip = new Sound("AudioNoteClip", audioNote);
        audioNoteClip.clip = audioNote;
        audioNoteClip.loop = false;
        audioNoteClip.volume = 1f;
        audioNoteClip.audioMixerGroup = audioNoteAudioMixerGroup;

        audioNoteTrig = false;
    }

    void Update()
    {
        
        //sets each line to active
        foreach (var line in cloudLines)
        {
            line.gameObject.SetActive(true);
        }

        //billboard towards player gameobject
        transform.LookAt(transform.position + player.transform.rotation * Vector3.forward, player.transform.rotation * Vector3.up);
        Vector3 eulerAngles = transform.eulerAngles;
        transform.eulerAngles = eulerAngles;
    }

    void OnTriggerEnter(Collider player) 
    {
        //StartCoroutine(PlayAudioNotePause());
        audioManager.uiPlaySource.Play();
        audioManager.SwapAudio(audioNoteClip);
        audioNoteTrig = true;

        if (ripple.isPlaying)
        {
            ripple.Stop();
        }
        
        audioUIPlaying.SetActive(true);
        LeanTween.moveLocal(audioUIPlaying, new Vector3(0, -0.25f, 0), 1f).setEaseInOutQuad();
        LeanTween.scale(audioUIPlaying, new Vector3(0.05f, 0.05f, 0.05f), 1f).setEaseInOutQuad();
    }

    void OnTriggerExit(Collider player)
    {
        audioManager.uiStopSource.Play();
        StartCoroutine(ReturntoBeaconPause());
        audioNoteTrig = false;
        

        LeanTween.moveLocal(audioUIPlaying, new Vector3(0, 0f, 0), 1f).setEaseInOutQuad();
        LeanTween.scale(audioUIPlaying, new Vector3(0, 0, 0), 1f).setEaseInOutQuad();
        StartCoroutine(UIPlayingInactivate()); //also starts the ripple particle system
    }

    public float DistanceFromPlayer()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        return distance;
    }

    private IEnumerator UIPlayingInactivate()
    {
        yield return new WaitForSeconds(1f);
        audioUIPlaying.SetActive(false);

        if(ripple.isStopped)
            {
                ripple.Play();
            }
    }

    private IEnumerator ReturntoBeaconPause()
    {
        yield return new WaitForSeconds(0.5f);
        audioManager.ReturnToDefault();
    }

    private IEnumerator PlayAudioNotePause()
    {
        yield return new WaitForSeconds(1f);
        audioManager.SwapAudio(audioNoteClip);
    }

}
