using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {
    public bool hand;//Right is true, left is false;
    public Controller other;    //controller attached to the other hand (it is assumed the user only has two hands)
    public List<Tool> tools;

    protected Tool currentTool;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {

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
        Tool lastTool = currentTool;
        //Debug.Log(toolName);
        switch(toolName)
        {
            case "MoveTool":
                currentTool = getToolByType(typeof(MoveTool));
                break;
            case "DoodleTool":
                currentTool = getToolByType(typeof(DoodleTool));
                break;
            case "DestroyTool":
                currentTool = getToolByType(typeof(DestroyTool));
                break;
            case "PointTool":
                currentTool = getToolByType(typeof(PointTool));
                break;
            case "LinkTool":
                currentTool = getToolByType(typeof(LinkTool));
                break;
            case "ExtrudeTool":
                currentTool = getToolByType(typeof(ExtrudeTool));
                break;
            case "BezierTool":
                currentTool = getToolByType(typeof(BezierTool));
                break;
            case "RectangleTool":
                currentTool = getToolByType(typeof(RectangleTool));
                break;

            default:
                break;
        }
        Debug.Log("Cur" + currentTool.GetType());
        Debug.Log("Last" + lastTool.GetType());
        if (currentTool.GetType() != lastTool.GetType())
        {
            deactivateLastTool(lastTool.GetType().ToString());
            activateNewTool(currentTool.GetType().ToString());
        }
        //Debug.Log(currentTool.GetType());
    }
    public Tool getToolByType(Type type)
    {
        //Debug.Log("Switching");
        foreach (Tool t in tools)
        {
            if (t.GetType() == type)
                return t;
        }
        //Debug.Log("Not Switching");
        return currentTool;
    }
    public void deactivateLastTool(string type) {
        GameObject.Find(type).SetActive(false);
    }
    public void activateNewTool(string type)
    {
        GameObject toolsList = GameObject.Find("Tools");
        toolsList.transform.Find(type).gameObject.SetActive(true);
    }
    public void OnEnable()
    {
        if (currentTool == null) currentTool = tools[0];
        foreach (Tool t in tools)
        {
            if (t.GetType() != currentTool.GetType())
                deactivateLastTool(t.GetType().ToString());
           // Debug.Log(t.GetType());
        }
    }
}
