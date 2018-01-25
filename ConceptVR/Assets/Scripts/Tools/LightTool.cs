using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTool : Tool {
    
    public GameObject LightPrefab;

	// Use this for initialization
	void Start () {
        LightPrefab.GetComponent<Light>().color = LightPrefab.GetComponent<Material>().color;
	}
	
	// Update is called once per frame
	void Update () {

    }

    public override void Tap(Vector3 position)
    {
        LightPrefab.transform.position = controllerPosition;
        Instantiate(LightPrefab);
    }
}
