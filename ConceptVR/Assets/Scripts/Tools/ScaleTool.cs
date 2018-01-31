using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTool : SelectTool {
    List<Vector3> startPositions;   //positions of the selected points at start
    Vector3 startCenter;    //average position of the selected points
    Vector3 grabPosition;   //position of the tool at start
    Quaternion grabOrientation; //rotation of the tool at start

    
	new void Update () {
        base.Update();

        if (triggerInput)
        {
            float scaleFactor = (controllerPosition - startCenter).magnitude / (grabPosition - startCenter).magnitude;

            for (int i = 0; i < sPoints.Count; ++i)
                sPoints[i].setPosition(startCenter + (startPositions[i] - startCenter) * scaleFactor);
        }
    }

    public override void TriggerDown()
    {
        startPositions = new List<Vector3>(sPoints.Count);
        startCenter = Vector3.zero;

        foreach (Point p in sPoints)
        {
            startPositions.Add(p.position);
            startCenter += p.position;
        }
        startCenter /= sPoints.Count;

        grabPosition = controllerPosition;
    }

    public override void TriggerUp()
    {
        startPositions = null;
    }
}
