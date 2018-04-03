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
        if (DCGBase.sElements == null || DCGBase.sElements.Count == 0)
            return false;
        //This is the undo section and it is disgusting. Please ask me to die later. k thx
        List<DCGElement> newSElement = new List<DCGElement>();
        foreach(DCGElement e in DCGBase.sElements){
            List<DCGElement> temp = e.Extrude();
            newSElement.Add(temp[temp.Count-1]);
            extrudedUndo.Add(temp);
        }
        Debug.Log(extrudedUndo.Count);
        ClearSelection();
        foreach (DCGElement elem in newSElement)
            TapDCG(elem);//It's ghetto dont care.
        base.TriggerDown();
        return true;
    }
    public override bool TriggerUp()
    {
        if(DCGBase.sElements.Count != 0)
            ClearSelection();
        return true;
    }
    public override bool Swipe()
    {
        base.Swipe();
        if (extrudedUndo.Count == 0)
            return true;
        for(int i =0; i< extrudedUndo.Count; ++i)
        {
            Debug.Log(extrudedUndo[i].Count);
            extrudedUndo[i][0].Copy();
            for(int j = 0; j < extrudedUndo[i].Count; ++j)
            {
                extrudedUndo[i][j].Remove();
            }
        }
        extrudedUndo.Clear();
        return true;
    }
}
