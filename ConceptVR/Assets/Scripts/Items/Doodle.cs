using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Doodle : Item {
    Bounds boundingBox;
    LineRenderer lr;
    Color oldColor;
    [SyncVar (hook = "Encapsulate")]
    public Vector3 latestPoint;
    [SyncVar(hook = "finalBounds")]
    public bool isFinished;
	// Use this for initialization
	void Start () {
        lr = this.gameObject.GetComponent<LineRenderer>();
        boundingBox = new Bounds();
        isFinished = false;
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
    public override void CmdSelect()
    {
        oldColor = lr.material.color;
        lr.material.color = Color.white;
        isLocked = true;
        isSelected = true;
    }
    public override void CmdDeSelect()
    {
        lr.material.color = oldColor;
        isLocked = false;
        isSelected = false;
    }
    public void Encapsulate()
    {
        boundingBox.Encapsulate(latestPoint);
    }
    public void finalBounds()
    {
        boundingBox.SetMinMax(boundingBox.min - new Vector3(.1f, .1f, .1f), boundingBox.max + new Vector3(.1f, .1f, .1f));
    }
    public override float Distance(Vector3 pos)
    {
        if (boundingBox.Contains(pos))
            return Vector3.Distance(pos, this.Position(pos));
        return 100000f;
    }
    [Command]
    public void CmdChangeWidth(float width)
    {
        lr.startWidth = width;
        lr.endWidth = width;
    }
}
