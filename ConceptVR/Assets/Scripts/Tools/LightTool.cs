using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTool : Tool {
    
    public GameObject LightPrefab;

    ItemBase itemBase;
    public GameObject Managers;

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
        Debug.Log("Generate Light");
        GameObject obj = Instantiate(itemBase.LightPrefab);
        obj.AddComponent<LightItem>();
        obj.transform.position = position;
        float playerScale = Managers.GetComponent<SettingsManager>().playerScale;
        obj.transform.localScale = new Vector3(playerScale, playerScale, playerScale)/50;
        ItemBase.items.Add(obj.GetComponent<LightItem>());
        return true;
    }
}
