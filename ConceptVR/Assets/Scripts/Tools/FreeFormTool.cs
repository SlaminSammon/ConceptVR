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
    private LineRenderer freeFormLine;
    public bool ended;

	// Use this for initialization
	new void Start () {
		rightPoints = new List<Vector3> ();
		leftPoints = new List<Vector3> ();
        util = new HandsUtil();
        ended = false;
	}
	
	// Update is called once per frame
	new void Update () {
        //If the LTC sends in an ended signal, end the line and create the object(S.I.P.)
        if (ended)
        {
            endPos = (Hands.Left.PalmPosition.ToVector3() + Hands.Right.PalmPosition.ToVector3()) / 2;
            freeFormLine.SetPosition(++freeFormLine.positionCount, endPos);
            rightPoints.Add(endPos);
            leftPoints.Add(endPos);
        }
        if(rightPoints.Count == 0 && leftPoints.Count == 0)
        {
            //Fetch the initial position, which is the midpoint between the two palms
            startPos = (Hands.Left.PalmPosition.ToVector3() + Hands.Right.PalmPosition.ToVector3()) / 2;
            //Initialize the new Line renderer
            GameObject go = new GameObject();
            freeFormLine = go.AddComponent<LineRenderer>();
            freeFormLine.startWidth = .05f;
            freeFormLine.endWidth = .05f;
            //Set all vars with the new pos
            freeFormLine.positionCount = 1;
            freeFormLine.SetPosition(1,startPos);
            rightPoints.Add(startPos);
            leftPoints.Add(startPos);
        }
        //If we are lacking a hand set everything back to it's start
        if(Hands.Left == null || Hands.Right == null)
        {
            rightPoints = new List<Vector3>();
            leftPoints = new List<Vector3>();
            return;
        }
        if (frameCount == 10)
        {
            //Add points to the line renderer and the point lists
            Vector3 rightPos = Hands.Right.PalmPosition.ToVector3();
            Vector3 leftPos = Hands.Left.PalmPosition.ToVector3();
            freeFormLine.SetPosition(++freeFormLine.positionCount, rightPos);
            freeFormLine.SetPosition(++freeFormLine.positionCount, leftPos);
            rightPoints.Add(rightPos);
            leftPoints.Add(leftPos);
        }
        else frameCount++;
	}
}
