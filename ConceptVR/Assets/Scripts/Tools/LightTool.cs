using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTool : Tool {
    
    public GameObject LightPrefab;

    GameObject itembase;

	// Use this for initialization
	void Start () {
        itembase = GameObject.Find("ItemBase");
        LightPrefab.GetComponent<Light>().color = LightPrefab.GetComponent<Material>().color;
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public override void Tap(Vector3 position)
    {
        LightPrefab.transform.position = controllerPosition;
        GameObject newLight = Instantiate(LightPrefab) as GameObject;
        newLight.name = "GeneratedPointLight_" + itembase.GetComponent<ItemBase>().lightCount;
        newLight.transform.parent = itembase.transform;
        itembase.GetComponent<ItemBase>().addLight(newLight);
    }
}
