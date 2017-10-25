using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDragTool : Tool {
    public float grabDistance = .1f;
    Edge nearestEdge;
    List<Vector3> pointGrabPos;
    Vector3 conGrabPos;
    DCGBase dcgObject;

    // Use this for initialization
    void Start()
    {
        dcgObject = GameObject.Find("DCG").GetComponent<DCGBase>();
    }

    // Update is called once per frame
    void Update()
    {

        if (triggerInput)
        {
            if (nearestEdge != null)
            {
                for (int i = 0; i < nearestEdge.points.Count; ++i)
                { 
                    nearestEdge.points[i].setPosition(pointGrabPos[i] + (transform.position - conGrabPos));
                }
            }
        }
        else
        {
            float nDist2 = grabDistance * grabDistance;
            nearestEdge = null;
            foreach (Edge e in DCGBase.edges)
            {
                for (int i = 0; i < (e.isLoop ? e.points.Count : e.points.Count-1); ++i)
                { 
                    Vector3 ediff = (e.points[i].position - e.points[(i+1) % e.points.Count].position);
                    Vector3 hdiff = (transform.position - e.points[i].position);
                    Vector3 proj = Vector3.Project(hdiff, ediff);
                    float dist2 = (hdiff - proj).sqrMagnitude;
                    if (dist2 < nDist2 && hdiff.sqrMagnitude < ediff.sqrMagnitude && (transform.position - e.points[(i + 1) % e.points.Count].position).sqrMagnitude < ediff.sqrMagnitude)
                    {
                        nearestEdge = e;
                        nDist2 = dist2;
                    }
                }
            }
        }
    }

    public override void TriggerDown()
    {
        if (nearestEdge != null)
        {
            pointGrabPos = new List<Vector3>();
            foreach (Point p in nearestEdge.points)
                pointGrabPos.Add(p.position);
        }
        conGrabPos = transform.position;
        return;
    }

    public override void TriggerUp()
    {
        return;
    }

    public void OnRenderObject()
    {
        if (nearestEdge != null)
        {
            dcgObject.edgeMat.SetPass(0);

       
            Vector3 edgeVec = new Vector3();
            for (int i = 0; i < nearestEdge.points.Count - 1; ++i)
            {
                edgeVec = nearestEdge.points[i].position - nearestEdge.points[i + 1].position;
                Graphics.DrawMeshNow(dcgObject.edgeMesh, Matrix4x4.TRS(nearestEdge.points[i].position - edgeVec / 2, Quaternion.FromToRotation(Vector3.up, edgeVec), new Vector3(.01f, edgeVec.magnitude / 2, .01f)));
            }
            if (nearestEdge.isLoop)
            {
                edgeVec = nearestEdge.points[nearestEdge.points.Count - 1].position - nearestEdge.points[0].position;
                Graphics.DrawMeshNow(dcgObject.edgeMesh, Matrix4x4.TRS(nearestEdge.points[0].position + edgeVec / 2, Quaternion.FromToRotation(Vector3.up, edgeVec), new Vector3(.01f, edgeVec.magnitude / 2, .01f)));
            }
            Graphics.DrawMeshNow(dcgObject.pointMesh, Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(.02f, .02f, .02f)));
        }
    }
}
