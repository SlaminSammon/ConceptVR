using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodColorBtn : HUDButton {

    public Color color;
	// Use this for initialization
	void Start () {
        base.Start();	
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
    public override void OnPress()
    {
        foreach(Doodle d in ItemBase.sItems)
        {
            d.changeColor(color);
        }
    }
}
