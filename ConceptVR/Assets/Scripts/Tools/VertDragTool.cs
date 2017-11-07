using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertDragTool : Tool {
    public float grabDistance = .1f;
    Point nearestPoint;
    Vector3 pointGrabPos;
    Vector3 conGrabPos;
    DCGBase dcgObject;

	// Use this for initialization
	void Start () {
        dcgObject = GameObject.Find("DCG").GetComponent<DCGBase>();
	}
	
	// Update is called once per frame
	void Update () {

        if (triggerInput)
        {
            if (nearestPoint != null)
            {
                nearestPoint.setPosition(pointGrabPos + (transform.position - conGrabPos));
            }
        }
        else
        {
            float nDist2 = grabDistance * grabDistance;
            nearestPoint = null;
            foreach (Point p in DCGBase.points)
            {
                float dist2 = (transform.position - p.position).sqrMagnitude;
                if (dist2 < nDist2)
                {
                    nearestPoint = p;
                    nDist2 = dist2;
                }
            }
        }
	}

    public override void TriggerDown()
    {
        if (nearestPoint != null)
            pointGrabPos = nearestPoint.position;
        conGrabPos = transform.position;
        return;
    }

    public override void TriggerUp()
    {
        return;
    }

    public void OnRenderObject()
    {
        if (nearestPoint != null)
        {
            dcgObject.pointMat.SetPass(0);
            Graphics.DrawMeshNow(dcgObject.pointMesh, Matrix4x4.TRS(nearestPoint.position, Quaternion.identity, new Vector3(.02f, .02f, .02f)));
            Graphics.DrawMeshNow(dcgObject.pointMesh, Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(.02f, .02f, .02f)));
        }
    }
    public override void onGrip()
    {
        return;
    }
}
