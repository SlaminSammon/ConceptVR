using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class FreeFormTool : Tool {
	private Vector3 startPos;
	private Vector3 endPos;
	private List<Vector3> rightPoints;
	private List<Vector3> leftPoints;
    private HandsUtil util;
    private int frameCount = 0;
    private LineRenderer rightFreeFormLine;
    private LineRenderer freeFormLine;

    // Use this for initialization
    new void Start () {
		rightPoints = new List<Vector3> ();
		leftPoints = new List<Vector3> ();
        util = new HandsUtil();
	}
	
	// Update is called once per frame
	new void Update () {
        if (formInput)
        {

            if (frameCount >= 700)
            {
                Debug.Log(frameCount);
                //Add points to the line renderer and the point lists
                Vector3 rightPos = Hands.Right.PalmPosition.ToVector3();
                Vector3 leftPos = Hands.Left.PalmPosition.ToVector3();
                if (leftPos == null || rightPos == null)
                    return;
                if (rightPos == Vector3.zero || leftPos == Vector3.zero)
                    return;
                freeFormLine.positionCount++;
                freeFormLine.SetPosition(freeFormLine.positionCount-1, leftPos);
                rightFreeFormLine.positionCount++;
                rightFreeFormLine.SetPosition(rightFreeFormLine.positionCount-1, rightPos);
                rightPoints.Add(rightPos);
                leftPoints.Add(leftPos);
            }
            else frameCount++;
        }
	}
    public override void FreeForm()
    {
        Debug.Log("Starting the ultimate freeform!!");
        Vector3 rightPos = Hands.Right.PalmPosition.ToVector3();
        Vector3 leftPos = Hands.Left.PalmPosition.ToVector3();
        //Initialize the new Line renderer
        GameObject go = new GameObject();
        go.transform.position = rightPos;
        rightFreeFormLine = go.AddComponent<LineRenderer>();
        rightFreeFormLine.startWidth = .01f;
        rightFreeFormLine.endWidth = .01f;
        GameObject gol = new GameObject();
        gol.transform.position = leftPos;
        freeFormLine = gol.AddComponent<LineRenderer>();
        freeFormLine.startWidth = .01f;
        freeFormLine.endWidth = .01f;
        //Set all vars with the new pos
        freeFormLine.positionCount++;
        Debug.Log("RightPos " + rightPos);
        Debug.Log("LeftPos " + leftPos);
        rightFreeFormLine.SetPosition(0, rightPos);
        freeFormLine.SetPosition(0, leftPos);
        rightPoints.Add(rightPos);
        leftPoints.Add(leftPos);
    }
    public override void FreeFormEnd()
    {
        rightPoints.Add(Hands.Right.PalmPosition.ToVector3());
        leftPoints.Add(Hands.Left.PalmPosition.ToVector3());
        List<Point> rPoints = new List<Point>();
        List<Point> lPoints = new List<Point>();
        foreach (Vector3 v in rightPoints)
            rPoints.Add(new Point(v));
        foreach (Vector3 v in leftPoints)
            lPoints.Add(new Point(v));
        List<Edge> edges = new List<Edge>();
        edges.Add(new Edge(lPoints[0], rPoints[rPoints.Count - 1]));
        edges.Add(new Edge(rPoints[0], lPoints[lPoints.Count - 1]));
        for (int p = 0; p < rPoints.Count - 1; ++p)
            edges.Add(new Edge(rPoints[p], rPoints[p + 1]));
        for (int p = 0; p < lPoints.Count - 1; ++p)
            edges.Add(new Edge(lPoints[p], lPoints[p + 1]));
        new Face(edges);
        Debug.Log("Endign FreeForm");
    }
}
