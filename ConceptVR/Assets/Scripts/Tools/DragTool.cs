using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTool : Tool
{
    enum GrabType { Point, Edge, Face };


    public float grabDistance = .1f;
    Face nearestFace;
    Edge nearestEdge;
    Point nearestPoint;

    List<Point> grabbedPoints;
    List<Vector3> pointGrabPos;
    Vector3 conGrabPos;
    Quaternion grabOrientation;
    DCGBase dcgObject;
    GrabType grabType;


    // Use this for initialization
    void Start () {
        dcgObject = GameObject.Find("DCG").GetComponent<DCGBase>();
    }
	
	// Update is called once per frame
	void Update () {
        if (triggerInput && grabbedPoints != null)
            updateGrabbed();

        if (grabbedPoints == null)
        {
            updateFace();
            updateEdge();
            updatePoint();
        }
	}

    private void OnRenderObject()
    {
        if (nearestFace != null)
        {
            dcgObject.faceMat.SetPass(0);

            Graphics.DrawMeshNow(nearestFace.mesh, Vector3.zero + nearestFace.getNormal() * .005f, Quaternion.identity);
            Graphics.DrawMeshNow(nearestFace.mesh, Vector3.zero - nearestFace.getNormal() * .005f, Quaternion.identity);
            Graphics.DrawMeshNow(dcgObject.pointMesh, Matrix4x4.TRS(controllerPosition, Quaternion.identity, new Vector3(.02f, .02f, .02f)));
        }
        else if (nearestEdge != null)
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
            Graphics.DrawMeshNow(dcgObject.pointMesh, Matrix4x4.TRS(controllerPosition, Quaternion.identity, new Vector3(.02f, .02f, .02f)));
        }
        else if (nearestPoint != null)
        {
            dcgObject.pointMat.SetPass(0);
            Graphics.DrawMeshNow(dcgObject.pointMesh, Matrix4x4.TRS(nearestPoint.position, Quaternion.identity, new Vector3(.02f, .02f, .02f)));
            Graphics.DrawMeshNow(dcgObject.pointMesh, Matrix4x4.TRS(controllerPosition, Quaternion.identity, new Vector3(.02f, .02f, .02f)));
        }
    }

    public override void TriggerDown()
    {

        if (nearestPoint != null)
        {
            grabbedPoints = new List<Point>();
            grabbedPoints.Add(nearestPoint);
            grabType = GrabType.Point;
        }

        if (nearestEdge != null)
        {
            grabbedPoints = nearestEdge.points;
            grabType = GrabType.Edge;
        }

        if (nearestFace != null)
        {
            grabbedPoints = nearestFace.getPoints();
            grabType = GrabType.Face;
        }

        
        conGrabPos = controllerPosition;
        grabOrientation = transform.rotation;
        pointGrabPos = new List<Vector3>();
        foreach (Point p in grabbedPoints)
            pointGrabPos.Add(p.position - conGrabPos);

        return;
    }

    public override void TriggerUp()
    {
        grabbedPoints = null;
        return;
    }

    public override void GripDown()
    {
        extrude();
    }

    void updateFace()
    {
        float nDist2 = grabDistance * grabDistance;
        nearestFace = null;
        foreach (Face f in DCGBase.faces)
        {
            List<Point> fp = f.getPoints();   //face points
            List<Vector2> pp = new List<Vector2>();     //projected points
            Vector3 zvec = f.getNormal();
            Vector3 yvec = fp[1].position - fp[0].position;
            Vector3 grabProj3 = Vector3.ProjectOnPlane(controllerPosition, zvec);
            Vector3 grabProjY = Vector3.Project(grabProj3, yvec);
            Vector2 grab = new Vector2((grabProj3 - grabProjY).magnitude, grabProjY.magnitude);

            Vector3 grabRel = controllerPosition - fp[0].position;
            float dist2 = (grabRel - Vector3.ProjectOnPlane(grabRel, zvec)).sqrMagnitude;

            if (dist2 > nDist2)
                continue;

            foreach (Point p in fp)
            {
                Vector3 proj3 = Vector3.ProjectOnPlane(p.position, zvec);
                Vector3 projY = Vector3.Project(proj3, yvec);
                pp.Add(new Vector2((proj3 - projY).magnitude, projY.magnitude));   //TODO
            }


            //Check if the projected poly contains the projected controller
            float count = 0;
            Vector2 prev = pp[pp.Count - 1];
            foreach (Vector2 cur in pp)
            {
                if ((prev.y > grab.y != cur.y > grab.y) && (grab.x < Mathf.Lerp(prev.x, cur.x, (grab.y - prev.y) / (cur.y - prev.y))))
                    count++;
                prev = cur;
            }


            if (count % 2 == 1)
            {
                nDist2 = dist2;//TODO
                nearestFace = f;
            }
        }

    }

    void updateEdge()
    {
        float nDist2 = grabDistance * grabDistance;
        nearestEdge = null;
        foreach (Edge e in DCGBase.edges)
        {
            for (int i = 0; i < (e.isLoop ? e.points.Count : e.points.Count - 1); ++i)
            {
                Vector3 ediff = (e.points[i].position - e.points[(i + 1) % e.points.Count].position);
                Vector3 hdiff = (controllerPosition - e.points[i].position);
                Vector3 proj = Vector3.Project(hdiff, ediff);
                float dist2 = (hdiff - proj).sqrMagnitude;
                if (dist2 < nDist2 && hdiff.sqrMagnitude < ediff.sqrMagnitude && (controllerPosition - e.points[(i + 1) % e.points.Count].position).sqrMagnitude < ediff.sqrMagnitude)
                {
                    nearestEdge = e;
                    nDist2 = dist2;
                }
            }
        }
    }

    void updatePoint()
    {
        float nDist2 = grabDistance * grabDistance;
        nearestPoint = null;
        foreach (Point p in DCGBase.points)
        {
            float dist2 = (controllerPosition - p.position).sqrMagnitude;
            if (dist2 < nDist2)
            {
                nearestPoint = p;
                nDist2 = dist2;
            }
        }
    }

    void updateGrabbed()
    {
        Quaternion orientation = transform.rotation * Quaternion.Inverse(grabOrientation);

        for (int i = 0; i < grabbedPoints.Count; ++i) {
            grabbedPoints[i].position = controllerPosition + orientation * pointGrabPos[i];
        }
        grabbedPoints[0].setPosition(controllerPosition + orientation * pointGrabPos[0]);
    }

    void extrude()
    {
        if (grabType == GrabType.Point)
        {
            Point ep = new Point(grabbedPoints[0].position);
            new Edge(ep, grabbedPoints[0]);
            grabbedPoints = new List<Point>();
            grabbedPoints.Add(ep);
            TriggerDown();
        } else if (grabType == GrabType.Edge)
        {
            List<Point> ep = new List<Point>();
            foreach (Point p in grabbedPoints)
            {
                ep.Add(new Point(p.position));
            }

            ep.Reverse();

            List<Edge> ee = new List<Edge>();
            ee.Add(new Edge(ep, nearestEdge.isLoop));
            ee.Add(new Edge(ep[ep.Count - 1], grabbedPoints[0]));
            ee.Add(nearestEdge);
            ee.Add(new Edge(grabbedPoints[ep.Count - 1], ep[0]));

            Face ef = new Face(ee);
            grabbedPoints = ep;
            nearestEdge = ee[0];
            TriggerDown();
        } else if (grabType == GrabType.Face)
        {
            List<Point> corners = new List<Point>();
            List<Point> eCorners = new List<Point>();

            foreach (Edge e in nearestFace.edges)
            {

            }

            TriggerDown();
        }
    }
}
