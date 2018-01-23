using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleTool : Tool {

    List<Point> rectanglePoints;
    Solid rectangle;
    Vector3 startPosition;

    private void Start()
    {
        rectanglePoints = new List<Point>();
        rectangle = new Solid();
        startPosition = new Vector3(0f, 0f, 0f);
    }

    void Update()
    {
        if (triggerInput)
        {
            UpdatePoints();
        }
    }

    public override void TriggerDown()
    {
        GenerateRectangle();
    }

    public override void TriggerUp()
    {

    }

    private void GenerateRectangle()
    {
        rectangle = new Solid();
        rectanglePoints = new List<Point>();
        startPosition = controllerPosition;

    }

    private void UpdatePoints()
    {
        Vector3 endPosition = controllerPosition;

        Point p1 = new Point(startPosition);
        Point p2 = new Point(new Vector3(endPosition.x, startPosition.y, startPosition.z));
        Point p3 = new Point(new Vector3(startPosition.x, endPosition.y, startPosition.z));
        Point p4 = new Point(new Vector3(endPosition.x, endPosition.y, startPosition.z));
        Point p5 = new Point(new Vector3(startPosition.x, startPosition.y, endPosition.z));
        Point p6 = new Point(new Vector3(endPosition.x, startPosition.y, endPosition.z));
        Point p7 = new Point(new Vector3(startPosition.x, endPosition.y, endPosition.z));
        Point p8 = new Point(endPosition);


        List<Face> faces = new List<Face>();
        List<Edge> edges = new List<Edge>();

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
}
