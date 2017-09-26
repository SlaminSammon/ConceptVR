using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCGBase : MonoBehaviour {
    public Mesh pointMesh;
    public Material pointMat;

    public static List<Point> points = new List<Point>();
    public static List<Edge> edges = new List<Edge>();
    public static List<Face> faces = new List<Face>();
    public static List<Solid> solids = new List<Solid>();

	// Use this for initialization
	void Start () {
        Mesh starterMesh = transform.Find("Starter").gameObject.GetComponent<MeshFilter>().mesh;
        
        new Solid(starterMesh);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnRenderObject()
    {
        foreach(Point p in points)
        {
            Graphics.DrawMesh(pointMesh, Matrix4x4.TRS(p.position, Quaternion.identity, new Vector3(.02f, .02f, .02f)), pointMat, 1);
            //Graphics.DrawMeshNow(pointMesh, p.position, Quaternion.identity);
        }

        foreach(Solid s in solids)
        {
            Graphics.DrawMesh(pointMesh, Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1f, 1f, 1f)), pointMat, 1);
            //Graphics.DrawMeshNow(s.mesh, transform.position, transform.rotation);
        }
    }
}
