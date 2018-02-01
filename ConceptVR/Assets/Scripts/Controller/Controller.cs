using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {
    public bool hand;//Right is true, left is false;
    public Controller other;    //controller attached to the other hand (it is assumed the user only has two hands)
    public GameObject tools;

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
        currentTool = tools.transform.Find(toolName).GetComponent<Tool>();
        //Debug.Log(toolName);
        
        Debug.Log("Cur" + currentTool.GetType());
        Debug.Log("Last" + lastTool.GetType());
        if (currentTool.GetType() != lastTool.GetType())
        {
            deactivateLastTool(lastTool);
            activateNewTool(currentTool);
        }
        //Debug.Log(currentTool.GetType());
    }
    /*public Tool getToolByType(Type type)
    {
        //Debug.Log("Switching");
        foreach (Tool t in tools)
        {
            if (t.GetType() == type)
                return t;
        }
        //Debug.Log("Not Switching");
        return currentTool;
    }*/
    public Tool GetToolByName(String name)
    {
        return tools.transform.Find(name).GetComponent<Tool>();
    }
    
    public void deactivateLastTool(Tool t) {
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
