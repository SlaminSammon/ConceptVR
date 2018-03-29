using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTool : SelectTool {
    List<Vector3> startPositions;   //positions of the selected points at start
    List<Vector3> startPos;
    Vector3 grabPosition;   //position of the tool at start
    Quaternion grabOrientation; //rotation of the tool at start

    // Update is called once per frame
    new void Update () {
        base.Update();

        if (triggerInput && sPoints != null)
        {
            for (int i = 0; i < sPoints.Count; ++i)
            {
                sPoints[i].setPosition(startPositions[i] + controllerPosition - grabPosition);
            }
        }
        if (triggerInput && ItemBase.sItems != null)
            for (int i = 0; i < ItemBase.sItems.Count; ++i)
                ItemBase.sItems[i].changePosition(startPos[i], controllerPosition, grabPosition);
    }

    public override bool TriggerDown()
    {
        if (sPoints.Count == 0 && ItemBase.sItems.Count == 0)
            return false;
        if (sPoints.Count > 0)
        {
            startPositions = new List<Vector3>(sPoints.Count);
            //Debug.Log(sPoints.Count);
            foreach (Point p in sPoints)
                startPositions.Add(p.position);
        }
        if(ItemBase.sItems.Count > 0)
        {
            startPos = new List<Vector3>();
            foreach (Item v in ItemBase.sItems)
                startPos.Add(v.Position());
        }

        grabPosition = controllerPosition;
        return true;
    }

    public override bool TriggerUp()
    {
        if (startPositions != null)
        {
            startPositions = null;
            return true;
        }
        else
            return false;
    }
}
