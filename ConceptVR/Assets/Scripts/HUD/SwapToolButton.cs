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
        controller.changeTool(tool);
        foreach (SwapToolButton b in transform.parent.GetComponentsInChildren<SwapToolButton>())
            if (b.toggled && b != this)
            {
                b.OnPress();
            }
    }
    public override void OnPress()
    {
        GameObject go = GameObject.Find("Tools");
        Debug.Log("TOOL " + go.transform.Find(tool).gameObject.activeSelf);
        if (go.transform.Find(tool).gameObject.activeSelf)
            return;
        base.OnPress();
    }
}
