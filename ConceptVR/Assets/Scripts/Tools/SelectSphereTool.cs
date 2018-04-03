﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSphereTool : SelectTool
{
    public Material selSphereMat;

    float selDist = 0.05f;

    // Use this for initialization
    void Start()
    {
        base.Start();   
    }

    new private void Update()
    {
        if (triggerInput)
        {
            DCGElement nElement;
            Item nItem;
            while (true)
            {
                float playerScale = GameObject.Find("Managers").GetComponent<SettingsManager>().playerScale;
                nElement = DCGBase.NearestElement(controllerPosition, selDist * playerScale);
                if (nElement == null)
                    break;
                else
                {
                    TapDCG(nElement);
                    break;
                }
                base.Update();
            }
        }

    }

    private void OnRenderObject()
    {
        float playerScale = GameObject.Find("Managers").GetComponent<SettingsManager>().playerScale;
        if (triggerInput)
        {
            selSphereMat.SetPass(0);
            Graphics.DrawMeshNow(GeometryUtil.icoSphere4, Matrix4x4.TRS(controllerPosition, Quaternion.identity, new Vector3(selDist, selDist, selDist) * playerScale));
        }
        base.OnRenderObject();
    }
}
