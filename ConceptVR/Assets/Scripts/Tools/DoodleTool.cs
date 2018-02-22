using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DoodleTool : Tool {
    private LineRenderer currLineRend;//Line Renderer
    private int numClicks = 0;
    public Material material;
    Doodle doodle;
    private int colorIndex;
    public GameObject doodPrefab;
    void Start () {
        colorIndex = 0;
	}
	
	// Update is called once per frame
	new void Update () {
        if (triggerInput && currLineRend != null && doodle != null && controllerPosition != new Vector3(0,0,0))
        {
            doodle.CmdUpdateLineRenderer(controllerPosition);
            /*currLineRend.positionCount = numClicks + 1;
            currLineRend.SetPosition(numClicks, controllerPosition);
            doodle.latestPoint = controllerPosition;
            numClicks++;*/
        }
	}
    public override bool TriggerDown()
    {
        GameObject go = Instantiate(doodPrefab,controllerPosition,new Quaternion(0,0,0,0));
        go.SetActive(true);
        ItemBase.Spawn(go);
        doodle = go.GetComponent<Doodle>();
        currLineRend = go.GetComponent<LineRenderer>();
        currLineRend.material = material;
        numClicks = 0;
        //Makes a thinner line
        currLineRend.startWidth = .01f;
        currLineRend.endWidth = .01f;
        return true;
    }
    public override bool TriggerUp()
    {
        if(doodle == false)
        {
            Debug.Log("Wut");
        }
        doodle.isFinished = true;
        return true;
    }
}
