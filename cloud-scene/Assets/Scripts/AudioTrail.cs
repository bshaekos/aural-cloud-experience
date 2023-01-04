using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrail : MonoBehaviour
{
    private float counter;
    [SerializeField] private float expirationTime = 2f;
    private SpriteRenderer cloudColor;

    private float speed = 0.4f;

    float alphaLevel = 1f;
    float scaleChange = 1f;

    public ParticleSystem cloudBurst;

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

        if(counter > expirationTime)
        {
            alphaLevel -= Time.deltaTime * speed;
            scaleChange -= Time.deltaTime * speed;
            //Debug.Log(alphaLevel);
            cloudColor.color = new Color(1f,1f,1f, alphaLevel);
            cloudScale = new Vector3(scaleChange, scaleChange, scaleChange);
            transform.localScale = cloudScale;
            cloudBurst.Play();
            StartCoroutine(CloudDestroy());
            //Debug.Log(gameObject.name+ " expired");
        }
    }

    private IEnumerator CloudDestroy()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject,0);
    }
}
