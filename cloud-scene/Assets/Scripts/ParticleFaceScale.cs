using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFaceScale : MonoBehaviour
{
    private float scaleMultiplier;
    
    Vector3 initialScale;
    [SerializeField] float randomValMin;
    [SerializeField] float randomValMax;
    float randomVal;

    public Particle particle;

    
    void Awake()
    {
        initialScale = transform.localScale;
    }
    
    void Start()
    {
        randomVal = Random.Range(randomValMin, randomValMax);
    }

    void Update()
    {
        ParticlePieceScale();
    }

    // particle pieces scale randomly
    public void ParticlePieceScale()
    {
        float playerDist = particle.DistanceFromPlayer();
        if (playerDist <= particle.scaleDist)
        {
            scaleMultiplier = 6f;
            transform.localScale = (initialScale * randomVal / playerDist) * scaleMultiplier;
        }

        if (playerDist > particle.scaleDist)
        {
            scaleMultiplier = 4f;
            transform.localScale = (initialScale / playerDist) * scaleMultiplier;
        }
        
    }

    // slowly inflate the object
    // private void Inflate(float scaleToValue, Transform faceScale)
    // {
    //     float currentScale = faceScale.transform.localScale.y;
    //     if (currentScale < scaleToValue)
    //     {
    //         int difference = int(scaleToValue - currentScale);
    //     }
    // }
}
