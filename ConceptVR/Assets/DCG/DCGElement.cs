using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DCGElement
{
    public static int currentID = 0;
    public int elementID;   //Unique identifier for this element
    public int lastMoveID;  
    public bool isSelected; 
    public bool isLocked;
    public int playerLocked;

    public virtual void Render() { }    //Called from OnRenderObject in DCGBase
    public virtual void Update() { }    //Called when an element associated with this one is updated
    public virtual void Remove() { }    //Called when this element is removed, should remove other elements that rely on this one
    public virtual bool ChildrenSelected() { return false; }    //Returns true iff all children of this element are selected
    public virtual float Distance(Vector3 position) { return Mathf.Infinity; }
    public virtual DCGConstraint NearestConstraint(Vector3 position) { return new DCGConstraint(); }
    public virtual Vector3 ConstraintPosition(float[] constraintData) { return Vector3.zero; }
    public virtual List<Point> GetPoints() { return new List<Point>(); }
    public virtual List<DCGElement> Extrude() { return new List<DCGElement>(); }
    public virtual void Lock() { } //Is used to lock all elements below this element in the hierarchy.
    public virtual void Unlock() { } //Is used to unlock all elements below this element in the hierarchy.
    public virtual DCGElement Copy() { return new Point(new Vector3(0, 0, 0));  }
    public virtual bool ParentSelected() { return false; }
    public virtual void RemoveChildren() { }
    public virtual List<DCGElement> GetParents() { return new List<DCGElement>(); }

    public int nextElementID()
    {
        currentID += 32;
        return currentID;
    }
}
