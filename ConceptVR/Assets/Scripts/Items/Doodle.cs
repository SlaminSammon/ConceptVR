using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Doodle : Item {
    public Bounds boundingBox;
    LineRenderer lr;
    int numClicks = 0;
    [SyncVar]
    Color oldColor;
    //[SyncVar (hook = "Encapsulate")]
    //public Vector3 latestPoint;
    [SyncVar(hook = "finalBounds")]
    public bool isFinished;
    [SyncVar(hook = "changeWidth")]
    public float lineWidth;
	// Use this for initialization
	void Start () {
        lr = this.gameObject.GetComponent<LineRenderer>();
        isFinished = false;
        base.Start();
        oldColor = Color.red;
    }
	
	// Update is called once per frame
	void Update () {
        if (this.gameObject.GetComponent<NetworkIdentity>().isServer)
            Debug.Log("Butts");
	}

    public override Vector3 Position(Vector3 contPos)
    {
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

    public void erasePoint(Vector3 p)
    {
        Vector3[] verts = new Vector3[lr.positionCount];
        lr.GetPositions(verts);

        int index = -1;
        for (int i = 0; i < lr.positionCount; ++i)
        {
            if (verts[i] == p)
            {
                index = i;
                break;
            }
        }
        if (index == -1)
            Debug.LogError("Attempted to erase non-existent point " + p);
        else if (index <= 1)    //front end
        {
            for (int i = index; i < lr.positionCount - index - 1; ++i)
                verts[i] = verts[i + index + 1];
            lr.positionCount -= (index + 1);
        }
        else if (index >= lr.positionCount - 1) //back end
        {
            lr.positionCount = index;
        }
        else //somewhere in the middle
        {
            List<Vector3> newDoodPositions = new List<Vector3>();
            for (int i = index; i < lr.positionCount - 1; ++i)
            {
                newDoodPositions.Add(verts[i]);
            }
            lr.positionCount = index - 1;

            //TODO: create new doodle and set its points to newDoodPositions
        }
    }

    public void setPoints(List<Vector3> points)
    {
        //TODO: Set all points at once
    }

    public override void CmdSelect()
    {
        isLocked = true;
        isSelected = true;
    }
    public override void CmdDeSelect()
    {
        isLocked = false;
        isSelected = false;
    }
    public void Encapsulate(Vector3 pos)
    {
        boundingBox.Encapsulate(pos);
    }
    public void finalBounds(bool boolean)
    {
        boundingBox.SetMinMax(boundingBox.min - new Vector3(.1f, .1f, .1f), boundingBox.max + new Vector3(.1f, .1f, .1f));
    }
    public override float Distance(Vector3 pos)
    {
        if (boundingBox.Contains(pos))
            return Vector3.Distance(pos, this.Position(pos));
        return 100000f;
    }
    public void changeWidth(float width)
    {
        lr.startWidth = width;
        lr.endWidth = width;
    }
    [Command]
    public void CmdUpdateLineRenderer(Vector3 pos)
    {
        if (lr == null) return;
        lr.positionCount = numClicks + 1;
        lr.SetPosition(numClicks, pos);
        Encapsulate(pos);
        numClicks++;
    }
    public override void SelectUtil()
    {
        if (!isSelected)
        {
            oldColor = lr.material.color;
            lr.material.color = Color.white;
        }
        else
            lr.material.color = oldColor;
    }
}
