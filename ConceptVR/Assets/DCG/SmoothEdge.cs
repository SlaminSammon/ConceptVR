﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothEdge : Edge {
    public Vector3[] smoothPoints;
    public Mesh mesh;

    public SmoothEdge(List<Point> points)
    {
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
            DCGBase.synch.CmdAddElement(elementID, pointIDs, ElementType.smoothEdge, NetPlayer.local.playerID);
        }

        updateMesh();
    }

    public override void Render()
    {
        Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity, 0);
    }

    public override List<DCGElement> Extrude()
    {
        List<Point> ep = new List<Point>();
        List<DCGElement> eElem = new List<DCGElement>();
        foreach (Point p in points)
            ep.Add(new Point(p.position));

        ep.Reverse();

        List<Edge> ee = new List<Edge>();
        SmoothEdge oppEdge = new SmoothEdge(ep);
        ee.Add(oppEdge);
        eElem.Add(oppEdge);
        ee.Add(new Edge(ep[ep.Count - 1], points[0]));
        ee.Add(this);
        ee.Add(new Edge(points[ep.Count - 1], ep[0]));

        Face ef = new SmoothQuadFace(ee);

        return eElem;
    }

    static int curveRes = 50;
    static int roundRes = 10;
    static float roundRad = .005f;
    public new void updateMesh()
    {
        smoothPoints = this.smoothVerts(curveRes);

        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        int[] tris = new int[3 * (curveRes - 1) * roundRes * 2];

        for (int i = 0; i < smoothPoints.Length; ++i)
        {
            Vector3 forward = smoothPoints[Mathf.Min(i + 1, curveRes)] - smoothPoints[Mathf.Max(i - 1, 0)];
            Vector3 left = Vector3.Cross(forward, Vector3.up).normalized;
            Vector3 up = Vector3.Cross(left, forward).normalized;

            for (int j = 0; j < roundRes; ++j)
            {
                Vector3 v = Mathf.Cos(2 * Mathf.PI * j / roundRes) * left + Mathf.Sin(2 * Mathf.PI * j / roundRes) * up;
                verts.Add(smoothPoints[i] + v * roundRad);
                normals.Add(v);
            }
        }

        for (int i = 0; i < (curveRes - 1) * roundRes; ++i)
        {
            int tb = i * 6;
            tris[tb++] = i;
            tris[tb++] = i + roundRes;
            tris[tb++] = i + 1;

            tris[tb++] = i + roundRes;
            tris[tb++] = i + roundRes + 1;
            tris[tb++] = i + 1;
        }

        mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.SetNormals(normals);
    }
}