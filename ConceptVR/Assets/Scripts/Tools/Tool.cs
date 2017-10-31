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

    public abstract void TriggerDown();
    public abstract void TriggerUp();
}
