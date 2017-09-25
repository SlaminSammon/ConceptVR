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
            Graphics.DrawMeshNow(pointMesh, p.position, Quaternion.identity);
        }

        foreach(Solid s in solids)
        {
            Graphics.DrawMeshNow(s.mesh, transform.position, transform.rotation);
        }
    }
}
