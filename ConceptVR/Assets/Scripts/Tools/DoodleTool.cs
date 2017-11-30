using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleTool : Tool {
    private LineRenderer currLineRend;//Line Renderer
    private int numClicks = 0;
    public Color currLineColor;
    void Start () {
		currLineColor = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
        if (triggerInput)
        {
            currLineRend.positionCount = numClicks + 1;
            currLineRend.SetPosition(numClicks, controllerPosition);
            numClicks++;
        }
	}
    public override void TriggerDown()
    {
        GameObject go = new GameObject();
        currLineRend = go.AddComponent<LineRenderer>();
        numClicks = 0;
        //Makes a thinner line
        currLineRend.SetWidth(.01f, .01f);
    }
}
