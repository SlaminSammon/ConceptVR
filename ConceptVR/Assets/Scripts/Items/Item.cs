using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Item: NetworkBehaviour  {
    [SyncVar]
    public bool isSelected;
    [SyncVar]
    public bool isLocked;
    protected static HUDManager HUD;
    // Use this for initialization
    protected void Start () {
        HUD = GameObject.Find("Managers").GetComponent<HUDManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual float Distance(Vector3 pos) { return -1f; }
    [Command]
    public virtual void CmdSelect() { }
    [Command]
    public virtual void CmdDeSelect() { }
    public virtual void Push() { }
    public virtual void changeColor(Color color) { }
    public virtual Vector3 Position(Vector3 contPos) { return new Vector3(); }
    public static void Pop()
    {
        HUD.Pop();
    }
    public void OnDestroy()
    {
        ItemBase.items.Remove(this);
    }
}
