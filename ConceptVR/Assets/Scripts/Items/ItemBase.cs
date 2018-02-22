using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemBase : NetworkBehaviour {

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
    public static void Spawn(GameObject go)
    {
        NetworkServer.Spawn(go);
        foreach (KeyValuePair<UnityEngine.Networking.NetworkInstanceId, UnityEngine.Networking.NetworkIdentity> kvp in NetworkServer.objects)
            Debug.Log(kvp);
    }
    public Item findNearestItem(Vector3 position)
    {
        Item nearestItem = null;
        float nearestDistance = 99999;
        float maxDistance = 0.1f;
        foreach (Item item in items)
        {
            float distance = Vector3.Distance(position, item.Position(position));
            Debug.Log(distance);
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
