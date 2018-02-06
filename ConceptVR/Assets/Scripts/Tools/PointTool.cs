﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTool : Tool {
    Vector3 grabPos;
    Vector3 startPos;
    Point grabbedPoint;
    float maxDist = 0.07f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
        if (triggerInput && grabbedPoint != null)
            grabbedPoint.setPosition(startPos + controllerPosition - grabPos);
	}

    public override bool TriggerDown()
    {
        grabbedPoint = DCGBase.NearestPoint(controllerPosition, maxDist);
        if (grabbedPoint != null)
        {
            grabPos = controllerPosition;
            startPos = grabbedPoint.position;
            return true;
        }
        return false;
    }

    public override bool TriggerUp()
    {
        if (grabbedPoint != null)
        {
            grabbedPoint = null;
            return true;
        }
        return false;
    }

    public override bool Tap(Vector3 position)
    {
        new Point(position);
        return true;
    }
}
