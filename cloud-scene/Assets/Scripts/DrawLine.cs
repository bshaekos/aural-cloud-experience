using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    [SerializeField] private Transform[] nodes;
    [SerializeField] private LineController line;

    void Start()
    {
        line.SetUpLine(nodes);
    }
}
