using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneAudioController : MonoBehaviour
{
    [SerializeField] private GameObject[] sceneCloudClusters;
    public Transform player;

    private int indexNum;

    void Start()
    {
        indexNum = 0;
    }

    void Update()
    {
        sceneCloudClusters = GameObject.FindGameObjectsWithTag("CloudCluster");
        sceneCloudClusters = sceneCloudClusters.OrderBy(cloudCluster => (player.transform.position - cloudCluster.transform.position).sqrMagnitude).ToArray(); //.First().transform;


        for (int i = 0; i < sceneCloudClusters.Length; i++)
        {
            if(indexNum > sceneCloudClusters.Length)
            {
                indexNum = 0;
                Debug.Log("Index Number = " + indexNum);
                //break;
            }
            if(indexNum <= i)
            {
                AudioManager cloudClusterAudioManager = sceneCloudClusters[i].GetComponentInChildren<AudioManager>();
                //cloudClusterAudioManager.beaconSound.volume = 1.0f / i+1;
                //Debug.Log(cloudClusterAudioManager.name + " , " +  cloudClusterAudioManager.beaconSound.volume);
                indexNum++;
                Debug.Log(indexNum);

                //break;
            }
        } 
        Debug.Log("outside for loop");


            
            // AudioManager closestCloudCluster = sceneCloudClusters[i].GetComponentInChildren<AudioManager>();
            // AudioManager nextClosestCloudCluster = sceneCloudClusters[i+1].GetComponentInChildren<AudioManager>();
            
            // closestCloudCluster.beaconSound.volume = 1.0f;
            // nextClosestCloudCluster.beaconSound.volume = 0.5f;
            // Debug.Log(nextClosestCloudCluster.name + " , " +  nextClosestCloudCluster.beaconSound.volume);

            // if (i - (i+1) <= 0)
            // {
            //     break;
            // }
        
    }
}
