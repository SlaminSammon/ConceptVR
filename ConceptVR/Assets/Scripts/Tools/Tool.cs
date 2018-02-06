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
    public virtual bool GripDown() { return false; }
    public virtual bool GripUp() { return false; }
    public virtual bool Tap(Vector3 position) { return false; }
    public virtual bool Swipe() { return false; }
    public virtual void FreeForm() {}
    public virtual void FreeFormEnd() {}
}
