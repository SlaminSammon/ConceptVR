using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class handController: Controller {
    LeapTrackedController leapControl;
    public SelectTool selectTool;
    // Use this for initialization
    void Start () {
        leapControl = GetComponent<LeapTrackedController>();
        leapControl.pinchMade += TriggerDown;
        leapControl.pinchGone += TriggerUp;
        leapControl.grabMade += GripDown;
        leapControl.grabGone += GripUp;
        leapControl.tapMade += Tap;
        currentTool = tools[0];
    }

    // Update is called once per frame
    void Update () {
        //Check to see if a pinch is being held.
        currentTool.triggerInput = leapControl.pinchHeld;
        currentTool.setPos(leapControl.position);

    }
    protected void TriggerDown()
    {
        currentTool.TriggerDown();
    }

    protected void TriggerUp()
    {
        currentTool.TriggerUp();
    }
    protected void GripUp()
    {
        return;
    }
    protected void GripDown()
    {
        //currentTool.GripDown();
        return;
    }

    protected void Tap(Vector3 position)
    {
        selectTool.Tap(position);
    }
   
}
