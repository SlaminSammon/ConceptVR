using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {
    public bool hand;//Right is true, left is false;
    public List<Tool> tools;

    protected Tool currentTool;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (currentTool.GetType() == typeof(DragTool))
            Debug.Log("Drag");
        else if (currentTool.GetType() == typeof(SketchTool))
            Debug.Log("Draw");
        else if (currentTool.GetType() == typeof(BezierTool))
            Debug.Log("Poo");
        else
            Debug.Log("Poobins");

    }

    protected void TriggerDown(object sender, ClickedEventArgs e)
    {
        currentTool.TriggerDown();
    }

    protected void TriggerUp(object sender, ClickedEventArgs e)
    {
        currentTool.TriggerUp();
    }
    protected void LaserPointer()
    {

    }
    protected void GripDown(object sender, ClickedEventArgs e)
    {
        currentTool.GripDown();
    }
    protected void GripUp(object sender, ClickedEventArgs e)
    {
        return;
    }
    public void changeTool(string toolName)
    {
        switch(toolName)
        {
            case "DragTool":
                currentTool = getToolByType(typeof(DragTool));
                break;
            case "SketchTool":
                currentTool = getToolByType(typeof(SketchTool));
                break;
            case "BezierTool":
                currentTool = getToolByType(typeof(BezierTool));
                break;
            /*case "ExtrudeTool":
                currentTool = getToolByType(typeof(ExtrudeTool));
                break;*/
            default:
                break;
        }
    }
    public Tool getToolByType(Type type)
    {
        foreach (Tool t in tools)
        {
            if (t.GetType() == type)
                return t;
        }
        return currentTool;
    }
}
