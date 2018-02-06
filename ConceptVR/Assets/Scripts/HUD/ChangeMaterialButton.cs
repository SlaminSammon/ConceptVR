using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialButton : HUDButton {

    public Material material;
    public Tool materialTool;
	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    public override void OnPress()
    {
        materialTool.GetComponent<MaterialTool>().changeMaterial(material);
        base.OnPress();
    }
}
