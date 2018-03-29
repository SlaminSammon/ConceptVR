﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : DCGElement
{
    public List<Point> points;
    public List<Face> faces;

    public bool isLoop;
    public GameObject Managers;

    public Edge()
    {
        this.isLoop = false;
        this.points = new List<Point>();
        this.faces = new List<Face>();
    }

    public Edge(List<Point> points, bool isLoop)
    {
        this.isLoop = isLoop;
        this.points = points;
        this.faces = new List<Face>();
        foreach (Point p in points)
            p.edges.Add(this);

        elementID = nextElementID();
        DCGBase.edges.Add(this);
        DCGBase.all.Add(elementID, this as DCGElement);

        if (NetPlayer.local != null)
        {
            int[] pointIDs = new int[points.Count];
            for (int i = 0; i < points.Count; ++i)
                pointIDs[i] = points[i].elementID;
            DCGBase.synch.CmdAddElement(elementID, pointIDs, ElementType.edge, NetPlayer.local.playerID);
        }
            
    }

    //Network constructor
    public Edge(List<Point> points, int netID)
    {
        this.isLoop = false;
        this.points = points;
        this.faces = new List<Face>();
        foreach (Point p in points)
            p.edges.Add(this);

        elementID = netID;
        DCGBase.edges.Add(this);
        DCGBase.all.Add(elementID, this as DCGElement);
    }

    public Edge(Point p1, Point p2)
    {
        this.points = new List<Point>();
        this.points.Add(p1);
        this.points.Add(p2);
        this.faces = new List<Face>();
        foreach (Point p in points)
            p.edges.Add(this);

        elementID = nextElementID();
        DCGBase.edges.Add(this);
        DCGBase.all.Add(elementID, this as DCGElement);

        if (NetPlayer.local != null)
        {
            int[] pointIDs = new int[2];
            pointIDs[0] = p1.elementID;
            pointIDs[1] = p2.elementID;
            DCGBase.synch.CmdAddElement(elementID, pointIDs, ElementType.edge, NetPlayer.local.playerID);
        }
    }

    public virtual void updateMesh()
    {
        return;
    }

    public override void Render()
    {
        float playerScale = GameObject.Find("Managers").GetComponent<SettingsManager>().playerScale;
        Vector3 edgeVec;
        for (int i = 0; i < points.Count - 1; ++i)
        {
            edgeVec = points[i].position - points[i + 1].position;
            Graphics.DrawMeshNow(GeometryUtil.cylinder8, Matrix4x4.TRS(points[i].position - edgeVec / 2, Quaternion.FromToRotation(Vector3.up, edgeVec), new Vector3(.005f * playerScale, edgeVec.magnitude / 2, .005f * playerScale)));
            // Graphics.DrawMeshNow(GeometryUtil.cylinder8, Matrix4x4.TRS(points[i].position - edgeVec / 2, Quaternion.FromToRotation(Vector3.up, edgeVec), new Vector3(.005f, edgeVec.magnitude / 2, .005f)));
        }
        if (isLoop)
        {
            edgeVec = points[points.Count - 1].position - points[0].position;
            Graphics.DrawMeshNow(GeometryUtil.cylinder8, Matrix4x4.TRS(points[0].position + edgeVec / 2, Quaternion.FromToRotation(Vector3.up, edgeVec), new Vector3(.005f * playerScale, edgeVec.magnitude / 2, .005f * playerScale)));
            // Graphics.DrawMeshNow(GeometryUtil.cylinder8, Matrix4x4.TRS(points[0].position + edgeVec / 2, Quaternion.FromToRotation(Vector3.up, edgeVec), new Vector3(.005f, edgeVec.magnitude / 2, .005f)));
        }
    }

    public override void Remove()
    {
        foreach (Face f in faces)
            f.Remove();
        foreach (Point p in points)
            p.edges.Remove(this);
        DCGBase.edges.Remove(this);
    }

    public override bool ChildrenSelected()
    {
        foreach (Point e in points)
            if (!e.isSelected)
                return false;
        return true;
    }

    public override float Distance(Vector3 position)
    {
        float nDist2 = Mathf.Infinity;
        for (int i = 0; i < points.Count - 1; ++i)
            nDist2 = Mathf.Min(Vector3.ProjectOnPlane(position - points[i].position, points[i + 1].position - points[i].position).sqrMagnitude, nDist2);

        if (isLoop)
            nDist2 = Mathf.Min(Vector3.ProjectOnPlane(position - points[0].position, points[points.Count].position - points[0].position).sqrMagnitude, nDist2);

        return Mathf.Sqrt(nDist2);
    }

    public override DCGConstraint NearestConstraint(Vector3 pos)
    {
        float nd2 = Mathf.Infinity;
        float nx = 0;
        int ni = 0;
        for (int i = 0; i < points.Count-1; ++i)
        {
            Vector3 dir = points[i + 1].position - points[i].position;
            Vector3 diff = pos - points[i].position;

            Vector3 x = Vector3.Project(diff, dir);
            float dist2 = (diff - x).sqrMagnitude;
            
            if (dist2 < nd2)
            {
                nd2 = dist2;
                nx = x.magnitude/dir.magnitude;
                ni = i;
            }
        }

        DCGConstraint con = new DCGConstraint();
        con.constrainerID = elementID;
        con.constraintData = new float[] {ni, nx};

        return con;
    }

    public override Vector3 ConstraintPosition(float[] constraintData)
    {
        int i = Mathf.FloorToInt(constraintData[0]);
        return points[i].position + constraintData[1] * (points[i + 1].position - points[i].position);
    }

    public override List<Point> GetPoints() { return points; }

    public override List<DCGElement> Extrude()
    {
        List<Point> ep = new List<Point>();
        List<DCGElement> eElem = new List<DCGElement>();
        foreach (Point p in points)
        {
            ep.Add(new Point(p.position));
            eElem.Add(p);
        }

        ep.Reverse();

        List<Edge> ee = new List<Edge>();
        Edge oppEdge = new Edge(ep, isLoop);
        ee.Add(oppEdge);
        ee.Add(new Edge(ep[ep.Count - 1], points[0]));
        ee.Add(this);
        ee.Add(new Edge(points[ep.Count - 1], ep[0]));
        Face ef = new Face(ee);
        eElem.Add(oppEdge); // newSelement is last element
        return eElem;
    }
    
    public Vector3[] smoothVerts(int res)
    {
        Vector3[] verts = new Vector3[res + 1];
        Vector3[] control = new Vector3[points.Count];

        for (int i = 0; i < points.Count; ++i)
            control[i] = points[i].position;

        for (int i = 0; i <= res; ++i)
            verts[i] = GeometryUtil.Bezerp(control, (float)i / (float)res);

        return verts;
    }
    
    public override void Lock()
    {
        foreach (Point p in points)
        {
            if (!p.isLocked)
                p.Lock();
        }
        isLocked = true;
    }
    public override void Unlock()
    {
        foreach (Point p in points)
        {
            if (p.isLocked)
                p.Unlock();
        }
        isLocked = false;
    }

    public bool HasEndpoints(Point p1, Point p2)
    {
        return (points[0] == p1 && points[points.Count-1] == p2 || points[0] == p2 && points[points.Count-1] == p1);
    }
}
