using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DCGElement {
    private static int currentID = 0;
    public int elementID;
    public int lastMoveID;
    public bool isSelected;
    public bool isLocked;

    public virtual void Render() { }    //Called from OnRenderObject in DCGBase
    public virtual void Update() { }    //Called when an element associated with this one is updated

    public int nextElementID()
    {
        return currentID++;
    }
}
