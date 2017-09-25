using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point {
    List<Edge> edges;
    public Vector3 position;

    public Point(Vector3 position)
    {
        this.position = position;
        DCGBase.points.Add(this);
    }
}
