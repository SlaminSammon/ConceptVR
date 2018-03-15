using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameButton : HUDButton {
    
    public HUDFrame targetFrame;

    public override void OnPress()
    {
        HUD.Pop();
        HUD.Push(targetFrame);
    }
}
