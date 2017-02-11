using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BezierSpline))]
public class Path : MonoBehaviour
{
    public List<Path> PathEnd = new List<Path>();
    [System.NonSerialized]
    public Factory FactoryEnd;

    [System.NonSerialized]
    public List<Path> PathStart = new List<Path>();
    [System.NonSerialized]
    public Factory FactoryStart;

    [System.NonSerialized]
    public List<Tank> Tanks = new List<Tank>();

    void Start()
    {
        for (int i = 0; i<PathEnd.Count; i++)
            PathEnd[i].PathStart.Add(this);
    }
}
