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
        leapControl.fireGun += fireGun;
    }

    // Update is called once per frame
    void Update () {
        if (currentTool != null)
        {
            currentTool.setPos(leapControl.position);

            //Check to see if a pinch is being held.
            if (currentTool.GetType() == typeof(FreeFormTool))
            {

                if (leapControl.forming)
                    currentTool.formInput = true;
                else
                    currentTool.formInput = false;
            }
            else{
                Debug.Log("leapControl.");
                currentTool.triggerInput = leapControl.pinchHeld;
            }

        }

    }
    protected void TriggerDown()
    {
        Debug.Log("gfhka");
        if (currentTool != null && currentTool.TriggerDown())
            return;
        foreach (Tool tool in ToolQueue)
            if (tool.TriggerDown())
                return;
    }

    protected void TriggerUp()
    {
        Debug.Log("hell");
        if (currentTool != null && currentTool.TriggerUp())
            return;
        else
            Debug.Log("Failure");
        foreach (Tool tool in ToolQueue)
            if (tool.TriggerUp())
                return;
    }
    protected void GripUp()
    {
        if (currentTool.GripUp())
            return;
        foreach (Tool tool in ToolQueue)
            if (tool.GripUp())
                return;
    }
    protected void GripDown()
    {
        if (currentTool.GripDown())
            return;
        foreach (Tool tool in ToolQueue)
            if (tool.GripDown())
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
        if (currentTool.Swipe())
            return;
        foreach (Tool tool in ToolQueue)
            if (tool.Swipe())
                return;
    }
    protected void freeForm()
    {
        lastTool = currentTool;
        Debug.Log(currentTool);
        changeTool("FreeFormTool");
        currentTool.FreeForm(leapControl);
    }
    protected void freeFormEnd()
    {
        currentTool.FreeFormEnd();
        changeTool(lastTool.GetType().ToString());
    }
    protected void freeFormFailure()
    {
        currentTool.formInput = false;
        changeTool(lastTool.GetType().ToString());
    }
    protected void fireGun()
    {
        if (currentTool.Fire())
            return;
        foreach (Tool tool in ToolQueue)
            if (tool.Fire())
                return;
    }
    void OnEnable(){
        
    }

}
