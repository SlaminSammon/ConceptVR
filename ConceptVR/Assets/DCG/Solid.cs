using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System.Linq;

public class Solid : DCGElement {
    public List<Face> faces;
    Mesh mesh;

    public Solid()
    {
        faces = new List<Face>();
        DCGBase.solids.Add(this);
    }

    public Solid(List<Face> faces)
    {
        this.faces = faces;
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

    public void updateMesh()
    {

    }

    public override void Render()
    {
        foreach (Face f in faces)
            f.Render();
    }

    public override void Update()
    {
        //TODO
    }

    public override void Remove()
    {
        foreach (Face f in faces)
            f.solids.Remove(this);
        DCGBase.solids.Remove(this);
    }
    
    public override bool ChildrenSelected()
    {
        foreach (Face e in faces)
            if (!e.isSelected && !e.ChildrenSelected())
                return false;
        return true;
    }

    public List<Point> getPoints()
    {
        List<Point> points = new List<Point>();
        foreach(Face f in faces)
        {
            points.AddRange(f.GetPoints());
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
    public override void Lock()
    {
        foreach (Face f in faces)
        {
            if (!f.isLocked)
                f.Lock();
        }
        isLocked = true;
    }
    public override void Unlock()
    {
        foreach (Face f in faces)
        {
            if (f.isLocked)
                f.Unlock();
        }
        isLocked = false;
    }

    public static Solid FindClosedSurface(Point start)
    {
        List<Point> connected = start.GetConnectedPoints(-1);   //Get the set of points we can path to
        Dictionary<Face, int> touches = new Dictionary<Face, int>();

        foreach (Point p in connected)  //Check for holes in the surface encompassed by the set of points
        {
            int singleTouches = 0;
            int doubleTouches = 0;
            int plusTouches = 0;
            foreach (Edge e in p.edges)
            {
                foreach (Face f in e.faces)
                {
                    if (!touches.ContainsKey(f))
                        touches.Add(f, 0);
                    int t = ++touches[f];
                    if (t == 1)
                        singleTouches++;
                    else if (t == 2)
                    {
                        singleTouches--;
                        doubleTouches++;
                    }
                    else if (t > 2)
                        plusTouches++;
                }
            }

            Debug.Log(singleTouches + " " + doubleTouches + " " + plusTouches);

            if (singleTouches > 0 || plusTouches > 0)   //if holes or weird shit are detected, fail out
                return null;

            foreach (Edge e in p.edges)
            {
                foreach (Face f in e.faces)
                    touches[f] = 0;
            }
        }

        List<Face> faces = start.edges[0].faces[0].GetConnectedFaces(-1);

        if (faces.Count > 1) //Ensure we didn't just make a single face
            return new Solid(faces);    //return a new solid built from the set of connected faces
        else
            return null;
    }
}
