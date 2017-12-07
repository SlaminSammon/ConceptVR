using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleTool : Tool {
    private LineRenderer currLineRend;//Line Renderer
    private int numClicks = 0;
    public Color currLineColor;
    public Material material;
    void Start () {
		currLineColor = Color.red;
	}
	
	// Update is called once per frame
	new void Update () {
        if (triggerInput && currLineRend != null)
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
        currLineRend.material = material;
        numClicks = 0;
        //Makes a thinner line
        currLineRend.SetWidth(.01f, .01f);
    }
}
