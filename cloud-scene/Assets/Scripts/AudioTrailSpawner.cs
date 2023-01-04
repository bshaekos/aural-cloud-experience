using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrailSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> audioBreadcrumbs;

    public PlayerMovement player;
    [SerializeField] private float spawnTime = 0.1f;
    private int indexNum;
    private float counter;

    // public Node node1;
    // public Node node2;

    void Start()
    {
        indexNum = 0;
        counter = 0;
    }

    void Update()
    {
        // once player starts moving, new audio breadcrumbs are created
        // if (node1.audioNoteTrig | node2.audioNoteTrig == false)
        // {
        // }
        if(player.move != Vector3.zero)
        {
            //Debug.Log("I'm moving");
            if (counter >= spawnTime)
            {
                CreateAudioBreadcrumb();
                counter = 0;
            }
        }
        // if(player.move == Vector3.zero)
        // {
        //     indexNum = 0;
        // }
        counter += Time.deltaTime;
        //Debug.Log(counter);

    }

    void CreateAudioBreadcrumb()
    {
        if(indexNum < audioBreadcrumbs.Count)
        {
            //Debug.Log("I'm spawning " + indexNum);
            Instantiate(audioBreadcrumbs[indexNum], transform.position, transform.rotation);
            indexNum++;
            //Debug.Log("New "+ indexNum);
        }
        else
        {
            indexNum = 0;
            //Debug.Log("Back to " + indexNum);
        }

    }
}
