using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardButton : HUDButton {

    public string text;

	// Use this for initialization
	void Start () {
        text = this.gameObject.transform.Find("Text").gameObject.GetComponent<TextMesh>().text;
        base.Start();
	}
    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void OnPress()
    {
        if (text == "backspace")
        {
            string textBoxText = transform.parent.Find("Textbox").GetComponent<TextMesh>().text;
            if (textBoxText != "")
            {
                // remove last character from string
                textBoxText = textBoxText.Substring(0, (textBoxText.Length - 1));
                transform.parent.Find("Textbox").GetComponent<TextMesh>().text = textBoxText;
            }
        }else if (text == "enter")
        {
            transform.parent.Find("Textbox").GetComponent<TextMesh>().text += '\n';
        }
        else
        {
            transform.parent.Find("Textbox").GetComponent<TextMesh>().text += text;
        }
        base.OnPress();
    }

}
