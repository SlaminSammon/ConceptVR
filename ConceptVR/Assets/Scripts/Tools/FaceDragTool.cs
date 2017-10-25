using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDragTool : Tool {

    public float grabDistance = .1f;
    Face nearestFace;
    List<Point> facePoints;
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
            if (nearestFace != null)
            {

                for (int i = 0; i < facePoints.Count; ++i)
                {
                    facePoints[i].setPosition(pointGrabPos[i] + (transform.position - conGrabPos));
                }
            }
        }
        else
        {
            float nDist2 = grabDistance * grabDistance;
            nearestFace = null;
            foreach (Face f in DCGBase.faces)
            {
                List<Point> fp = nearestFace.getPoints();   //face points
                List<Vector2> pp = new List<Vector2>();     //projected points
                Vector3 zvec = f.getNormal();
                Vector2 grabProj = Vector3.ProjectOnPlane(transform.position, zvec);    //TODO

                foreach (Point p in fp)
                {
                    pp.Add(Vector3.ProjectOnPlane(p.position, zvec));   //TODO
                }

                Vector2 prev = pp[pp.Count - 1];
                foreach (Vector2 p in pp)
                {
                    //Check if a lione sweeping left from the projected grab position passes through the segment from prev to p
                    prev = p;
                }
            }
        }
    }

    public override void TriggerDown()
    {
        if (nearestFace != null)
        {
            facePoints = nearestFace.getPoints();
            pointGrabPos = new List<Vector3>();
            foreach (Point p in facePoints)
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
        if (nearestFace != null)
        {
            dcgObject.edgeMat.SetPass(0);


            Graphics.DrawMeshNow(nearestFace.mesh, Vector3.zero, Quaternion.identity);
            Graphics.DrawMeshNow(dcgObject.pointMesh, Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(.02f, .02f, .02f)));
        }
    }
}
