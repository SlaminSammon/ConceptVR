using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTool : Tool {
    public Material selMat;

    protected List<DCGElement> sElements;
    protected List<Point> sPoints;

    protected float selectDistance = .05f;
    public void Start()
    {
        sElements = new List<DCGElement>();
        sPoints = new List<Point>();
    }

    public void Update()
    {
        selMat.SetFloat("Offset", Time.time);
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
                if (newSel.GetType() == typeof(Point))
                {
                    sPoints.Add(e as Point);
                }
            }
        }
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
    }

    public void Deselect(DCGElement e)
    {
        e.isSelected = false;
    }

    void OnRenderObject()
    {
        selMat.SetPass(0);
    }
}
