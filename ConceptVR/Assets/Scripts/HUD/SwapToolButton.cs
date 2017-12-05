using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapToolButton : HUDButton {

	Controller controller;
    public string tool;
    public System.Type toolType;
    float cdTime;
    
    new void Start()
    {
        base.Start();
        controller = GameObject.Find("LoPoly_Rigged_Hand_Right").GetComponent<handController>();
    }

    public override void OnPress()
    {
        controller.changeTool(tool);
        HUD.updateToolButtonColor(tool);
        HUD.setCooldownTime(Time.time + HUD.getCooldown());
    }
}
