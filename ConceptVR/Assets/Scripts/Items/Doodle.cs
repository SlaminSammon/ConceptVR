﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doodle : Item {

    LineRenderer lr;
    Color oldColor;
	// Use this for initialization
	void Start () {
        lr = this.gameObject.GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override Vector3 Position(Vector3 contPos)
    {
        lr = this.gameObject.GetComponent<LineRenderer>();
        int lowestPos = 0;
        float lowDist = Mathf.Abs(Vector3.Distance(lr.GetPosition(0), contPos));
        for (int i = 1; i < lr.positionCount; ++i)
        {
            float newDist = Mathf.Abs(Vector3.Distance(lr.GetPosition(i), contPos));
            if (newDist < lowDist)
            {
                lowestPos = i;
                lowDist = newDist;
            }
        }
        return lr.GetPosition(lowestPos);
    }
    public override void Select()
    {
        oldColor = lr.material.color;
        lr.material.color = Color.white;
    }
    public override void DeSelect()
    {
        lr.material.color = oldColor;
    }

}