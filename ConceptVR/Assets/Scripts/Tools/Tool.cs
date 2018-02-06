using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour {
    public bool triggerInput;
    public bool gripInput;
    public bool formInput;
    public Vector3 controllerPosition;
    public int playerID;
    public void setPos(Vector3 pos)
    {
        controllerPosition = pos;
    }
    public Vector3 getPos()
    {
        return controllerPosition;
    }

    public virtual void Update()
    {
        transform.position = controllerPosition;
    }

    public virtual bool TriggerDown() { return false; }
    public virtual bool TriggerUp() { return false; }
    public virtual void GripDown() { }
    public virtual void GripUp() { }
    public virtual bool Tap(Vector3 position) { return false; }
    public virtual void Swipe() { }
    public virtual void FreeForm() { }
    public virtual void FreeFormEnd() { }
}
