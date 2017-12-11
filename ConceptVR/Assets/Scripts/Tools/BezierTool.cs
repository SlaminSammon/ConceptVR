using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierTool : Tool
{
    List<Point> currentPoints;
    Edge currentEdge;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (currentPoints != null)
        {
            currentPoints[currentPoints.Count - 1].position = controllerPosition;
            currentEdge.updateMesh();
        }
    }

    public override void TriggerDown()
    {
        if (currentPoints == null)
        {
            currentPoints = new List<Point>();
            currentPoints.Add(new Point(controllerPosition));
            currentPoints.Add(new Point(controllerPosition));
            currentEdge = new Edge(currentPoints[0], currentPoints[1]);
            currentEdge.setSmooth(true);
        } else
        {
            currentPoints = null;
            currentEdge = null;
        }
    }

    /*public override void TriggerUp()
    {
        currentPoints = null;
        currentEdge = null;
    }*/

    /*public override void GripDown()
    {
        Point newPoint = new Point(controllerPosition);
        newPoint.edges.Add(currentEdge);
        currentPoints.Add(newPoint);
        currentEdge.points.Add(newPoint);
    }*/

    public override void Tap(Vector3 position)
    {
        if (currentPoints != null)
        {
            Point newPoint = new Point(controllerPosition);
            newPoint.edges.Add(currentEdge);
            currentPoints.Add(newPoint);
            currentEdge.points.Add(newPoint);
        }
    }

    public override void Swipe()
    {
        
    }
}
