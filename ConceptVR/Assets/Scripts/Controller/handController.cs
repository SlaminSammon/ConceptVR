using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class handController: Controller {
    LeapTrackedController leapControl;
    private Tool lastTool;
    // Use this for initialization
    void Start () {
        leapControl = GetComponent<LeapTrackedController>();
        leapControl.pinchMade += TriggerDown;
        leapControl.pinchGone += TriggerUp;
        leapControl.grabMade += GripDown;
        leapControl.grabGone += GripUp;
        leapControl.tapMade += Tap;
        leapControl.swipeMade += Swipe;
        leapControl.freeForm += freeForm;
        leapControl.freeFormEnd += freeFormEnd;
    }

    // Update is called once per frame
    void Update () {
        currentTool.setPos(leapControl.position);
        //Check to see if a pinch is being held.
        if (currentTool.GetType() == typeof(FreeFormTool))
        {
            if (leapControl.forming)
                currentTool.formInput = true;
            else
                currentTool.formInput = false;
        }
        if(!leapControl.pinchHeld && leapControl.pinchInput)
            currentTool.triggerInput = leapControl.pinchInput;
        else
            currentTool.triggerInput = leapControl.pinchHeld;
        currentTool.gripInput = leapControl.gripInput;

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
        currentTool.Tap(position);
    }
    protected void Swipe()
    {
        currentTool.Swipe();
    }
    protected void freeForm()
    {
        lastTool = currentTool;
        Debug.Log(currentTool);
        currentTool = getToolByType(typeof(FreeFormTool));
        activateNewTool(currentTool.GetType().ToString());
        currentTool.FreeForm();
    }
    protected void freeFormEnd()
    {
        deactivateLastTool(currentTool.GetType().ToString());
        currentTool.FreeFormEnd();
        currentTool = lastTool;
    }

}
