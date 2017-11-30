using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTool : SelectTool {
    List<Vector3> startPositions;   //positions of the selected points at start
    Vector3 grabPosition;   //position of the tool at start
    Quaternion grabOrientation; //rotation of the tool at start

    // Update is called once per frame
    new void Update () {
        base.Update();

        if (triggerInput)
        {
            for(int i = 0; i < sPoints.Count; ++i)
                sPoints[i].setPosition(startPositions[i] + controllerPosition - grabPosition);
        }
	}

    public override void TriggerDown()
    {
        startPositions = new List<Vector3>(sPoints.Count);
        foreach (Point p in sPoints)
            startPositions.Add(p.position);

        grabPosition = controllerPosition;
    }

    public override void TriggerUp()
    {
        startPositions = null;
    }
}
