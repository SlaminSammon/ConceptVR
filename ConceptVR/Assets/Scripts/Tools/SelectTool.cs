using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTool : Tool {
    public Material selMat;

    protected List<DCGElement> sElements;
    protected List<Point> sPoints;

    protected float selectDistance = .07f;
    public void Start()
    {
        sElements = new List<DCGElement>();
        sPoints = new List<Point>();
    }

    public override void Update()
    {
        base.Update();
        selMat.SetFloat("_Offset", Time.time / 6f);
    }

    public override bool Tap(Vector3 position)
    {
        DCGElement nearestElement = DCGBase.NearestElement(position, selectDistance);
        if (nearestElement != null && !nearestElement.isLocked)
        {
            List<Point> newSel = Select(nearestElement);
            sElements.Remove(nearestElement);
            sElements.Add(nearestElement);
            foreach (Point p in newSel)
            {
                sPoints.Remove(p); //If the point exists in the point list, remove the copy before adding it in
                sPoints.Add(p);
            }
            return true;
        }
        else
            return false;
    }
    public override bool Swipe()
    {
        if (sElements.Count > 0)
        {
            ClearSelection();
            return true;
        }
        return false;
    }

    public List<Point> Select(DCGElement elem)
    {
        elem.isSelected = true;
        elem.isLocked = true;
        return elem.GetPoints();
    }

    public void ClearSelection()
    {
        foreach(DCGElement e in sElements)
        {
            Deselect(e);
        }

        sElements = new List<DCGElement>();
        sPoints = new List<Point>();
    }

    public void Deselect(DCGElement e)
    {
        e.isSelected = false;
        e.isLocked = false;
    }

    protected void OnRenderObject()
    {
        selMat.SetPass(0);
        foreach (DCGElement e in sElements)
        {
            e.Render();
        }
    }

    void OnDisable()
    {
        ClearSelection();
    }
}
