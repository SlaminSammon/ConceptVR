using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face {
    List<Edge> edges;   //The edges of this face
    List<Solid> solids; //Any solids that this face is a part of.  Since we're in 3D, this should logically only ever be 1.
    public Mesh mesh;  
    
    public Face(List<Edge> edges)
    {
        this.edges = edges;
        DCGBase.faces.Add(this);
        updateMesh();
    }

    public void updateMesh()
    {
        mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
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
                    center += p.position;
                    tris.Add(0);
                    tris.Add(verts.Count - 1);
                    tris.Add(verts.Count - 2);
                }
                lpos = p.position;
            }
        }

        verts[0] = center / (verts.Count - 1);
        tris[2] = verts.Count-1;
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
    }
}
