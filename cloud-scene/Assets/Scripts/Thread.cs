using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thread : MonoBehaviour
{
    [SerializeField] private List<Cloud> allResponses;
    private List<LineController> threadLines;
    [SerializeField] private LineController linePrefab;

    
    void Awake()
    {
        //instantiate a line to connect threads
        threadLines = new List<LineController>();
        for (int i = 0; i < allResponses.Count; i++)
        {
            LineController newLine = Instantiate(linePrefab);
            threadLines.Add(newLine);

            newLine.AssignTarget(transform.position, allResponses[i].transform);
            newLine.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        //sets each line to active
        foreach (var line in threadLines)
        {
            line.gameObject.SetActive(true);
        }
    }
}
