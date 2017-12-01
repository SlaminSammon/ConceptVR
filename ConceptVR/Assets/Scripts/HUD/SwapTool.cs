using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapTool : MonoBehaviour {

    HUDManager HUD;
    Controller controller;
    public string tool;
    public Type toolType;
    float cdTime;

    // Use this for initialization
    void Start()
    {
        HUD = GameObject.Find("Managers").GetComponent<HUDManager>();
        controller = GameObject.Find("LoPoly_Rigged_Hand_Right").GetComponent<handController>();
        if (controller == null)
            Debug.Log("Fuck");
        cdTime = HUD.getCooldownTime();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "bone3" || collision.gameObject.name == "bone2")
        {
            Leap.Unity.FingerModel finger = collision.gameObject.GetComponentInParent<Leap.Unity.FingerModel>();

            if (finger && finger.fingerType.ToString() == "TYPE_INDEX")
            {
                cdTime = HUD.getCooldownTime();

                if (Time.time > cdTime)
                {
                    // add swap tool function right here swaptool(tool);
                    controller.changeTool(tool);
                    HUD.updateToolButtonColor(tool);
                    HUD.setCooldownTime(Time.time + HUD.getCooldown());


                }

            }
        }
    }
}
