using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge {
    List<Point> points;
    List<Face> faces;

    bool isLoop;
    bool smooth;

    public Edge(List<Point> points, bool isLoop)
    {
        this.points = points;
    }
}
