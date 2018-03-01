using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTextButton : HUDButton {

    public GameObject textBox;
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
        GameObject generatedText = GameObject.Instantiate(textBox);
        generatedText.name = "GeneratedText - " + generatedText.GetComponent<TextMesh>().text;
        generatedText.transform.position = textBox.transform.position;
        generatedText.transform.rotation = textBox.transform.rotation;
        // clear the current text in the textbox
        textBox.GetComponent<TextMesh>().text = "";

        base.OnPress();
    }


}
