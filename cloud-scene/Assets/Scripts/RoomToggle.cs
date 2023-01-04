using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomToggle : MonoBehaviour
{
    [SerializeField] private GameObject[] stratusAudioManagers; 

    public GameObject stratusScene;

    public bool stratusPlay;
    void Start()
    {
        //audioManagers = AudioManager.FindGameObjectsWithTag("AudioManager-Stratus");
        //var audioManagers = Resources.FindObjectsOfTypeAll<GameObject>().Where(audioManager => audioManager.name == "AudioManager-Stratus");

        stratusPlay = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            stratusScene.SetActive(true);
            Debug.Log("key pressed");
            stratusPlay = true;
        
            // for(int i = 0; i < stratusAudioManagers.Length; i++)
            // {
            //     stratusAudioManagers[i].SetActive(true);
            //     AudioSource beacon = stratusAudioManagers[i].GetComponentInChildren<AudioSource>();
            //     beacon.enabled = true;
            //     beacon.volume = 1f;
            // }

        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {

        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
    }
}
