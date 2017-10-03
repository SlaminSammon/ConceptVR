using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCGBase : MonoBehaviour {
    public Mesh pointMesh;
    public Material pointMat;
    public Material edgeMat;
    public Material faceMat;
    public Material solidMat;

    public static List<Point> points = new List<Point>();
    public static List<Edge> edges = new List<Edge>();
    public static List<Face> faces = new List<Face>();
    public static List<Solid> solids = new List<Solid>();

    private static int moveID = 0;

	// Use this for initialization
	void Start () {
        Transform starter = transform.Find("Starter");
        Mesh starterMesh = starter.gameObject.GetComponent<MeshFilter>().mesh;
        
        new Solid(starterMesh, Matrix4x4.TRS(starter.position, starter.rotation, starter.localScale));
        Debug.Log(points.Count);

        starter.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        points[1].setPosition(points[1].position + new Vector3(0f, 0f, Mathf.Cos(Time.time) * Time.deltaTime / 10f));
	}

    private void OnRenderObject()
    {
        pointMat.SetPass(0);
        foreach (Point p in points)
        {
            //Graphics.DrawMeshNow(pointMesh, Matrix4x4.TRS(p.position, Quaternion.identity, new Vector3(.02f, .02f, .02f)), pointMat, 1);
            Graphics.DrawMeshNow(pointMesh, Matrix4x4.TRS(p.position, Quaternion.identity, new Vector3(.01f, .01f, .01f)));
        }

        edgeMat.SetPass(0);
        Mesh m = new Mesh();
        Vector3[] verts = new Vector3 [4];
        m.SetVertices(new List<Vector3>(verts));
        m.SetTriangles(new int[] { 0, 1, 2, 1, 2, 3 }, 0);

        foreach (Edge e in edges)
        {
            Vector3 left = Vector3.Cross(e.points[0].position - Camera.main.transform.position, e.points[0].position - e.points[e.points.Count - 1].position).normalized * .002f;
            verts[0] = e.points[0].position + left;
            verts[1] = e.points[0].position - left;
            verts[2] = e.points[e.points.Count - 1].position + left;
            verts[3] = e.points[e.points.Count - 1].position - left;
            m.SetVertices(new List<Vector3>(verts));
            Graphics.DrawMeshNow(m, Vector3.zero, Quaternion.identity);
        }

        faceMat.SetPass(0);
        foreach(Face f in faces)
        {
            Graphics.DrawMeshNow(f.mesh, Vector3.zero, Quaternion.identity);
        }

        solidMat.SetPass(0);
        foreach (Solid s in solids)
        {
            //Graphics.DrawMeshNow(pointMesh, Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1f, 1f, 1f)), pointMat, 1);
            Graphics.DrawMeshNow(s.mesh, transform.position, transform.rotation);
        }
    }

    public static int nextMoveID()
    {
        return moveID++;
    }
}
