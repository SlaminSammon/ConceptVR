using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : DCGElement {
    public List<Edge> edges;
    public List<DCGElement> elements;   //elements this point is a part of
    public Vector3 position;

    public Point(Vector3 position)
    {
        this.position = position;
        edges = new List<Edge>();
        elements = new List<DCGElement>();
        DCGBase.points.Add(this);
    }

    public override void Render()
    {
        Graphics.DrawMeshNow(GeometryUtil.icoSphere2, Matrix4x4.TRS(this.position, Quaternion.identity, new Vector3(.007f, .007f, .007f)));
    }

    public override void Remove()
    {
        foreach (Edge e in edges)
            if (e.points[0] == this || e.points[e.points.Count-1] == this)
                e.Remove();
            else
                e.points.Remove(this);
        DCGBase.points.Remove(this);
    }

    public override float Distance(Vector3 position)
    {
        return Vector3.Distance(position, this.position);
    }
    
    public override List<Point> GetPoints() {
        List<Point> me = new List<Point>();
        me.Add(this);
        return me;
    }

    public override List<Point> Extrude()
    {
        Point p = new Point(position);
        new Edge(this, p);
        List<Point> list = new List<Point>();
        list.Add(p);
        return list;
    }

    public void setPosition(Vector3 value)
    {
        position = value;
        int moveID = DCGBase.nextMoveID();
        lastMoveID = moveID;

        foreach (Edge e in edges) if (e.lastMoveID != moveID)
            {
                e.lastMoveID = moveID;
                if (e.smooth)
                    e.updateMesh();
                foreach (Face f in e.faces) if (f.lastMoveID != moveID)
                    {
                        f.lastMoveID = moveID;
                        f.updateMesh();
                        foreach (Solid s in f.solids) if (s.lastMoveID != moveID)
                            {
                                s.lastMoveID = moveID;
                            }
                    }
            }

        foreach (DCGElement e in elements) if (e.lastMoveID != moveID)
            {
                e.lastMoveID = moveID;
                e.Update();
            }
    }

    public void Merge(Point that)
    {
        foreach (Edge e in that.edges)
            e.points[e.points.FindIndex(x => x == that)] = this;
        that.edges = new List<Edge>();
        that.Remove();
    }
}
