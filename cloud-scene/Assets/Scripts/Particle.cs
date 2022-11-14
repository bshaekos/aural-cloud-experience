using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Particle : MonoBehaviour
{
    public TextMeshProUGUI dateTime;
    public Transform player;

    public Flock flock;

    public float scaleDist = 2f;

    public float degreesPerSecond;
    public float amplitude;
    public float frequency;

    Vector3 tempPos;
    Vector3 posOffset;

    Vector3 initialPosition;

    void Awake()
    {
        initialPosition = transform.localPosition;
    }

    void Start()
    {
        
    }

    void Update()
    {
        // particle floats up and down, rotates according to a sin wave
        transform.Rotate(new Vector3(Time.deltaTime * degreesPerSecond, Time.deltaTime * degreesPerSecond, 0f));
        transform.position = particleFloat(frequency, amplitude);

        //turn date-time text off
        float playerdistance = DistanceFromPlayer();
        if(playerdistance <= scaleDist)
        {
            dateTime.enabled = false;

        }
        else 
        {
            dateTime.enabled = true;
           
        }

    }

    public Vector3 particleFloat(float frequency, float amplitude)
    {
        tempPos = transform.position;
        tempPos.y = initialPosition.y + Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        return tempPos;
    }

    public float DistanceFromPlayer()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        return distance;
    }
}
