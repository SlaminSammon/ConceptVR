using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDFrame: MonoBehaviour
{
    bool isSubFrame = false;
    enum animationState { In, Out, Idle };
    List<HUDView> viewList;

    void Update(){
        // handle animation
        // call OnUpdate
        // check for presses
    }

    // called when the frame begins animating in
    void OnEnable(){

    }

    // called when the frame finishes animating out
    void OnDisable(){

    }

    // called by base Update function each update
    void OnUpdate(){

    }

    internal void toggleSettingsButton()
    {
        throw new NotImplementedException();
    }
}