using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFormTool : Tool {
	private Vector3 startPos;
	private Vector3 endPos;
	private List<Vector3> rightPoints;
	private List<Vector3> leftPoints;

	// Use this for initialization
	void Start () {
		rightPoints = new List<Vector3> ();
		leftPoints = new List<Vector3> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
