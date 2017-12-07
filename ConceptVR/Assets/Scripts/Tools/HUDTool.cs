using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDTool : Tool {
    HUDManager HUD;

    //TODO: change to PalmOpened
    public override void TriggerDown()
    {
        HUD.HUDObject.SetActive(true);
    }

    //TODO: change to PalmClosed
    public override void TriggerUp()
    {
        HUD.HUDObject.SetActive(false);
    }
}
