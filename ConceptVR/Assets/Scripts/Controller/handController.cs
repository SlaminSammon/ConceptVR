using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class handController: Controller {
    LeapTrackedController leapControl;
    private Tool lastTool;
    // Use this for initialization
    new void Start () {
        base.Start();
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
        if (currentTool.TriggerDown())
            return;
        foreach (Tool tool in ToolQueue)
            if (tool.TriggerDown())
                return;
    }

    protected void TriggerUp()
    {
        if (currentTool.TriggerUp())
            return;
        foreach (Tool tool in ToolQueue)
            if (tool.TriggerUp())
                return;
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
        if (currentTool.Tap(position))
            return;
        foreach (Tool tool in ToolQueue)
            if (tool.Tap(position))
                return;
    }
    protected void Swipe()
    {
        currentTool.Swipe();
    }
    protected void freeForm()
    {
        lastTool = currentTool;
        Debug.Log(currentTool);
        changeTool("FreeFormTool");
        currentTool.FreeForm();
    }
    protected void freeFormEnd()
    {
        currentTool.FreeFormEnd();
        changeTool(lastTool.GetType().ToString());
    }

}
