using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class FreeFormTool : Tool {
    #region Member Berries
    private Vector3 startPos;
	private Vector3 endPos;
	private List<Vector3> rightPoints;
	private List<Vector3> leftPoints;
    private HandsUtil util;
    private int frameCount = 0;
    private LineRenderer rightFreeFormLine;
    private LineRenderer freeFormLine;
    private List<Vector3> startCurve;
    private List<Vector3> endCurve;
    private List<Vector3> backFacePoints;
    private List<Vector3> frontFacePoints;
    #endregion

    // Use this for initialization
    new void Start () {
		rightPoints = new List<Vector3> ();
		leftPoints = new List<Vector3> ();
        util = new HandsUtil();
        startCurve = new List<Vector3>();
        endCurve = new List<Vector3>();
    }
	
	// Update is called once per frame
	new void Update () {
        if (formInput)
        {

            if (frameCount >= 25)
            {
                Debug.Log(frameCount);
                //Add points to the line renderer and the point lists
                Vector3 rightPos = Hands.Right.PalmPosition.ToVector3();
                Vector3 leftPos = Hands.Left.PalmPosition.ToVector3();
                if (leftPos == null || rightPos == null)
                    return;
                freeFormLine.positionCount++;
                freeFormLine.SetPosition(freeFormLine.positionCount-1, leftPos);
                rightFreeFormLine.positionCount++;
                rightFreeFormLine.SetPosition(rightFreeFormLine.positionCount-1, rightPos);
                rightPoints.Add(rightPos);
                leftPoints.Add(leftPos);
                Debug.Log("Bacon");
                frameCount = 0;
            }
            else frameCount++;
        }
	}
    public override void FreeForm()
    {
        Vector3 rightPos = Hands.Right.PalmPosition.ToVector3();
        Vector3 leftPos = Hands.Left.PalmPosition.ToVector3();
        #region Line Renderer Initialize
        GameObject go = new GameObject();
        go.transform.position = rightPos;
        rightFreeFormLine = go.AddComponent<LineRenderer>();
        rightFreeFormLine.startWidth = .01f;
        rightFreeFormLine.endWidth = .01f;
        GameObject gol = new GameObject();
        gol.transform.position = leftPos;
        freeFormLine = gol.AddComponent<LineRenderer>();
        freeFormLine.startWidth = .01f;
        freeFormLine.endWidth = .01f;
        #endregion
        //Set all vars with the new pos
        freeFormLine.positionCount++;
        rightFreeFormLine.SetPosition(0, rightPos);
        freeFormLine.SetPosition(0, leftPos);
        rightPoints.Add(rightPos);
        leftPoints.Add(leftPos);
    }
    public override void FreeFormEnd()
    {
        /*
        Bezerp();
        List<Point> rPoints = new List<Point>();
        List<Point> lPoints = new List<Point>();
        for (int i = 0; i < rightPoints.Count; ++i)
            rPoints.Add(new Point(rightPoints[i]));
        for (int i = 0; i < leftPoints.Count; ++i)
            lPoints.Add(new Point(leftPoints[i]));
        List<Edge> edges = new List<Edge>();
        edges.Add(new Edge(startCurve[1], startCurve[2]));
        edges.Add(new Edge(startCurve[2], rPoints[0]));
        for (int p = 0; p < rPoints.Count - 1; ++p)
            edges.Add(new Edge(rPoints[p], rPoints[p + 1]));
        edges.Add(new Edge(rPoints[rPoints.Count - 1], endCurve[2]));
        edges.Add(new Edge(endCurve[2], endCurve[1]));
        edges.Add(new Edge(endCurve[1], endCurve[0]));
        edges.Add(new Edge(lPoints[lPoints.Count-1], endCurve[0]));
        for (int p = lPoints.Count-1; p > 0; --p)
            edges.Add(new Edge(lPoints[p], lPoints[p - 1]));
        edges.Add(new Edge(lPoints[0], startCurve[0]));
        edges.Add(new Edge(startCurve[0], startCurve[1]));
        new Face(edges);
        Destroy(freeFormLine.gameObject);
        Destroy(rightFreeFormLine.gameObject);
        Debug.Log("Endign FreeForm");
        startCurve.Clear();
        endCurve.Clear();
        */
    }
    public void Bezerp()
    {
        Vector3 virtL = leftPoints[1] + (leftPoints[1] - leftPoints[2]);
        Vector3 virtR = rightPoints[0] + (rightPoints[0] - rightPoints[1]);
        Vector3[] startVerts = { leftPoints[0], virtL, virtR, rightPoints[0] };

        startCurve.Add(GeometryUtil.Bezerp(startVerts,.25f));
        startCurve.Add(GeometryUtil.Bezerp(startVerts, .5f));
        startCurve.Add(GeometryUtil.Bezerp(startVerts, .75f));

        virtL = leftPoints[leftPoints.Count-1] + (leftPoints[leftPoints.Count - 1] - leftPoints[leftPoints.Count - 2]);
        virtR = rightPoints[rightPoints.Count - 1] + (rightPoints[rightPoints.Count - 1] - rightPoints[rightPoints.Count - 2]);
        Vector3[] endVerts = { leftPoints[leftPoints.Count - 1], virtL, virtR, rightPoints[rightPoints.Count - 1] };
        endCurve.Add(GeometryUtil.Bezerp(endVerts, .25f));
        endCurve.Add(GeometryUtil.Bezerp(endVerts, .5f));
        endCurve.Add(GeometryUtil.Bezerp(endVerts, .75f));

    }
    public void generateFaceLists()
    {
        backFacePoints.Add(generateBackFacePoint(startCurve[1]));
        backFacePoints.Add(generateBackFacePoint(startCurve[2]));
        for (int i = 0; i < rightPoints.Count; ++i)
            backFacePoints.Add(generateBackFacePoint(rightPoints[i]));
        backFacePoints.Add(generateBackFacePoint(endCurve[0]));
        backFacePoints.Add(generateBackFacePoint(endCurve[1]));
        backFacePoints.Add(generateBackFacePoint(endCurve[2]));
        for (int i = leftPoints.Count; i > 0; --i)
            backFacePoints.Add(generateBackFacePoint(leftPoints[i]));
        backFacePoints.Add(generateBackFacePoint(startCurve[0]));

        frontFacePoints.Add(generateFrontFacePoint(startCurve[1]));
        frontFacePoints.Add(generateFrontFacePoint(startCurve[2]));
        for (int i = 0; i < rightPoints.Count; ++i)
            frontFacePoints.Add(generateFrontFacePoint(rightPoints[i]));
        frontFacePoints.Add(generateFrontFacePoint(endCurve[0]));
        frontFacePoints.Add(generateFrontFacePoint(endCurve[1]));
        frontFacePoints.Add(generateFrontFacePoint(endCurve[2]));
        for (int i = leftPoints.Count; i > 0; --i)
            frontFacePoints.Add(generateFrontFacePoint(leftPoints[i]));
        frontFacePoints.Add(generateFrontFacePoint(startCurve[0]));
    }
    public Vector3 generateBackFacePoint(Vector3 vec)
    {
        return new Vector3((vec.x - (vec.x / 2)), vec.y, (vec.z - (vec.z / 2)));
    }
    public Vector3 generateFrontFacePoint(Vector3 vec)
    {
        return new Vector3((vec.x + (vec.x / 2)), vec.y, (vec.z + (vec.z / 2)));
    }
}
