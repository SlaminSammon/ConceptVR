using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item: MonoBehaviour  {
    public bool isSelected;
    public bool isLocked;
    protected static HUDManager HUD;
    // Use this for initialization
    protected void Start () {
        HUD = GameObject.Find("Managers").GetComponent<HUDManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual Vector3 Distance() { return new Vector3(); }
    public virtual void Select() { }
    public virtual void DeSelect() { }
    public virtual void Push() { }
    public virtual void changeColor(Color color) { }
    public static void Pop()
    {
        HUD.Pop();
    }
    public void OnDestroy()
    {
        ItemBase.items.Remove(this);
    }
}
