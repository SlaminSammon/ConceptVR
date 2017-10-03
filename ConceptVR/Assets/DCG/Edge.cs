using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge {
    public List<Point> points;
    public List<Face> faces;

    bool isLoop;
    bool smooth;

    public Edge(List<Point> points, bool isLoop)
    {
        this.points = points;
        DCGBase.edges.Add(this);
    }

    public Edge(Point p1, Point p2)
    {
        this.points = new List<Point>();
        this.points.Add(p1);
        this.points.Add(p2);
        DCGBase.edges.Add(this);
    }
}
