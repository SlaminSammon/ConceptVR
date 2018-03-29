using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeClock : HUDButton {

    public override void OnPress()
    {
        HUD.changeClock();
    }
}

