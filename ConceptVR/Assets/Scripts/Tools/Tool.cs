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

    public virtual void TriggerDown() { }
    public virtual void TriggerUp() { }
    public virtual void GripDown() { }
    public virtual void GripUp() { }
    public virtual void Tap(Vector3 position) { }
    public virtual void Swipe() { }
    public virtual void FreeForm() { }
    public virtual void FreeFormEnd() { }
}
