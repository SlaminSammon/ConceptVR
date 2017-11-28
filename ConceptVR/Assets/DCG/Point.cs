using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : DCGElement {
    public List<Edge> edges;
    public Vector3 position;

    public Point(Vector3 position)
    {
        this.position = position;
        edges = new List<Edge>();
        DCGBase.points.Add(this);
    }

    public void setPosition(Vector3 value) {
        position = value;
        int moveID = DCGBase.nextMoveID();
        lastMoveID = moveID;

        foreach (Edge e in edges) if (e.lastMoveID != moveID)
        {
            e.lastMoveID = moveID;
            foreach (Face f in e.faces) if (f.lastMoveID != moveID)
            {
                f.lastMoveID = moveID;
                f.updateMesh();
                foreach(Solid s in f.solids) if (s.lastMoveID != moveID)
                {
                    s.lastMoveID = moveID;
                }
            }
        }
    }
}
