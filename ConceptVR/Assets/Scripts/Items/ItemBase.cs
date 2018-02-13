using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour {

    public static List<Item> items;
    public static List<Item> sItems;
    public GameObject LightPrefab;
    public string firstType;
    public bool isHUD = false;

    // Use this for initialization
    void Start () {
        firstType = "";
        items = new List<Item>();
        sItems = new List<Item>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Item findNearestItem(Vector3 position)
    {
        Item nearestItem = null;
        float nearestDistance = 99999;
        float maxDistance = 0.1f; // maximum distance to consider an object as being intended to be selected

        foreach (Item item in items)
        {
            float distance = Vector3.Distance(position, item.Position(position));
            if (distance < nearestDistance && distance < maxDistance)
            {
                nearestItem = item;
                nearestDistance = distance;
            }
        }
        // TODO: same for text, or any other item that we add in the future

        return nearestItem;
    }
    public void itemHudManager(Item item)
    {
        if(firstType == "")
        {
            firstType = item.GetType().ToString();
            item.Push();
            isHUD = true;
            return;
        }
        if(firstType != item.GetType().ToString())
        {
            Item.Pop();
            isHUD = false;
        }
        
    }

}
