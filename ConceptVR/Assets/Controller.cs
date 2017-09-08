using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    SteamVR_TrackedController trackedController;

    public List<Tool> tools;

    Tool currentTool;

	// Use this for initialization
	void Start () {
		trackedController = GetComponent<SteamVR_TrackedController>();
        trackedController.TriggerClicked += this.TriggerDown;
        trackedController.TriggerUnclicked += this.TriggerUp;


        currentTool = tools[0];
    }
	
	// Update is called once per frame
	void Update () {
        currentTool.triggerInput = trackedController.triggerPressed;
	}

    void TriggerDown(object sender, ClickedEventArgs e)
    {
        currentTool.TriggerDown();
    }

    void TriggerUp(object sender, ClickedEventArgs e)
    {
        currentTool.TriggerUp();
    }
}
