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
    public virtual void Remove() { }    //Called when this element is removed, should remove other elements that rely on this one
    public virtual bool ChildrenSelected() { return false; }    //Returns true iff all children of this element are selected
    public virtual float Distance(Vector3 position) { return Mathf.Infinity; }
    public virtual List<Point> GetPoints() { return new List<Point>(); }
    public virtual List<Point> Extrude() { return new List<Point>(); }
    public virtual void Lock() { } //Is used to lock all elements below this element in the hierarchy.
    public virtual void Unlock() { } //Is used to unlock all elements below this element in the hierarchy.


    public int nextElementID()
    {
        return currentID++;
    }
}
