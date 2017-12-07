using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Face : DCGElement {
    public List<Edge> edges;   //The edges of this face
    public List<Solid> solids; //Any solids that this face is a part of.  Since we're in 3D, this should logically only ever be 1.
    public Mesh mesh;

    public bool isAwful;
    bool normalConfident;
    
    public Face()
    {
        solids = new List<Solid>();
        edges = new List<Edge>();

        DCGBase.faces.Add(this);
    }

    public Face(List<Edge> edges)
    {
        solids = new List<Solid>();
        this.edges = edges;
        foreach (Edge e in edges)
            e.faces.Add(this);
        DCGBase.faces.Add(this);
        updateAwful();
        updateMesh();
    }

    public override void Render()
    {
        Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
    }

    public override void Update()
    {
        updateAwful();
        updateMesh();
    }

    public override void Remove()
    {
        foreach (Solid s in solids)
            s.Remove();
        foreach (Edge e in edges)
            e.faces.Remove(this);
        DCGBase.faces.Remove(this);
    }

    public void updateAwful()
    {
        Vector3 avgNormal = getNormal();
        /*if (!normalConfident)
        {
            isAwful = true;
            return;
        }*/
        List<Point> points = getPoints();
        Vector2[] proj = new Vector2[points.Count];     //projected points
        Vector3 yVec = points[1].position - points[0].position;
        for (int i = 0; i < points.Count; ++i)
        {
            Vector3 proj3 = Vector3.ProjectOnPlane(points[i].position, avgNormal);
            Vector3 projY = Vector3.Project(proj3, yVec);
            proj[i] = new Vector2((proj3 - projY).magnitude, projY.magnitude);
        }

        for (int i = 0; i < points.Count; ++i)
        {
            Vector2 a = proj[i];
            Vector2 b = proj[(i+1) % points.Count];
            Vector2 ir = new Vector2(a.y, -a.x);
            for (int j = i+1; j < points.Count; ++j)
            {
                Vector2 c = proj[j];
                Vector2 d = proj[(i + 1) % points.Count];
                if (Mathf.Sign(Vector2.Dot(c-a, ir)) != Mathf.Sign(Vector2.Dot(d - a, ir)))
                {
                    Vector2 jr = new Vector2(c.y, -c.x);
                    if (Mathf.Sign(Vector2.Dot(a - c, jr)) != Mathf.Sign(Vector2.Dot(b - c, jr)))
                    {
                        isAwful = true;
                        return;
                    }
                }
            }
        }
        isAwful = false;
    }

    public void updateMesh()
    {
        mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        Vector3 avgNormal = getNormal();
        
        Vector3 lpos = edges[edges.Count-1].points[edges[edges.Count-1].points.Count-1].position; //Gomenasai
        foreach (Edge e in edges)
            foreach (Point p in e.points)
            {
                if (p.position != lpos)
                {
                    verts.Add(p.position);
                    normals.Add(avgNormal);
                }   
                lpos = p.position;
            }

        List<int> tris;

        tris = GeometryUtil.mediocreTriangulate(verts);

        //mirror verts
        int vertCount = verts.Count;
        for (int i = 0; i < vertCount; ++i)
        {
            verts.Add(verts[i]);
            normals.Add(-avgNormal);
        }

        //mirror triangles
        int triCount = tris.Count;
        for (int i = 0; i < triCount; i += 3)
        {
            tris.Add(tris[i] + vertCount);
            tris.Add(tris[i+2] + vertCount);
            tris.Add(tris[i+1] + vertCount);
        }

        mesh.SetVertices(verts);
        mesh.SetNormals(normals);
        mesh.SetTriangles(tris, 0);
        //mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
    }
    
    public List<Point> getPoints()
    {
        List<Point> points = new List<Point>();
        foreach(Edge e in edges)
        {
            points.AddRange(e.points);
        }
        return points.Distinct().ToList();
    }

    public Vector3 getNormal()
    {
        List<Point> points = getPoints();
        Vector3 prevPos = points[points.Count - 1].position;
        Vector3 prev = prevPos - points[points.Count - 2].position;
        Vector3 norm = new Vector3(0,0,0);
        foreach (Point p in points)
        {
            Vector3 next = p.position - prevPos;
            Vector3 cross = Vector3.Cross(prev, next);
            if (Vector3.Dot(cross, norm) < 0)
            {
                cross = -cross;
                normalConfident = false;
            }
            norm += cross;
            prev = next;
            prevPos = p.position;
        }

        return norm.normalized;
    }
}
