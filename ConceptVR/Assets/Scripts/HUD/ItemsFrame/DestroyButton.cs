using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyButton : HUDButton {

    ItemBase itemBase;
    SelectItemsTool selectTool;
	// Use this for initialization
	void Start () {
        itemBase = GameObject.Find("ItemBase").GetComponent<ItemBase>();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    public override void OnPress()
    {
        foreach (Item item in ItemBase.sItems)
        {
            item.destroyed = true;
            Destroy(item.gameObject);
        }
        ItemBase.sItems.Clear();
        Item.Pop();
        itemBase.firstType = "";
        /*
        GameObject selectedItem = GameObject.Find("SelectItemsTool").GetComponent<SelectItemsTool>().selected;
        GameObject.Find("SelectItemsTool").GetComponent<SelectItemsTool>().Deselect();

        if (selectedItem.tag == "Light")
        {
            GameObject.Find("ItemBase").GetComponent<ItemBase>().removeLight(selectedItem);
        }
        Destroy(selectedItem);
        */
        base.OnPress();
    }

}
