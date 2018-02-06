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
    private List<Vector3> finalPoints;
    private HandsUtil util;
    private int frameCount = 0;
    private LineRenderer rightFreeFormLine;
    private LineRenderer freeFormLine;
    private List<Point> backFacePoints;
    private List<Point> frontFacePoints;

    #endregion

    // Use this for initialization
    new void Start () {
		rightPoints = new List<Vector3> ();
		leftPoints = new List<Vector3> ();
        util = new HandsUtil();
        finalPoints = new List<Vector3>();
        backFacePoints = new List<Point>();
        frontFacePoints = new List<Point>();
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
                #region Line Renderer adding
                freeFormLine.positionCount++;
                freeFormLine.SetPosition(freeFormLine.positionCount-1, leftPos);
                rightFreeFormLine.positionCount++;
                rightFreeFormLine.SetPosition(rightFreeFormLine.positionCount-1, rightPos);
                rightPoints.Add(rightPos);
                leftPoints.Add(leftPos);
                #endregion
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
        
        Bezerp();
        generateFreeFormSolidCubic();
        
    }
    /*  Bezerp
     *  Input - none
     *  Output - Start & end Bezier curves
     *  Creates a minor curve connecting the start and end positions of a free form.
     */
    public void Bezerp()
    {
        #region Start Curve Bezier
        Vector3 virtL = leftPoints[1] + (leftPoints[1] - leftPoints[2]);
        Vector3 virtR = rightPoints[0] + (rightPoints[0] - rightPoints[1]);
        Vector3[] startVerts = { leftPoints[0], virtL, virtR, rightPoints[0] };

        rightPoints.Insert(0,GeometryUtil.Bezerp(startVerts,.75f));
        rightPoints.Insert(0, GeometryUtil.Bezerp(startVerts, .5f));
        rightPoints.Insert(0, GeometryUtil.Bezerp(startVerts, .25f));
        #endregion

        #region End Curve Bezier
        virtL = leftPoints[leftPoints.Count-1] + (leftPoints[leftPoints.Count - 1] - leftPoints[leftPoints.Count - 2]);
        virtR = rightPoints[rightPoints.Count - 1] + (rightPoints[rightPoints.Count - 1] - rightPoints[rightPoints.Count - 2]);
        Vector3[] endVerts = { leftPoints[leftPoints.Count - 1], virtL, virtR, rightPoints[rightPoints.Count - 1] };

        /*endCurve.Add(GeometryUtil.Bezerp(endVerts, .25f));
        endCurve.Add(GeometryUtil.Bezerp(endVerts, .5f));
        endCurve.Add(GeometryUtil.Bezerp(endVerts, .75f));*/
        #endregion

    }
    /*  generateFreeFormSolidCubic
     *  Input - none
     *  Output - Generated Solid
     *  Takes the drawn line made by the user, and turns it into a cubic solid
     */
    public void generateFreeFormSolidCubic()
    {
        /*
        //Vector3 midpoint =-
        if (startCurve.Count > 0)
            Debug.Log(startCurve.Count);
        #region Back Face Generation
        backFacePoints.Add(new Point(generateBackFacePoint(startCurve[1])));
        backFacePoints.Add(new Point(generateBackFacePoint(startCurve[2])));
        for (int i = 0; i < rightPoints.Count; ++i)
            backFacePoints.Add(new Point(generateBackFacePoint(rightPoints[i])));
        backFacePoints.Add(new Point(generateBackFacePoint(endCurve[0])));
        backFacePoints.Add(new Point(generateBackFacePoint(endCurve[1])));
        backFacePoints.Add(new Point(generateBackFacePoint(endCurve[2])));
        for (int i = leftPoints.Count-1; i > 0; --i)
            backFacePoints.Add(new Point(generateBackFacePoint(leftPoints[i])));
        backFacePoints.Add(new Point(generateBackFacePoint(startCurve[0])));

        List<Edge> backEdges = new List<Edge>();
        for(int i = 0; i < backFacePoints.Count-1; ++i)
        {
            backEdges.Add(new Edge(backFacePoints[i], backFacePoints[i + 1]));
        }
        backEdges.Add(new Edge(backFacePoints[backFacePoints.Count-1], backFacePoints[0]));
        new Face(backEdges);
        #endregion

        #region Front Face Generation
        frontFacePoints.Add(new Point(generateFrontFacePoint(startCurve[1])));
        frontFacePoints.Add(new Point(generateFrontFacePoint(startCurve[2])));
        for (int i = 0; i < rightPoints.Count; ++i)
            frontFacePoints.Add(new Point (generateFrontFacePoint(rightPoints[i])));
        frontFacePoints.Add(new Point(generateFrontFacePoint(endCurve[0])));
        frontFacePoints.Add(new Point(generateFrontFacePoint(endCurve[1])));
        frontFacePoints.Add(new Point(generateFrontFacePoint(endCurve[2])));
        for (int i = leftPoints.Count-1; i > 0; --i)
            frontFacePoints.Add(new Point(generateFrontFacePoint(leftPoints[i])));
        frontFacePoints.Add(new Point(generateFrontFacePoint(startCurve[0])));

        List<Edge> frontEdges = new List<Edge>();
        for (int i = 0; i < frontFacePoints.Count - 1; ++i)
        {
            frontEdges.Add(new Edge(frontFacePoints[i], frontFacePoints[i + 1]));
        }
        frontEdges.Add(new Edge(frontFacePoints[backFacePoints.Count - 1], frontFacePoints[0]));
        new Face(frontEdges);
        #endregion

        #region Side Edges and Face Generation
        List<Edge> sideEdges = new List<Edge>();
        for (int i = 0; i < frontFacePoints.Count ; ++i)
        {
            sideEdges.Add(new Edge(frontFacePoints[i], backFacePoints[i]));
        }
        List<Edge> tempEdges = new List<Edge>();
        List<Edge> tempList = new List<Edge>();
        for (int i =0; i < sideEdges.Count; ++i)
        {
            if (i == sideEdges.Count - 1)
            {
                
                tempList = new List<Edge>() { sideEdges[0], frontEdges[i], sideEdges[i], backEdges[i] };
                tempEdges.AddRange(tempList);
            }
            else
            {
                tempList = new List<Edge>() {  frontEdges[i], backEdges[i],sideEdges[i+1], sideEdges[i] };
                tempEdges.AddRange(tempList);
            }
                
        }
        Debug.Log("temp edges" + tempEdges.Count);
        new Face(tempEdges);
        #endregion
        */
    }
    public Vector3 generateBackFacePoint(Vector3 vec)
    { 
        return new Vector3(vec.x- (vec.x / 6), vec.y, (vec.z - (vec.z / 6)));
    }
    public Vector3 generateFrontFacePoint(Vector3 vec)
    {
        return new Vector3(vec.x + (vec.x / 6), vec.y, (vec.z+ (vec.z / 6)));
    }
    public Vector3 findCenter(List<Vector3> vecs)
    {
        float x = 0f;
        float y = 0f;
        float z = 0f;
        foreach(Vector3 v in vecs)
        {
            x += v.x;
            y += v.y;
            z += v.z;
        }
        return new Vector3(x / vecs.Count, y / vecs.Count, z / vecs.Count);
    }
}
