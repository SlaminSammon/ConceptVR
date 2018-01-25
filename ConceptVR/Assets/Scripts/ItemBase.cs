using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour {

    public List<GameObject> lights;
    public int lightCount;

	// Use this for initialization
	void Start () {
        lightCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addLight(GameObject newLight)
    {
        lights.Add(newLight);
        lightCount++;
    }

}
