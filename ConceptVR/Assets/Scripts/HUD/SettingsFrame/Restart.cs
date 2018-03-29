using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : HUDButton {

    public override void OnPress()
    {
        DCGBase.RemoveAll();
    }
}
