using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : DCGElement
{
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
        //TODO: Signal creation over network
    }

    public Edge(List<Point> points, int netID)
    {
        this.isLoop = false;
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

    public override void Render()
    {
        if (smooth)
        {
            Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity, 0);
        }
        else
        {
            Vector3 edgeVec;
            for (int i = 0; i < points.Count - 1; ++i)
            {
                edgeVec = points[i].position - points[i + 1].position;
                Graphics.DrawMeshNow(GeometryUtil.cylinder8, Matrix4x4.TRS(points[i].position - edgeVec / 2, Quaternion.FromToRotation(Vector3.up, edgeVec), new Vector3(.005f, edgeVec.magnitude / 2, .005f)));
            }
            if (isLoop)
            {
                edgeVec = points[points.Count - 1].position - points[0].position;
                Graphics.DrawMeshNow(GeometryUtil.cylinder8, Matrix4x4.TRS(points[0].position + edgeVec / 2, Quaternion.FromToRotation(Vector3.up, edgeVec), new Vector3(.005f, edgeVec.magnitude / 2, .005f)));
            }
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

    public override List<Point> GetPoints() { return points; }

    public override List<DCGElement> Extrude()
    {
        List<Point> ep = new List<Point>();
        List<DCGElement> eElem = new List<DCGElement>();
        foreach (Point p in points)
            ep.Add(new Point(p.position));

        ep.Reverse();

        List<Edge> ee = new List<Edge>();
        Edge oppEdge = new Edge(ep, isLoop);
        ee.Add(oppEdge);
        eElem.Add(oppEdge);
        ee.Add(new Edge(ep[ep.Count - 1], points[0]));
        ee.Add(this);
        ee.Add(new Edge(points[ep.Count - 1], ep[0]));

        Face ef = new Face(ee);

        return eElem;
    }

    public void setSmooth(bool smooth)
    {
        this.smooth = smooth;
        if (smooth)
            updateMesh();
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
