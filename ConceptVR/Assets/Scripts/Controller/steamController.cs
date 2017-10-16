using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class steamController : Controller {
    SteamVR_TrackedController trackedController;

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
}
