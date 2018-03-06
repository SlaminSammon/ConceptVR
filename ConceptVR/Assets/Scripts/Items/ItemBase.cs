using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemBase : NetworkBehaviour {
    public static ItemBase itemBase;

    public static List<Item> items;
    public static List<Item> sItems;
    public GameObject LightPrefab;
    public GameObject DoodlePrefab;
    public GameObject TextPrefab;
    public string firstType;
    public bool isHUD = false;

    // Use this for initialization
    void Start () {
        firstType = "";
        items = new List<Item>();
        sItems = new List<Item>();

        itemBase = this;
    }
	
	// Update is called once per frame
	void Update () {

    }
    //Spawns the object on the Server. This may be a bad function and can be handled in the specific tools.
    public static void Spawn(GameObject go)
    {
        NetworkServer.Spawn(go);
    }
    public static void DeSpawn(GameObject go)
    {
        NetworkServer.Destroy(go);
    }
    public Item findNearestItem(Vector3 position)
    {
        Item nearestItem = null;
        float nearestDistance = 99999;
        float maxDistance = 0.1f;
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
    //Not gonna lie this is pretty ghetto. It works though so i dunno
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
