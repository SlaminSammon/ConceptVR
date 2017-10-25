using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge {
    public List<Point> points;
    public List<Face> faces;
    public int lastMoveID;

    public bool isLoop;
    bool smooth;

    public Edge(List<Point> points, bool isLoop)
    {
        this.isLoop = isLoop;
        this.points = points;
        this.faces = new List<Face>();
        foreach (Point p in points)
            p.edges.Add(this);
        DCGBase.edges.Add(this);
    }

    public Edge(Point p1, Point p2)
    {
        this.points = new List<Point>();
        this.points.Add(p1);
        this.points.Add(p2);
        this.faces = new List<Face>();
        foreach (Point p in points)
            p.edges.Add(this);
        DCGBase.edges.Add(this);
    }
}
