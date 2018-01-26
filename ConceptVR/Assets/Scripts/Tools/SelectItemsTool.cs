using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItemsTool : Tool {

    public Material newMat;
    public HUDFrame itemsFrame;

    HUDManager HUD;
    [HideInInspector]
    public GameObject selected;
    GameObject itembase;
    Material selectedMaterial;
    Color selectedLightColor;

	// Use this for initialization
	void Start () {
        HUD = GameObject.Find("Managers").GetComponent<HUDManager>();
        itembase = GameObject.Find("ItemBase");
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    public override void Swipe()
    {
        Deselect();
    }

    public override void Tap(Vector3 position)
    {
        Deselect();
        selected = findNearestItem();
        if (selected)
        {
            if (selected.tag == "Light")
            {
                selected.gameObject.GetComponent<MeshRenderer>().material = newMat;
                selected.gameObject.GetComponent<Light>().color = Color.blue;
            }
        }
    }

    public void Deselect()
    {
        if (selected)
        {
            if (selected.tag == "Light")
            {
                selected.gameObject.GetComponent<MeshRenderer>().material = selectedMaterial;
                selected.gameObject.GetComponent<Light>().color = selectedLightColor;
            }
            HUD.Pop();
            selected = null;
        }
    }

    GameObject findNearestItem()
    {
        GameObject nearestItem = null;
        float nearestDistance = 99999;
        float maxDistance = 0.1f; // maximum distance to consider an object as being intended to be selected

        foreach (GameObject light in itembase.GetComponent<ItemBase>().lights) {
            float distance = Vector3.Distance(controllerPosition, light.transform.position);
            if (distance < nearestDistance && distance < maxDistance)
            {
                nearestItem = light;
                nearestDistance = distance;
                selectedMaterial = light.GetComponent<MeshRenderer>().material;
                selectedLightColor = light.GetComponent<Light>().color;
            }
        }
        // TODO: same for text, or any other item that we add in the future

        if (nearestItem)
        {
            HUD.Push(itemsFrame);
        }
        return nearestItem;
    }
}
