using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameToolButton : SwapToolButton {
    public HUDFrame targetFrame;

    public bool isSubFrameButton = true;
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
        HUD.Push(targetFrame);
        base.OnPress();
    }
}
