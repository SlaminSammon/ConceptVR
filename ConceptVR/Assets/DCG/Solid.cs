using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solid {
    public List<Face> faces;
    public Mesh mesh;


    public Solid(Mesh m)
    {
        mesh = m;
        foreach(Vector3 p in mesh.vertices)
        {
            new Point(p);
        }

        DCGBase.solids.Add(this);
    }
}
