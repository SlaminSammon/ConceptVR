using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face {
    List<Edge> edges;   //The edges of this face
    List<Solid> solids; //Any solids that this face is a part of.  Since we're in 3D, this should logically only ever be 1.
    public Mesh mesh;  
    
    public Face(List<Edge> edges)
    {

    }

    public void updateMesh()
    {
        mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        for (int i = 0; i < edges.Count; ++i)
        {
            //for (int j = 0; )
        }
    }
}
