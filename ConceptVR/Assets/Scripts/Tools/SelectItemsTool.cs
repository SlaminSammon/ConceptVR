using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
    }

    public void Deselect()
    {
        foreach(Item item in ItemBase.sItems)
        {
            item.CmdDeSelect();
        }
        ItemBase.sItems.Clear();
        if (itemBase.isHUD)
        {
            Item.Pop();
            itemBase.firstType = "";
        }
    }
    public void Select(Item item)
    {
        item.CmdSelect();
        ItemBase.sItems.Add(item);
        itemBase.itemHudManager(item);
    }
    private void OnDisable()
    {
        Deselect();
    }


}
