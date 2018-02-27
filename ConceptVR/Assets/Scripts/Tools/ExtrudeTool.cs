using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudeTool : MoveTool {

	public override bool TriggerDown()
    {
        if (sElements == null || sElements.Count == 0)
            return false;
        List<DCGElement> eElements = new List<DCGElement>();
        foreach(DCGElement e in sElements){
            eElements.AddRange(e.Extrude());
        }

        sElements = eElements;
        base.TriggerDown();
        return true;
    }
}
