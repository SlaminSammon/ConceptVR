using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeometryUtil
{
    public static Mesh icoSphere2 = GenerateSphereMesh(2);
    public static Mesh icoSphere3 = GenerateSphereMesh(3);
    public static Mesh icoSphere4 = GenerateSphereMesh(4);

    public static Mesh cylinder8 = GenerateCylinderMesh(8);
    public static Mesh cylinder16 = GenerateCylinderMesh(16);
    public static Mesh cylinder32 = GenerateCylinderMesh(32);
    public static Mesh cylinder64 = GenerateCylinderMesh(64);



    public static void MinimizeMesh(Mesh mesh)
    {
        List<Vector3> pos = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        mesh.GetVertices(pos);
        mesh.GetNormals(normals);
        int[] tris = mesh.GetTriangles(0);

        int[] map = new int[pos.Count];
        List<Vector3> nPosList = new List<Vector3>();

        for (int i = 0; i < pos.Count; ++i)
        {
            bool match = false;
            int j = 0;
            foreach (Vector3 v in nPosList)
            {
                if (pos[i] == v)
                {
                    map[i] = j;
                    match = true;
                }
                ++j;
            }

            if (!match)
            {
                nPosList.Add(pos[i]);
                map[i] = nPosList.Count - 1;
            }
        }

        for (int i = 0; i < tris.Length; ++i)
        {
            tris[i] = map[tris[i]];
        }

        //mesh.SetNormals();    //TODO: set mesh stuff, do normals
    }

    static void subdivide(List<Vector3> verts, List<int> tris, int v1, int v2, int v3, int depth)
    {
        if (depth > 0)
        {
            int b = verts.Count;
            //Debug.Log(b + " " + v1);
            verts.Add((verts[v1] + verts[v2]).normalized);
            verts.Add((verts[v2] + verts[v3]).normalized);
            verts.Add((verts[v3] + verts[v1]).normalized);

            subdivide(verts, tris, v1, b, b + 2, depth - 1);
            subdivide(verts, tris, b, v2, b + 1, depth - 1);
            subdivide(verts, tris, b + 2, b + 1, v3, depth - 1);
            subdivide(verts, tris, b, b + 1, b + 2, depth - 1);
        }
        else
        {
            tris.Add(v1);
            tris.Add(v2);
            tris.Add(v3);
        }
    }

    public static Mesh GenerateSphereMesh(int depth)
    {
        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

        List<int> tris = new List<int>(20 * Mathf.FloorToInt(Mathf.Pow(4, depth)));
        List<Vector3> verts = new List<Vector3>(12 * Mathf.FloorToInt(Mathf.Pow(2, depth)));

        verts.Add(new Vector3(-1, t, 0).normalized);
        verts.Add(new Vector3(1, t, 0).normalized);
        verts.Add(new Vector3(-1, -t, 0).normalized);
        verts.Add(new Vector3(1, -t, 0).normalized);

        verts.Add(new Vector3(0, -1, t).normalized);
        verts.Add(new Vector3(0, 1, t).normalized);
        verts.Add(new Vector3(0, -1, -t).normalized);
        verts.Add(new Vector3(0, 1, -t).normalized);

        verts.Add(new Vector3(t, 0, -1).normalized);
        verts.Add(new Vector3(t, 0, 1).normalized);
        verts.Add(new Vector3(-t, 0, -1).normalized);
        verts.Add(new Vector3(-t, 0, 1).normalized);

        subdivide(verts, tris, 0, 11, 5, depth);
        subdivide(verts, tris, 0, 5, 1, depth);
        subdivide(verts, tris, 0, 1, 7, depth);
        subdivide(verts, tris, 0, 7, 10, depth);
        subdivide(verts, tris, 0, 10, 11, depth);

        subdivide(verts, tris, 1, 5, 9, depth);
        subdivide(verts, tris, 5, 11, 4, depth);
        subdivide(verts, tris, 11, 10, 2, depth);
        subdivide(verts, tris, 10, 7, 6, depth);
        subdivide(verts, tris, 7, 1, 8, depth);

        subdivide(verts, tris, 3, 9, 4, depth);
        subdivide(verts, tris, 3, 4, 2, depth);
        subdivide(verts, tris, 3, 2, 6, depth);
        subdivide(verts, tris, 3, 6, 8, depth);
        subdivide(verts, tris, 3, 8, 9, depth);

        subdivide(verts, tris, 4, 9, 5, depth);
        subdivide(verts, tris, 2, 4, 11, depth);
        subdivide(verts, tris, 6, 2, 10, depth);
        subdivide(verts, tris, 8, 6, 7, depth);
        subdivide(verts, tris, 9, 8, 1, depth);

        Mesh m = new Mesh();
        m.SetVertices(verts);
        m.SetTriangles(tris, 0);
        m.SetNormals(verts);
        m.RecalculateBounds();
        m.RecalculateTangents();

        return m;
    }

    public static Mesh GenerateCylinderMesh(int detail)
    {
        List<int> tris = new List<int>();
        List<Vector3> verts = new List<Vector3>();

        for (int i = 0; i < detail; ++i)
        {
            float theta = (float)i / detail * Mathf.PI * 2;
            verts.Add(new Vector3(Mathf.Cos(theta), -1, Mathf.Sin(theta)));
            verts.Add(new Vector3(Mathf.Cos(theta), 1, Mathf.Sin(theta)));

            int a = i * 2;
            int b = ((i + 1) % detail) * 2;
            tris.Add(a); tris.Add(b + 1); tris.Add(b);
            tris.Add(b + 1); tris.Add(a); tris.Add(a + 1);
            tris.Add(a); tris.Add(b); tris.Add(detail * 2);
            tris.Add(a + 1); tris.Add(b + 1); tris.Add(detail * 2 + 1);
        }

        verts.Add(new Vector3(0, -1, 0));
        verts.Add(new Vector3(0, 1, 0));

        Mesh m = new Mesh();
        m.SetVertices(verts);
        m.SetTriangles(tris, 0);
        m.RecalculateNormals();
        m.RecalculateBounds();
        m.RecalculateTangents();

        return m;
    }


    //WARNING: will hang i given a self-intersecting polygon
    public static List<int> triangulate(List<Vector3> points, Vector3 normal)
    {
        int c = points.Count;
        int i;
        List<int> triangles = new List<int>();
        bool[] concave = new bool[c];
        bool[] consumed = new bool[c];
        int consumedCount = 0;
        int concaveCount = 0;

        for (i = 0; i < c; ++i)
        {
            bool isConcave = Vector3.Angle(normal, Vector3.Cross(points[(i + 2) % c] - points[(i + 1) % c], points[(i + 1) % c] - points[i])) < 90;
            concave[(i + 1) % c] = isConcave;
            if (isConcave)
                ++concaveCount;
        }

        i = 0;
        int i1 = 1;
        int i2 = 2;
        while (consumedCount < c - 3)
        {
            i1 = (i + 1) % c;
            while (consumed[i1])
                i1 = (i1 + 1) % c;

            i2 = (i1 + 1) % c;
            while (consumed[i2])
                i2 = (i2 + 1) % c;

            Vector3 a = points[i];
            Vector3 b = points[i1];
            Vector3 g = points[i2];

            if (Vector3.Angle(normal, Vector3.Cross(b - a, g - b)) < 90)
            {
                bool bad = false;
                for (int j = 0; j < c; ++j) if (j != i && j != i1 && j != i2 && concave[j] && !consumed[j])
                    {
                        float signb = Mathf.Sign(Vector3.Dot(Vector3.Cross(normal, b - a), b - points[j]));
                        float signg = Mathf.Sign(Vector3.Dot(Vector3.Cross(normal, g - b), g - points[j]));
                        float signa = Mathf.Sign(Vector3.Dot(Vector3.Cross(normal, a - g), a - points[j]));

                        if (signa < 0 || signb < 0 || signg < 0)
                        {
                            bad = true;
                            break;
                        }
                    }

                if (!bad)
                {
                    triangles.Add(i); triangles.Add(i1); triangles.Add(i2);
                    consumed[i1] = true;
                    consumedCount++;
                    i = i2;
                }
            }
            else
            {
                i = i1;
            }
        }

        i1 = (i + 1) % c;
        while (consumed[i1])
            i1 = (i1 + 1) % c;

        i2 = (i1 + 1) % c;
        while (consumed[i2])
            i2 = (i2 + 1) % c;

        triangles.Add(i); triangles.Add(i1); triangles.Add(i2);

        return triangles;
    }

    //
    public static List<int> dumbTriangulate(List<Vector3> points)
    {
        List<int> tris = new List<int>();
        for (int i = 1; i < points.Count - 1; ++i)
        {
            tris.Add(0); tris.Add(i); tris.Add(i + 1);
        }

        return tris;
    }

    //Not the worst I guess, 3D makes it make sense...ish
    public static List<int> mediocreTriangulate(List<Vector3> points)
    {
        int c = points.Count;
        List<int> triangles = new List<int>();
        bool[] consumed = new bool[c];
        int consumedCount = 0;

        int i = 0;
        int i1 = 1;
        int i2 = 2;
        while (consumedCount < c - 3)
        {
            i1 = (i + 1) % c;
            while (consumed[i1])
                i1 = (i1 + 1) % c;

            i2 = (i1 + 1) % c;
            while (consumed[i2])
                i2 = (i2 + 1) % c;

            triangles.Add(i); triangles.Add(i1); triangles.Add(i2);
            consumed[i1] = true;
            consumedCount++;
            i = i2;

        }


        i1 = (i + 1) % c;
        while (consumed[i1])
            i1 = (i1 + 1) % c;

        i2 = (i1 + 1) % c;
        while (consumed[i2])
            i2 = (i2 + 1) % c;

        triangles.Add(i); triangles.Add(i1); triangles.Add(i2);

        return triangles;
    }
    
    public static Vector3 Bezerp(Vector3[] control, float t)
    {
        Vector3[] nControl = new Vector3[control.Length - 1];
        for (int i = 0; i < nControl.Length; ++i)
        {
            nControl[i] = Vector3.Lerp(control[i], control[i + 1], t);
        }

        if (nControl.Length == 1)
            return nControl[0];
        else
            return Bezerp(nControl, t);
    }

    /*public static Vector3[] ReControlBezier(Vector3[] control, int count)
    {

    }*/
}
