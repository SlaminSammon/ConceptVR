using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialButton : HUDButton {

    public DCGMaterial DCGMat;
    public Tool materialTool;
    
	// Use this for initialization
	void Start () {
        // set the color of the view of the button to the dcgmat
        //this.transform.Find("View").GetComponent<MeshRenderer>().material = DCGMat.mat;
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    public override void OnPress()
    {
        materialTool.GetComponent<MaterialTool>().changeMaterial(DCGMat);
        base.OnPress();
    }
}
