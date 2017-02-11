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

    void OnValidate()
    {
        BezierSpline spline = GetComponent<BezierSpline>();
        Vector3 bezierPos = transform.TransformPoint(spline.GetControlPoint(spline.ControlPointCount - 2));
        Vector3 snapPos = transform.TransformPoint(spline.GetControlPoint(spline.ControlPointCount - 1));
        bezierPos = snapPos + (snapPos - bezierPos);
        for (int i = 0; i<PathEnd.Count; i++)
        {
            BezierSpline target = PathEnd[i].GetComponent<BezierSpline>();
            target.SetControlPoint(0, target.transform.InverseTransformPoint(snapPos));
            target.SetControlPoint(1, target.transform.InverseTransformPoint(bezierPos));
        }
    }
}
