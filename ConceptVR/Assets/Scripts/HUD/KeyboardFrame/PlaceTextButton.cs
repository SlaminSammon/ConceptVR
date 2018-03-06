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
        if (textBox.GetComponent<TextMesh>().text != null)
        {
            GameObject go = Instantiate(ItemBase.itemBase.TextPrefab, textBox.transform.position, textBox.transform.rotation);
            go.GetComponent<TextItem>().updateText(textBox.GetComponent<TextMesh>().text);
            go.transform.localScale = textBox.transform.localScale *1.25f;
            ItemBase.Spawn(go);
            // clear the current text in the textbox
            textBox.GetComponent<TextMesh>().text = "";
        }


        base.OnPress();
    }


}
