using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTool : Tool {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool Tap(Vector3 position)
    {
        GameObject tapObject = new GameObject();
        tapObject.transform.position = position;
        tapObject.AddComponent<TapRenderer>();
        return true;
    }
}
