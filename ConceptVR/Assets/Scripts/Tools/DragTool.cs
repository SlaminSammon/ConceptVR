using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTool : Tool
{
    enum GrabType { Point, Edge, Face };
    
    public Material selectMat;
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
            selectMat.SetPass(0);

            Graphics.DrawMeshNow(nearestFace.mesh, Vector3.zero + nearestFace.getNormal() * .005f, Quaternion.identity);
            Graphics.DrawMeshNow(nearestFace.mesh, Vector3.zero - nearestFace.getNormal() * .005f, Quaternion.identity);
            Graphics.DrawMeshNow(dcgObject.pointMesh, Matrix4x4.TRS(controllerPosition, Quaternion.identity, new Vector3(.02f, .02f, .02f)));
        }
        else if (nearestEdge != null)
        {
            selectMat.SetPass(0);


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
            selectMat.SetPass(0);
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
        if (grabbedPoints != null)
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
        nearestFace = DCGBase.NearestFace(controllerPosition, grabDistance);
    }

    void updateEdge()
    {
        nearestEdge = DCGBase.NearestEdge(controllerPosition, grabDistance);
    }

    void updatePoint()
    {
        nearestPoint = DCGBase.NearestPoint(controllerPosition, grabDistance);
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

            List<Point> ePoints = new List<Point>();
            List<Edge> eEdges = new List<Edge>();
            List<Face> eFaces = new List<Face>();
            
            eFaces.Add(nearestFace);

            foreach (Edge e in nearestFace.edges)
            {
                corners.Add(e.points[0]);
                eCorners.Add(new Point(e.points[0].position));
            }

            int i = 0;
            foreach (Edge e in nearestFace.edges)
            { 
                List<Point> edgePoints = new List<Point>();
                int j = 0;
                foreach (Point p in e.points)
                {
                    if (j < e.points.Count - 1)
                        ePoints.Add(p);


                    if (j == 0)
                        edgePoints.Add(eCorners[i]);
                    else if (j == e.points.Count-1)
                        edgePoints.Add(eCorners[(i + 1) % eCorners.Count]);
                    else
                        edgePoints.Add(new Point(p.position));
                    ++j;
                }

                edgePoints.Reverse();
                Edge eEdge = new Edge(edgePoints, e.isLoop);
                eEdges.Add(eEdge);

                List<Edge> faceEdges = new List<Edge>();
                faceEdges.Add(eEdge);
                faceEdges.Add(new Edge(eEdge.points[eEdge.points.Count - 1], e.points[0]));
                faceEdges.Add(e);
                faceEdges.Add(new Edge(e.points[e.points.Count - 1], eEdge.points[0]));
                eFaces.Add(new Face(faceEdges));
                ++i;
            }

            eEdges.Reverse();
            Face gFace = new Face(eEdges);
            eFaces.Add(gFace);

            nearestFace = gFace;
            grabbedPoints = ePoints;

            TriggerDown();
        }
    }
}
