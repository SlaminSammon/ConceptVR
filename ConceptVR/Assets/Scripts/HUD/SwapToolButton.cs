using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapToolButton : ToggleButton {

	Controller controller;
    public string tool;
    public System.Type toolType;
    float cdTime;
    
    new void Start()
    {
        base.Start();
        controller = GameObject.Find("LoPoly_Rigged_Hand_Right").GetComponent<handController>();
    }

    public override void ToggleOn()
    {
        foreach (SwapToolButton b in transform.parent.GetComponentsInChildren<SwapToolButton>())
            if (b.toggled && b != this)
            {
                b.OnPress();
            }
        controller.changeTool(tool);
    }
}
