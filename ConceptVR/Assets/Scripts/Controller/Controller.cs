using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {
    public bool hand;//Right is true, left is false;
    public Controller other;    //controller attached to the other hand (it is assumed the user only has two hands)
    public GameObject tools;

    public List<Tool> ToolQueue;
    public Tool currentTool;

	// Use this for initialization
	protected void Start () {
        if (ToolQueue.Count == 0)
        {
            ToolQueue = new List<Tool>();
            ToolQueue.Add(new BaseTool());
        }
        currentTool = ToolQueue[0];
        tools = this.gameObject.transform.parent.gameObject.transform.Find("Tools").gameObject;
    }
	
	// Update is called once per frame
	void Update () {

    }
    
    protected void TriggerDown(object sender, ClickedEventArgs e)
    {
        if (currentTool.TriggerDown())
            return;
        foreach (Tool tool in ToolQueue)
            if (tool.TriggerDown())
                return;
    }

    protected void TriggerUp(object sender, ClickedEventArgs e)
    {
        currentTool.triggerInput = false;
        if (currentTool.TriggerUp())
            return;
        foreach (Tool tool in ToolQueue)
            if (tool.TriggerUp())
                return;
    }
    protected void LaserPointer()
    {

    }
    protected void GripDown(object sender, ClickedEventArgs e)
    {
        if (currentTool.GripDown())
            return;
        foreach (Tool tool in ToolQueue)
            if (tool.GripDown())
                return;
    }
    protected void GripUp(object sender, ClickedEventArgs e)
    {
        if (currentTool.GripUp())
            return;
        foreach (Tool tool in ToolQueue)
            if (tool.GripUp())
                return;
    }

    public void changeTool(string toolName)
    {
        Tool lastTool = currentTool;
        currentTool = tools.transform.Find(toolName).GetComponent<Tool>();
        //Debug.Log(toolName);
        
        if (currentTool.GetType() != lastTool.GetType())
        {
            deactivateLastTool(lastTool);
            activateNewTool(currentTool);
        }
        //Debug.Log(currentTool.GetType());
    }
    public Tool GetToolByName(String name)
    {
        return tools.transform.Find(name).GetComponent<Tool>();
    }
    
    public void deactivateLastTool(Tool t) {
        if (t)
            t.gameObject.SetActive(false);
    }
    public void activateNewTool(Tool t)
    {
        t.gameObject.SetActive(true);
    }
    public void OnEnable()
    {
        if (currentTool == null) currentTool = tools.GetComponentInChildren<Tool>();
        foreach (Tool t in tools.GetComponentsInChildren<Tool>())
            deactivateLastTool(t);
    }
}
