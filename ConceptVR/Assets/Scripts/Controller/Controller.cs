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
}
