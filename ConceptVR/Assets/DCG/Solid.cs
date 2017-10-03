using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solid {
    public List<Face> faces;
    public Mesh mesh;
    public int lastMoveID;


    public Solid(Mesh m, Matrix4x4 t)
    {
        mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        ArrayList points = new ArrayList();
        ArrayList allPoints = new ArrayList();
        faces = new List<Face>();

        foreach (Vector3 v in m.vertices)
        {
            Point match = null;
            foreach (Point p in points)
            {
                if (Vector3.Distance(p.position, t * v) <= 0.01f)
                {
                    match = p;
                    break;
                }
            }
            if (match == null)
            {
                verts.Add(t * v);
                points.Add(new Point(t * v));
                allPoints.Add(new Point(t * v));
            } else
            {
                allPoints.Add(match);
            }
        }

        for (int tri = 0; tri < m.triangles.Length; tri += 3)
        {
            tris.Add(m.triangles[tri]);
            tris.Add(m.triangles[tri+1]);
            tris.Add(m.triangles[tri+2]);

            Point p1 = (Point)allPoints[m.triangles[tri]];
            Point p2 = (Point)allPoints[m.triangles[tri+1]];
            Point p3 = (Point)allPoints[m.triangles[tri+2]];

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

    public void updateMesh()
    {

    }
}
