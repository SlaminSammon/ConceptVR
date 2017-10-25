using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCGBase : MonoBehaviour {
    public Mesh pointMesh;
    public Mesh edgeMesh;
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
        
        new Solid(starterMesh, Matrix4x4.TRS(starter.position, starter.rotation, starter.localScale), starter.position);
        Debug.Log(points.Count);

        starter.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        
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

        foreach (Edge e in edges)
        {
            Vector3 edgeVec;
            for (int i = 0; i < e.points.Count-1; ++i)
            {
                edgeVec = e.points[i].position - e.points[i+1].position;
                Graphics.DrawMeshNow(edgeMesh, Matrix4x4.TRS(e.points[i].position - edgeVec / 2, Quaternion.FromToRotation(Vector3.up, edgeVec), new Vector3(.005f, edgeVec.magnitude / 2, .005f)));
            }
            if (e.isLoop)
            {
                edgeVec = e.points[e.points.Count - 1].position - e.points[0].position;
                Graphics.DrawMeshNow(edgeMesh, Matrix4x4.TRS(e.points[0].position + edgeVec / 2, Quaternion.FromToRotation(Vector3.up, edgeVec), new Vector3(.005f, edgeVec.magnitude / 2, .005f)));
            }
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
