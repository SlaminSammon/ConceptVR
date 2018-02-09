using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ListContains { lights, doodles } // this will be expanded as we gain more items
public class SelectItemsTool : Tool {

    ItemBase itemBase;

	// Use this for initialization
	void Start () {
        itemBase = GameObject.Find("ItemBase").GetComponent<ItemBase>();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    public override bool Swipe()
    {
        if (ItemBase.sItems != null)
        {
            Deselect();
            return true;
        }
        return false;
    }

    public override bool Tap(Vector3 position)
    {
        Item item = itemBase.findNearestItem(position);
        if(item != null && !item.isLocked)
        {
            Select(item);
            return true;
        }
        else
            return false;
        /*
        bool wasSelected = selected != null;
        Deselect();
        selected = findNearestItem();
        if (selected)
        {
            if (selected.tag == "Light")
            {
                selected.gameObject.GetComponent<MeshRenderer>().material = newMat;
                selected.gameObject.GetComponent<Light>().color = Color.blue;
            }
        }
        if (selected != null || wasSelected)
            return true;
        else
            return false;
            */

    }

    public void Deselect()
    {
        foreach(Item item in ItemBase.sItems)
        {
            item.DeSelect();
        }
        ItemBase.sItems.Clear();
        if (itemBase.isHUD)
        {
            Item.Pop();
            itemBase.firstType = "";
        }
        /*
        if (selected)
        {
            if (selected.tag == "Light")
            {
                selected.gameObject.GetComponent<MeshRenderer>().material = selectedMaterial;
                selected.gameObject.GetComponent<Light>().color = selectedLightColor;
            }
            HUD.Pop();
            selected = null;
        }
        */
    }
    public void Select(Item item)
    {
        item.Select();
        ItemBase.sItems.Add(item);
        itemBase.itemHudManager(item);
    }


}
