using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyButton : HUDButton {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    public override void OnPress()
    {
        GameObject selectedItem = GameObject.Find("SelectItemsTool").GetComponent<SelectItemsTool>().selected;
        GameObject.Find("SelectItemsTool").GetComponent<SelectItemsTool>().Deselect();

        if (selectedItem.tag == "Light")
        {
            GameObject.Find("ItemBase").GetComponent<ItemBase>().removeLight(selectedItem);
        }
        Destroy(selectedItem);
        base.OnPress();
    }

}
