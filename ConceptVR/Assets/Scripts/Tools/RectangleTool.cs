using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleTool : Tool {

    List<Point> rectanglePoints;
    Solid rectangle;
    Vector3 startPosition;
    List<Face> faces;
    List<Vector3> verts;

    private void Start()
    {
        faces = new List<Face>();
        rectanglePoints = new List<Point>();
        rectangle = new Solid();
        startPosition = new Vector3(0f, 0f, 0f);
    }

    void Update()
    {
        if (triggerInput)
        {
            UpdateVerts();
        }
    }

    public override void TriggerDown()
    {
        rectangle = new Solid();
        rectanglePoints = new List<Point>();
        startPosition = controllerPosition;
    }

    public override void TriggerUp()
    {
        GenerateRectangle();
        verts.Clear();
        faces.Clear();
    }

    private void GenerateRectangle()
    {
        faces = new List<Face>();
        List<Edge> edges = new List<Edge>();
        Point p1 = new Point(verts[0]);
        Point p2 = new Point(verts[1]);
        Point p3 = new Point(verts[2]);
        Point p4 = new Point(verts[3]);
        Point p5 = new Point(verts[4]);
        Point p6 = new Point(verts[5]);
        Point p7 = new Point(verts[6]);
        Point p8 = new Point(verts[7]);

        // front face
        edges.Add(new Edge(p1, p2));
        edges.Add(new Edge(p1, p3));
        edges.Add(new Edge(p2, p4));
        edges.Add(new Edge(p3, p4));
        faces.Add(new Face(edges));
        edges.Clear();

        // back face
        edges.Add(new Edge(p5, p6));
        edges.Add(new Edge(p5, p7));
        edges.Add(new Edge(p6, p8));
        edges.Add(new Edge(p7, p8));
        faces.Add(new Face(edges));
        edges.Clear();

        // left face
        edges.Add(new Edge(p1, p3));
        edges.Add(new Edge(p1, p5));
        edges.Add(new Edge(p3, p7));
        edges.Add(new Edge(p5, p7));
        faces.Add(new Face(edges));
        edges.Clear();

        // right face
        edges.Add(new Edge(p2, p4));
        edges.Add(new Edge(p2, p6));
        edges.Add(new Edge(p4, p8));
        edges.Add(new Edge(p6, p8));
        faces.Add(new Face(edges));
        edges.Clear();

        // top face
        edges.Add(new Edge(p1, p2));
        edges.Add(new Edge(p1, p5));
        edges.Add(new Edge(p2, p6));
        edges.Add(new Edge(p5, p6));
        faces.Add(new Face(edges));
        edges.Clear();

        // bottom face
        edges.Add(new Edge(p3, p4));
        edges.Add(new Edge(p3, p7));
        edges.Add(new Edge(p4, p8));
        edges.Add(new Edge(p7, p8));
        faces.Add(new Face(edges));
        edges.Clear();

        rectangle = new Solid(faces);
    }

    private void UpdateVerts()
    {
        verts = new List<Vector3>();
        Vector3 endPosition = controllerPosition;

        verts.Add(startPosition);
        verts.Add(new Vector3(endPosition.x, startPosition.y, startPosition.z));
        verts.Add(new Vector3(startPosition.x, endPosition.y, startPosition.z));
        verts.Add(new Vector3(endPosition.x, endPosition.y, startPosition.z));
        verts.Add(new Vector3(startPosition.x, startPosition.y, endPosition.z));
        verts.Add(new Vector3(endPosition.x, startPosition.y, endPosition.z));
        verts.Add(new Vector3(startPosition.x, endPosition.y, endPosition.z));
        verts.Add(endPosition);
    } 
}
