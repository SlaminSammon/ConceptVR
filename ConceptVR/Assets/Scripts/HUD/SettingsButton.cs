using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : HUDButton {

    public string buttonType;
	// Use this for initialization
	new void Start () {
		
	}
	
	// Update is called once per frame
	new void Update () {
		if(buttonType == "ChangeDoodleColor")
        {
            GameObject go = GameObject.Find("Tools");
            go.transform.Find("DoodleTool").gameObject.GetComponent<DoodleTool>().changeColor();
        }

	}
}
