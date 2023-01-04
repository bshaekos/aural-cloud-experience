using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBreadcrumb : MonoBehaviour
{
    private float counter;
    [SerializeField] private float expirationTime = 17f;

    [SerializeField] AudioSource audioClip;

    public ParticleSystem cloudBurst;

    private SpriteRenderer cloudColor;

    [SerializeField] private float cloudChangeSpeed = 0.4f;

    float alphaLevel = 1f;
    float scaleChange = 1f;
    Vector3 cloudScale;

    void Start()
    {
        counter = 0;
        cloudColor = GetComponent<SpriteRenderer>();
        cloudScale = transform.localScale;

    }

    void Update()
    {
        counter += Time.deltaTime;
        //Debug.Log(gameObject.name + counter);

        alphaLevel -= Time.deltaTime * cloudChangeSpeed;
        scaleChange -= Time.deltaTime * cloudChangeSpeed;
        //Debug.Log(alphaLevel);
        cloudColor.color = new Color(1f,1f,1f, alphaLevel);
        cloudScale = new Vector3(scaleChange, scaleChange, scaleChange);
        transform.localScale = cloudScale;
        cloudBurst.Play();

        if(counter > expirationTime)
        {
            Destroy(gameObject,0);
            //StartCoroutine(CloudDestroy());
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

    // private IEnumerator CloudDestroy()
    // {
    //     yield return new WaitForSeconds(2.5f);
    //     Destroy(gameObject,0);
    // }
}
