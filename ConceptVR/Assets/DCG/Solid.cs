using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solid {
    public List<Face> faces;
    public Mesh mesh;


    public Solid(Mesh m, Matrix4x4 t)
    {
        mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        ArrayList points = new ArrayList();
        faces = new List<Face>();

        foreach (Vector3 p in m.vertices)
        {
            verts.Add(t * p);
            points.Add(new Point(t * p));
        }

        for (int tri = 0; tri < m.triangles.Length; tri += 3)
        {
            tris.Add(m.triangles[tri]);
            tris.Add(m.triangles[tri+1]);
            tris.Add(m.triangles[tri+2]);

            Point p1 = (Point)points[m.triangles[tri]];
            Point p2 = (Point)points[m.triangles[tri+1]];
            Point p3 = (Point)points[m.triangles[tri+2]];

            List<Edge> edges = new List<Edge>();
            edges.Add(new Edge(p1, p2));
            edges.Add(new Edge(p2, p3));
            edges.Add(new Edge(p3, p1));

            faces.Add(new Face(edges));
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris.ToArray(), 0);

        DCGBase.solids.Add(this);
    }
}
