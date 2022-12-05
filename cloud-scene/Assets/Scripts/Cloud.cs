using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cloud : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Node node;

    [Header("Cloud Move In-Place")]
    [SerializeField] private float speed;
    [SerializeField] private float moveRange;
    private bool cloudMovedOut;
    private Vector3 target;
    private Vector3 unitSphere;

    //[SerializeField] private float collapseSpeed;

    [Header("Cloud Animate In-Out Setup")]
    [SerializeField]private Vector3 finalPos;
    [SerializeField] private Vector3 finalScale;
    [SerializeField] private Vector3 initialPos;
    [SerializeField] private Vector3 initialScale;


    SpriteRenderer cloudSpriteRenderer;
    Color noCloudColor = new Color(255, 255, 255, 0);
    Color fullCloudColor = new Color(255, 255, 255, 1f);


    void Start()
    {
        unitSphere = gameObject.transform.position;

        cloudSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        cloudMovedOut = false;

        transform.localPosition = initialPos;
        transform.localScale = initialScale;
        //cloudSpriteRenderer.color = noCloudColor;
    }

    void Update()
    {
        // //collapse towards node over a 24-hour period
        // unitSphere = Vector3.MoveTowards(unitSphere, node.transform.position, collapseSpeed * Time.deltaTime);
        
        //billboard towards player gameobject
        transform.LookAt(transform.position + player.transform.rotation * Vector3.forward, player.transform.rotation * Vector3.up);
        Vector3 eulerAngles = transform.eulerAngles;
        transform.eulerAngles = eulerAngles;

        //expand and contract the cloud cluster based on proximity to the node
        if (node.DistanceFromPlayer() <= node.playerDistTrig)
        {       
            if (cloudMovedOut == false)
            {
                LeanTween.moveLocal(gameObject, finalPos, 1f).setEaseInOutQuad();
                LeanTween.scale(gameObject, finalScale, 1f).setEaseInOutQuad();
                //cloudSpriteRenderer.color = Color.Lerp(noCloudColor, fullCloudColor, 1f);
                cloudMovedOut = true;
                
            }
            if (cloudMovedOut == true)
            {
                //randomly move around 
                target = ChooseTarget();

                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if(transform.position == target)
                {
                    target = ChooseTarget();
                }

            }
        }

        if(node.DistanceFromPlayer() > node.playerDistTrig)
        {
            if(cloudMovedOut == true)
            {

                LeanTween.moveLocal(gameObject, node.transform.localPosition, 1f).setEaseInOutQuad();
                LeanTween.scale(gameObject, initialScale, 1f).setEaseInOutQuad();
                //cloudSpriteRenderer.color = Color.Lerp(fullCloudColor, noCloudColor, 1f);
                cloudMovedOut = false;
            }
        }
        
    }

    private Vector3 ChooseTarget()
    {
        Vector3 newGoal = Random.insideUnitSphere * moveRange + unitSphere;
        return newGoal;
    }

    // private float DetermineCollapseSpeed()
    // {
    //     float distance = Vector3.Distance(node.transform.position, transform.position);
    //     collapseSpeed = distance / 86400;
    //     return collapseSpeed;
    // }
}
