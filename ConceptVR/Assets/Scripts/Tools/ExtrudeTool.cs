using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudeTool : MoveTool {

	public override void TriggerDown()
    {
        foreach(DCGElement e in sElements){
            e.Extrude();
        }
        base.TriggerDown();
    }
}
