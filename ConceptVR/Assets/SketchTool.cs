using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SketchTool : Tool {
    public GameObject linePrefab;
    
    LineRenderer currentLine;
    List<Vector3> currentPositions;

    const float circleSnap = 0.05f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (triggerInput)
        {
            currentPositions.Add(transform.position);
            currentLine.positionCount = currentPositions.Count;
            currentLine.SetPositions(currentPositions.ToArray());
        }
	}

    public override void TriggerDown()
    {
        currentPositions = new List<Vector3>();
        currentLine = Instantiate(linePrefab).GetComponent<LineRenderer>();
    }

    public override void TriggerUp()
    {
        if (Vector3.Distance(currentPositions[0], currentPositions[currentPositions.Count - 1]) < circleSnap)
        {
            Vector3 avgPos = new Vector3(0f, 0f, 0f);
            Vector3 avgNormal = new Vector3(0f, 0f, 0f);
            Vector3 prevSegment = currentPositions[currentPositions.Count-1] - currentPositions[0];
            Vector3 prevPos = currentPositions[currentPositions.Count-1];
            for(int i = 0; i < currentPositions.Count; ++i)
            {
                Vector3 v = currentPositions[i];
                avgPos += v;
                Vector3 segment = v - prevPos;

                avgNormal = Vector3.Cross(prevSegment.normalized, segment.normalized).normalized;
                Debug.Log(Vector3.Cross(prevSegment.normalized, segment.normalized).normalized);

                prevSegment = segment;
                prevPos = v;
            }
            avgPos /= currentPositions.Count;
            avgNormal.Normalize();

            float avgRadius = 0f;
            foreach (Vector3 v in currentPositions)
                avgRadius += Vector3.Distance(v, avgPos);
            avgRadius /= currentPositions.Count;
            
            Vector3[] circlePoints = new Vector3[60];
            for (int i = 0; i < 60; ++i)
            {
                circlePoints[i] = avgPos + Quaternion.AngleAxis(i * 6f, avgNormal) * (Vector3.Cross(Vector3.up, avgNormal) * avgRadius);
            }

            currentLine.positionCount = 60;
            currentLine.loop = true;
            currentLine.SetPositions(circlePoints);
        }
        else
        {
            currentLine.positionCount = currentPositions.Count;
            currentLine.SetPositions(currentPositions.ToArray());
        }
    }
}
