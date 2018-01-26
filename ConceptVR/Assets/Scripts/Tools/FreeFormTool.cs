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
            //If we are lacking a hand set everything back to it's start
            if (Hands.Left == null || Hands.Right == null)
            {
                formInput = false;
            }
            if (frameCount == 100)
            {
                //Add points to the line renderer and the point lists
                Vector3 rightPos = Hands.Right.PalmPosition.ToVector3();
                Vector3 leftPos = Hands.Left.PalmPosition.ToVector3();
                if (leftPos == null || rightPos == null)
                    return;
                if (rightPos == new Vector3(0, 0, 0) || leftPos == new Vector3(0, 0, 0))
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
        endPos = (Hands.Left.PalmPosition.ToVector3() + Hands.Right.PalmPosition.ToVector3()) / 2;
        freeFormLine.SetPosition(++freeFormLine.positionCount, endPos);
        rightPoints.Add(endPos);
        leftPoints.Add(endPos);
        for(int i = 0; i < freeFormLine.positionCount; ++i)
        {
            if (freeFormLine.GetPosition(i) == new Vector3(0, 0, 0))
                Debug.Log("It's here: " + i);
        }
        for (int i = 0; i < rightFreeFormLine.positionCount; ++i)
        {
            if (rightFreeFormLine.GetPosition(i) == new Vector3(0, 0, 0))
                Debug.Log("It's here: " + i);
        }
    }
}
