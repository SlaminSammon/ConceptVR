using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightItem : Item
{
    const float INTENSITY_SCALE = 10f;
    Material newMat;
    Material selectedMaterial;
    public Color selectedLightColor;

    public void Start()
    {
        base.Start();
        newMat = Resources.Load("Material/DCGMats/Select.mat", typeof(Material)) as Material;
        selectedMaterial = gameObject.GetComponent<MeshRenderer>().material;
        selectedLightColor = gameObject.GetComponent<Light>().color;
    }

    public override void CmdSelect()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = newMat;
        //this.gameObject.GetComponent<Light>().color = Color.blue;

    }
    public override void CmdDeSelect()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = selectedMaterial;
        this.gameObject.GetComponent<Light>().color = selectedLightColor;
    }
    public override void Push()
    {
        Debug.Log("Pushing Frame");
        GameObject frame = GameObject.Find("Frames");
        if (HUD != null && frame != null)
            HUD.Push(frame.transform.Find("LightFrame").gameObject.GetComponent<HUDFrame>());
    }
    public override Vector3 Position(Vector3 contPos)
    {
        return this.gameObject.transform.position;
    }

    public override void changeColor(Color color)
    {
        this.gameObject.GetComponent<Light>().color = color;
        selectedLightColor = color;
        base.changeColor(color);
    }

    public void changeIntensity(float intensity)
    {
        this.gameObject.GetComponent<Light>().intensity = intensity * INTENSITY_SCALE;
    }
}
