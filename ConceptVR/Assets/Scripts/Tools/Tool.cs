using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour {
    public bool triggerInput;
    public Vector3 controllerPosition;
    public void setPos(Vector3 pos)
    {
        controllerPosition = pos;
    }
    public Vector3 getPos()
    {
        return controllerPosition;
    }

    public virtual void TriggerDown() { }
    public virtual void TriggerUp() { }
    public virtual void GripDown() { }
}
