using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightItem : Item
{
    Material newMat;
    Material selectedMaterial;
    Color selectedLightColor;
    public void Start()
    {
        base.Start();
        newMat = Resources.Load("Material/DCGMats/Select.mat", typeof(Material)) as Material;
        selectedMaterial = gameObject.GetComponent<MeshRenderer>().material;
        selectedLightColor = gameObject.GetComponent<Light>().color;
    }

    public override void Select()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = newMat;
        this.gameObject.GetComponent<Light>().color = Color.blue;
    }
    public override void DeSelect()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = selectedMaterial;
        this.gameObject.GetComponent<Light>().color = selectedLightColor;
    }
    public override void Push()
    {
        HUD.Push(GameObject.Find("ItemsFrame").GetComponent<HUDFrame>());
    }
}
