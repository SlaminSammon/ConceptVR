﻿using System.Collections;
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
        Debug.Log(tool);
        if (controller == null)
        {
            controller = GameObject.Find("LoPoly_Rigged_Hand_Right").GetComponent<handController>();
        }

        controller.changeTool(tool);
        //ToggleOffAllOtherTools();
        
        foreach (SwapToolButton b in transform.parent.GetComponentsInChildren<SwapToolButton>())
            if (b.toggled && b != this)
            {
                b.OnPress();
            }
            
    }
    public override void OnPress()
    {
        GameObject go = GameObject.Find("Tools");
        if (go.transform.Find(tool).gameObject.activeSelf)
            return;
        base.OnPress();
    }

    private void ToggleOffAllOtherTools()
    {
        foreach (HUDFrame frame in GameObject.Find("Managers").GetComponent<HUDManager>().framesList)
        {
            foreach(SwapToolButton b in frame.GetComponentsInChildren<SwapToolButton>())
            {
                if (b.toggled && b != this)
                {
                    b.OnPress();
                }
            }
        }
    }
}
