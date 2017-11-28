using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : DCGElement {
    public List<Point> points;
    public List<Face> faces;

    public Vector3[] smoothPoints;

    public bool isLoop;
    public bool smooth;

    public Mesh mesh;

    public Edge(List<Point> points, bool isLoop)
    {
        this.isLoop = isLoop;
        this.points = points;
        this.faces = new List<Face>();
        foreach (Point p in points)
            p.edges.Add(this);
        DCGBase.edges.Add(this);
        Debug.Log(points.Count);
    }

    public Edge(Point p1, Point p2)
    {
        this.points = new List<Point>();
        this.points.Add(p1);
        this.points.Add(p2);
        this.faces = new List<Face>();
        foreach (Point p in points)
            p.edges.Add(this);
        DCGBase.edges.Add(this);
    }

    public void setSmooth(bool smooth)
    {
        this.smooth = smooth;
        if (smooth)
            updateMesh();
    }

    Vector3 Bezerp(Vector3[] control, float t)
    {
        Vector3[] nControl = new Vector3[control.Length - 1];
        for (int i = 0; i < nControl.Length; ++i)
        {
            nControl[i] = Vector3.Lerp(control[i], control[i + 1], t);
        }

        if (nControl.Length == 1)
            return nControl[0];
        else
            return Bezerp(nControl, t);
    }

    public Vector3[] smoothVerts(int res)
    {
        Vector3[] verts = new Vector3[res+1];
        Vector3[] control = new Vector3[points.Count];

        for (int i = 0; i < points.Count; ++i)
            control[i] = points[i].position;
        
        for (int i = 0; i <= res; ++i)
            verts[i] = Bezerp(control, (float)i / (float)res);

        return verts;
    }

    static int curveRes = 50;
    static int roundRes = 10;
    static float roundRad = .005f;
    public void updateMesh()
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

        for (int i = 0; i < (curveRes-1) * roundRes; ++i)
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
