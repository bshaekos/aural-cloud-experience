using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    LineRenderer lineRenderer;
    Transform[] nodes;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetUpLine(Transform[] nodes)
    {
        lineRenderer.positionCount = nodes.Length;
        this.nodes = nodes;
    }
    void LateUpdate()
    {
        for( int i = 0; i < nodes.Length; i++)
        {
            lineRenderer.SetPosition(i, nodes[i].position);
        }
    }
}
