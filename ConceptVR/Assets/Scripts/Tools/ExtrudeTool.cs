using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudeTool : MoveTool {

	public override void TriggerDown()
    {
        List<DCGElement> eElements = new List<DCGElement>();
        foreach(DCGElement e in sElements){
            e.Extrude();
        }
        base.TriggerDown();
    }
}
