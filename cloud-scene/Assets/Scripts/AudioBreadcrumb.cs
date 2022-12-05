using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBreadcrumb : MonoBehaviour
{
    private float counter;
    [SerializeField] private float expirationTime = 20f;

    [SerializeField] AudioSource audioClip;
    void Start()
    {
        counter = 0;
    }

    void Update()
    {
        counter += Time.deltaTime;
        //Debug.Log(gameObject.name + counter);

        if(counter > expirationTime)
        {
            Destroy(gameObject,0);
            //Debug.Log(gameObject.name+ " expired");
        }
    }

    void OnTriggerEnter(Collider player) 
    {
        audioClip.Play();
    }

    void OnTriggerExit(Collider player) 
    {
        audioClip.Stop();
    }
}
