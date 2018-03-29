using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExtrudeTool : MoveTool {

    List<List<DCGElement>> extrudedUndo;
    public void Start()
    {
        extrudedUndo = new List<List<DCGElement>>();
        base.Start();
    }
    public override bool TriggerDown()
    {
        if (sElements == null || sElements.Count == 0)
            return false;
        //This is the undo section and it is disgusting. Please ask me to die later. k thx
        List<DCGElement> eElements = new List<DCGElement>();
        List<DCGElement> newSElement = new List<DCGElement>();
        foreach(DCGElement e in sElements){
            List<DCGElement> temp = e.Extrude();
            newSElement.Add(temp[temp.Count-1]);
            temp.RemoveAt(temp.Count-1);
            eElements.AddRange(temp);
        }
        Debug.Log(eElements.Count);
        if(extrudedUndo.Count == 4)
        {
            extrudedUndo.RemoveAt(0);
        }
        extrudedUndo.Add(eElements);
        Debug.Log(extrudedUndo.Count);
        sElements = newSElement;
        base.TriggerDown();
        return true;
    }
    public override bool TriggerUp()
    {
        if(sElements.Count != 0)
            ClearSelection();
        return true;
    }
    public override bool Swipe()
    {
        base.Swipe();
        if (extrudedUndo.Count == 0)
            return true;
        foreach (DCGElement d in extrudedUndo[extrudedUndo.Count-1])
            d.Remove();
        extrudedUndo.RemoveAt(extrudedUndo.Count - 1);
        return true;
    }
}
