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

        starter.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnRenderObject()
    {
        //Render top-level DCG Elements
        solidMat.SetPass(0);
        foreach (Solid s in solids)
            s.Render();
        foreach (Face f in faces)
            if (f.solids.Count == 0 && !f.isSelected)
                f.Render();
        foreach (Edge e in edges)
            if (e.faces.Count == 0 && !e.isSelected)
                e.Render();
        foreach (Point p in points)
            if (p.edges.Count == 0 && !p.isSelected)
                p.Render();


        //Render highlights
        pointMat.SetPass(0);
        foreach (Point p in points)
            if (p.edges.Count > 0 && !p.isSelected)
                p.Render();

        edgeMat.SetPass(0);
        foreach (Edge e in edges)
            if (e.faces.Count > 0 && !e.isSelected)
                e.Render();
        
        faceMat.SetPass(0);
        foreach (Face f in faces)
            if (f.solids.Count > 0 && !f.isSelected)
                f.Render();
    }

    public static int nextMoveID()
    {
        return moveID++;
    }

    public static Point NearestPoint(Vector3 pos, float maxDist)
    {
        float nDist2 = maxDist * maxDist;
        Point nPoint = null;
        foreach (Point p in DCGBase.points)
        {
            float dist2 = (pos - p.position).sqrMagnitude;
            if (dist2 < nDist2)
            {
                nPoint = p;
                nDist2 = dist2;
            }
        }

        return nPoint;
    }

    public static Edge NearestEdge(Vector3 pos, float maxDist)
    {
        float nDist2 = maxDist * maxDist;
        Edge nEdge = null;
        foreach (Edge e in DCGBase.edges)
        {
            for (int i = 0; i < (e.isLoop ? e.points.Count : e.points.Count - 1); ++i)
            {
                Vector3 ediff = (e.points[i].position - e.points[(i + 1) % e.points.Count].position);
                Vector3 hdiff = (pos - e.points[i].position);
                Vector3 proj = Vector3.Project(hdiff, ediff);
                float dist2 = (hdiff - proj).sqrMagnitude;
                if (dist2 < nDist2 && hdiff.sqrMagnitude < ediff.sqrMagnitude && (pos - e.points[(i + 1) % e.points.Count].position).sqrMagnitude < ediff.sqrMagnitude)
                {
                    nEdge = e;
                    nDist2 = dist2;
                }
            }
        }

        return nEdge;
    }

    public static Face NearestFace(Vector3 pos, float maxDist)
    {
        float nDist2 = maxDist * maxDist;
        Face nFace = null;
        foreach (Face f in DCGBase.faces)
        {
            if (f.edges.Count == 0)
                continue;
            List<Point> fp = f.getPoints();   //face points
            List<Vector2> pp = new List<Vector2>();     //projected points
            Vector3 zvec = f.getNormal();
            Vector3 yvec = fp[1].position - fp[0].position;
            Vector3 grabProj3 = Vector3.ProjectOnPlane(pos, zvec);
            Vector3 grabProjY = Vector3.Project(grabProj3, yvec);
            Vector2 grab = new Vector2((grabProj3 - grabProjY).magnitude, grabProjY.magnitude);

            Vector3 grabRel = pos - fp[0].position;
            float dist2 = (grabRel - Vector3.ProjectOnPlane(grabRel, zvec)).sqrMagnitude;

            if (dist2 > nDist2)
                continue;

            foreach (Point p in fp)
            {
                Vector3 proj3 = Vector3.ProjectOnPlane(p.position, zvec);
                Vector3 projY = Vector3.Project(proj3, yvec);
                pp.Add(new Vector2((proj3 - projY).magnitude, projY.magnitude));
            }


            //Check if the projected poly contains the projected controller
            float count = 0;
            Vector2 prev = pp[pp.Count - 1];
            foreach (Vector2 cur in pp)
            {
                if ((prev.y > grab.y != cur.y > grab.y) && (grab.x < Mathf.Lerp(prev.x, cur.x, (grab.y - prev.y) / (cur.y - prev.y))))
                    count++;
                prev = cur;
            }


            if (count % 2 == 1)
            {
                nDist2 = dist2;//TODO
                nFace = f;
            }
        }

        return nFace;
    }

}
