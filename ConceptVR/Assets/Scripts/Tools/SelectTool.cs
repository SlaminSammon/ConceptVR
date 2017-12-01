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

    public void Update()
    {
        base.Update();
        selMat.SetFloat("_Offset", Time.time / 6f);
    }

    public override void Tap(Vector3 position)
    {
        Point nearestPoint = DCGBase.NearestPoint(position, selectDistance);
        if (nearestPoint != null)
        {
            List<DCGElement> newSel = Select(nearestPoint);
            foreach(DCGElement e in newSel)
            {
                sElements.Add(e);
                Debug.Log(e.GetType());
                if (e.GetType() == typeof(Point))
                {
                    sPoints.Remove(e as Point); //If the point exists in the point list, remove the copy before adding it in
                    sPoints.Add(e as Point);
                }
            }
        }
    }
    public override void Swipe()
    {
        ClearSelection();
    }

    public List<DCGElement> Select(Point p)
    {
        p.isSelected = true;
        List<DCGElement> n = new List<DCGElement>();
        n.Add(p as DCGElement);
        return n;
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
    }

    protected void OnRenderObject()
    {
        selMat.SetPass(0);
        foreach (DCGElement e in sElements)
        {
            e.Render();
        }
    }
}
