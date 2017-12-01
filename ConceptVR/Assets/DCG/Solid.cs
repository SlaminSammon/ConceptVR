using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Solid : DCGElement {
    public List<Face> faces;
    public Mesh mesh;

    public Solid()
    {
        faces = new List<Face>();
        DCGBase.solids.Add(this);
    }

    public Solid(Mesh m, Matrix4x4 t, Vector3 translate)
    {
        mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        ArrayList points = new ArrayList();
        List<int> pointMap = new List<int>();
        faces = new List<Face>();

        foreach (Vector3 v in m.vertices)
        {
            int match = -1;
            Vector3 tv = (Vector3)(t * v) + translate;
            for (int i = 0; i < points.Count; ++i)
            {
                Point p = (Point)points[i];
                if (Vector3.Distance(p.position, tv) <= .0001f)
                {
                    match = i;
                    break;
                }
            }
            if (match == -1)
            {
                verts.Add(tv);
                points.Add(new Point(tv));
                pointMap.Add(points.Count-1);
            } else
            {
                pointMap.Add(match);
            }
        }

        for (int tri = 0; tri < m.triangles.Length - 2; tri += 3)
        {
            tris.Add(pointMap[m.triangles[tri]]);
            tris.Add(pointMap[m.triangles[tri+1]]);
            tris.Add(pointMap[m.triangles[tri+2]]);

            Point p1 = (Point)points[pointMap[m.triangles[tri]]];
            Point p2 = (Point)points[pointMap[m.triangles[tri+1]]];
            Point p3 = (Point)points[pointMap[m.triangles[tri+2]]];

            List<Edge> edges = new List<Edge>();
            edges.Add(new Edge(p1, p2));
            edges.Add(new Edge(p2, p3));
            edges.Add(new Edge(p3, p1));

            Face f = new Face(edges);
            faces.Add(f);
            f.solids.Add(this);
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris.ToArray(), 0);
        DCGBase.solids.Add(this);
    }

    public override void Render()
    {
        Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
    }

    public override void Update()
    {
        //TODO
    }

    public override void Remove()
    {
        DCGBase.solids.Remove(this);
    }

    public List<Point> getPoints()
    {
        List<Point> points = new List<Point>();
        foreach(Face f in faces)
        {
            points.AddRange(f.getPoints());
        }
        return points.Distinct().ToList();
    }
    public List<Edge> getEdges()
    {
        List<Edge> edges = new List<Edge>();
        foreach(Face f in faces)
        {
            edges.AddRange(f.edges);
        }
        return edges.Distinct().ToList();
    }
}
