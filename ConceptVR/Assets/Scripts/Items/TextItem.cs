using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextItem : Item {

    string text;
    Bounds textBounds;
	// Use this for initialization
	void Start () {
        textBounds = this.gameObject.GetComponent<Renderer>().bounds;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void CmdSelect()
    {
        base.CmdSelect();
    }
    public override void CmdDeSelect()
    {
        base.CmdDeSelect();
    }
    public void finalBounds()
    {
        textBounds.SetMinMax(textBounds.min - new Vector3(.1f, .1f, .1f), textBounds.max + new Vector3(.1f, .1f, .1f));
    }
    public override float Distance(Vector3 pos)
    {
        if (textBounds.Contains(pos))
            return Vector3.Distance(pos, this.Position(pos));
        return 100000f;
    }
    public void updateText(string txt)
    {
        text = txt;
        this.gameObject.GetComponent<TextMesh>().text = txt;
        textBounds = this.gameObject.GetComponent<Renderer>().bounds;
        finalBounds();
    }
    public override void changeColor(Color color)
    {
        this.gameObject.GetComponent<TextMesh>().color = color;
    }
    public override void Push()
    {
        GameObject frame = GameObject.Find("Frames");
        if (HUD != null && frame != null)
            HUD.Push(frame.transform.Find("DoodleFrame").gameObject.GetComponent<HUDFrame>());
    }
}
