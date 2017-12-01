using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Face : DCGElement {
    public List<Edge> edges;   //The edges of this face
    public List<Solid> solids; //Any solids that this face is a part of.  Since we're in 3D, this should logically only ever be 1.
    public Mesh mesh;
    
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
        updateMesh();
    }

    public override void Render()
    {
        Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
    }

    public override void Update()
    {
        updateMesh();
    }

    public override void Remove()
    {
        foreach (Solid s in solids)
            s.Remove();
        DCGBase.faces.Remove(this);
    }

    public void updateMesh()
    {
        mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        Vector3 avgNormal = getNormal();
        normals.Add(avgNormal);
        normals.Add(avgNormal);
        verts.Add(new Vector3());
        verts.Add(new Vector3());

        List<int> tris = new List<int>();
        Vector3 center = new Vector3();
        Vector3 lpos = edges[edges.Count-1].points[edges[edges.Count-1].points.Count-1].position; //Gomenasai
        foreach (Edge e in edges)
        {
            foreach (Point p in e.points)
            {
                if (p.position != lpos)
                {
                    verts.Add(p.position);
                    verts.Add(p.position);
                    normals.Add(avgNormal);
                    normals.Add(avgNormal);
                    center += p.position;
                    tris.Add(1); tris.Add(verts.Count - 1); tris.Add(verts.Count - 3);  //TODO: Nuke this line, it's here to get around backface culling by creating a seperate backface
                    tris.Add(verts.Count - 4); tris.Add(verts.Count - 2); tris.Add(0);
                }
                lpos = p.position;
            }
        }

        verts[0] = center / ((verts.Count - 1) / 2f);
        verts[1] = center / ((verts.Count - 1) / 2f);
        tris[2] = verts.Count-1;
        mesh.SetVertices(verts);
        mesh.SetNormals(normals);
        mesh.SetTriangles(tris, 0);
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
            norm += Vector3.Cross(prev, next);
            prev = next;
            prevPos = p.position;
        }

        return norm.normalized;
    }
}
