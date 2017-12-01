using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTool : SelectTool {
    public Material linkMat;

	// Use this for initialization
	new void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	new void Update () {
		
	}

    public override void TriggerDown()
    {
        if (sPoints.Count == 2)
        {
            new Edge(sPoints[0], sPoints[1]);
        }
        else if (sPoints.Count >= 3)
        {
            List<Edge> newEdges = new List<Edge>();
            for(int i = 0; i < sPoints.Count; ++i)
                newEdges.Add(new Edge(sPoints[i], sPoints[(i+1) % sPoints.Count]));
            new Face(newEdges);
        }

        ClearSelection();
    }

    new void OnRenderObject()
    {
        base.OnRenderObject();

        linkMat.SetPass(0);

        Vector3 edgeVec;
        if (sPoints.Count > 2)
            for (int i = 0; i < sPoints.Count - 1; ++i)
            {
                edgeVec = sPoints[i].position - sPoints[i + 1].position;
                Graphics.DrawMeshNow(GeometryUtil.cylinder8, Matrix4x4.TRS(sPoints[i].position - edgeVec / 2, Quaternion.FromToRotation(Vector3.up, edgeVec), new Vector3(.005f, edgeVec.magnitude / 2, .005f)));
            }
        if (sPoints.Count > 1)
        {
            edgeVec = sPoints[sPoints.Count - 1].position - sPoints[0].position;
            Graphics.DrawMeshNow(GeometryUtil.cylinder8, Matrix4x4.TRS(sPoints[0].position + edgeVec / 2, Quaternion.FromToRotation(Vector3.up, edgeVec), new Vector3(.005f, edgeVec.magnitude / 2, .005f)));
        }
    }
}
