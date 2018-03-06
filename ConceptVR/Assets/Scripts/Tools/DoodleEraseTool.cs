using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleEraseTool : Tool {
    public Material destMat;

    float delDist = 0.07f;

    // Use this for initialization
    void Start()
    {

    }

    new private void Update()
    {
        if (triggerInput)
        {
            foreach (Doodle d in ItemBase.items)
            {
                d.eraseSphere(controllerPosition, delDist);
            }
        }
    }

    private void OnRenderObject()
    {
        float playerScale = GameObject.Find("Managers").GetComponent<SettingsManager>().playerScale;
        if (triggerInput)
        {
            destMat.SetPass(0);
            Graphics.DrawMeshNow(GeometryUtil.icoSphere4, Matrix4x4.TRS(controllerPosition, Quaternion.identity, new Vector3(delDist, delDist, delDist) * playerScale));
        }
    }
}
