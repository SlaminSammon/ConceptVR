using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTool : Tool {
    public Material selMat;

    ItemBase itemBase;

    protected float defaultSelectDistance = .045f;
    protected float selectDistance = 0.45f;
    public void Start()
    {
        itemBase = GameObject.Find("ItemBase").GetComponent<ItemBase>();
    }

    public override void Update()
    {
        base.Update();
        selMat.SetFloat("_Offset", Time.time / 6f);
    }

    public override bool Tap(Vector3 position)
    {
        float playerScale = GameObject.Find("Managers").GetComponent<SettingsManager>().playerScale;
        selectDistance = defaultSelectDistance * playerScale * 0.50f;
        Debug.Log(DCGBase.sElements.Count);
        DCGElement nearestElement = DCGBase.NearestElement(position, selectDistance);
        Item item = ItemBase.itemBase.findNearestItem(position);
        return (item == null && nearestElement == null) ? false : 
            (item == null ? TapDCG(nearestElement) : 
            (nearestElement == null ? TapItem(item) : 
            (item.Distance(position) > nearestElement.Distance(position) ? TapDCG(nearestElement) : TapItem(item))));
        
    }
    public override bool Swipe()
    {
        if (DCGBase.sElements.Count > 0)
        {
            ClearSelection();
            return true;
        }
        Deselect();
        return false;
    }
    public override bool DualTriggerDown()
    {
        if (DCGBase.sElements.Count == 0)
            return true;
        List<DCGElement> copiedElements = new List<DCGElement>();
        foreach(DCGElement d in DCGBase.sElements)
        {
            copiedElements.Add(d.Copy());
        }

        return true;
    }

    public List<Point> Select(DCGElement elem)
    {
        elem.isSelected = true;
        elem.isLocked = true;
        if (ItemBase.sItems.Count > 0)
            Item.Pop();
        return elem.GetPoints();
    }
    public void Select(Item item)
    {
        item.CmdSelect();
        ItemBase.sItems.Add(item);
        if(DCGBase.sPoints.Count == 0)
            itemBase.itemHudManager(item);
    }

    public void ClearSelection()
    {
        foreach(DCGElement e in DCGBase.sElements)
        {
            Deselect(e);
        }

        DCGBase.sElements = new List<DCGElement>();
        DCGBase.sPoints = new List<Point>();
    }

    public void Deselect(DCGElement e)
    {
        e.isSelected = false;
        e.isLocked = false;
    }
    public void Deselect()
    {
        Debug.Log(Item.popped);
        foreach (Item item in ItemBase.sItems)
        {
            item.CmdDeSelect();
        }
        ItemBase.sItems.Clear();
        if (!Item.popped)
        {
            Item.Pop();
            itemBase.firstType = "";
        }
    }

    protected void OnRenderObject()
    {
        selMat.SetPass(0);
        foreach (DCGElement e in DCGBase.sElements)
        {
            e.Render();
        }
    }

    void OnDisable()
    {
        ClearSelection();
        if (ItemBase.sItems.Count != 0)
            Deselect();
    }
    public bool TapDCG(DCGElement nearestElement)
    {
        if (nearestElement != null && !nearestElement.isLocked)
        {
            List<Point> newSel = Select(nearestElement);
            DCGBase.sElements.Remove(nearestElement);
            DCGBase.sElements.Add(nearestElement);
            foreach (Point p in newSel)
            {
                DCGBase.sPoints.Remove(p); //If the point exists in the point list, remove the copy before adding it in
                DCGBase.sPoints.Add(p);
            }
            return true;
        }
        else
            return false;
    }
    public bool TapItem(Item item)
    {
        if (item != null && !item.isLocked)
        {
            Select(item);
            return true;
        }
        else
            return false;
    }
}
