using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTestTool : Tool {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    public override void Tap(Vector3 position)
    {
        new Point(position);
    }
}
