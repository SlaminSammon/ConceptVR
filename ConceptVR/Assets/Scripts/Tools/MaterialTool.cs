using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTool : Tool {

    public Material newMat;
    HUDManager HUD;

    // Use this for initialization
    void Start()
    {
        HUD = GameObject.Find("Managers").GetComponent<HUDManager>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override bool Tap(Vector3 position)
    {
        Face nearestFace = DCGBase.NearestFace(position, 0.07f);
        if (nearestFace != null && !nearestFace.isLocked)
        {
            nearestFace.material = newMat;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void changeMaterial(Material newMaterial)
    {
        newMat = newMaterial;
    }

}
