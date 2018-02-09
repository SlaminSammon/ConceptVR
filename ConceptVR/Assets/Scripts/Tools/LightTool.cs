using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTool : Tool {
    
    public GameObject LightPrefab;

    ItemBase itemBase;

	// Use this for initialization
	void Start () {
        itemBase = GameObject.Find("ItemBase").GetComponent<ItemBase>();
        //LightPrefab.GetComponent<Light>().color = LightPrefab.GetComponent<Material>().color;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public override bool Tap(Vector3 position)
    {
        GameObject obj = Instantiate(ItemBase.LightPrefab);
        obj.AddComponent<LightItem>();
        obj.transform.position = position;
        ItemBase.items.Add(obj.GetComponent<LightItem>());
        return true;
    }
}
