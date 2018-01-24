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
            //If we are lacking a hand set everything back to it's start
            if (Hands.Left == null || Hands.Right == null)
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
                freeFormLine.positionCount += 2;
                freeFormLine.SetPosition(freeFormLine.positionCount-2, rightPos);
                freeFormLine.SetPosition(++freeFormLine.positionCount-1, leftPos);
                rightPoints.Add(rightPos);
                leftPoints.Add(leftPos);
            }
            else frameCount++;
        }
	}
    public override void FreeForm()
    {
        //Fetch the initial position, which is the midpoint between the two palms
        startPos = (Hands.Left.PalmPosition.ToVector3() + Hands.Right.PalmPosition.ToVector3()) / 2;
        
        //Initialize the new Line renderer
        GameObject go = new GameObject();
        freeFormLine = go.AddComponent<LineRenderer>();
        freeFormLine.startWidth = .01f;
        freeFormLine.endWidth = .01f;
        //Set all vars with the new pos
        freeFormLine.positionCount = 1;
        freeFormLine.SetPosition(0, startPos);
        rightPoints.Add(startPos);
        leftPoints.Add(startPos);
    }
    public override void FreeFormEnd()
    {
        endPos = (Hands.Left.PalmPosition.ToVector3() + Hands.Right.PalmPosition.ToVector3()) / 2;
        freeFormLine.SetPosition(++freeFormLine.positionCount, endPos);
        rightPoints.Add(endPos);
        leftPoints.Add(endPos);
    }
}
