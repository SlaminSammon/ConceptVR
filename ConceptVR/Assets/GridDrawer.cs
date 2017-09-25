using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDrawer : MonoBehaviour {
    public Material glowMat;
    Vector3 center;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        SetCenter(transform.position);
	}

    public void SetCenter(Vector3 pos)
    {
        center = pos;
        transform.position = new Vector3(Mathf.Floor(pos.x*5f)/5f, Mathf.Floor(pos.y*5f)/5f, Mathf.Floor(pos.z*5f)/5f);
        glowMat.SetVector("_Center", new Vector4(center.x, center.y, center.z, 0));
    }
}
