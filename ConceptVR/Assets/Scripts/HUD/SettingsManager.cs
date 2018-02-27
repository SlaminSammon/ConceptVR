using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {

    const float SCALE_FACTOR = 2.75f;

    public float gridSnap = 1f/64f;
    public float rotationSnap = 15f;
    public bool snapEnabled = false;
    public GameObject scaleSlider;
    public float playerScale = 1f;

    GameObject LMHeadMountedRig;
    float startLMx;
    float startLMy;
    float startLMz;

    
    void Start () {
        LMHeadMountedRig = GameObject.Find("LMHeadMountedRig");
        playerScale = 1f;
        startLMx = LMHeadMountedRig.transform.position.x;
        startLMy = LMHeadMountedRig.transform.position.y;
        startLMz = LMHeadMountedRig.transform.position.z;

    }
    
    void Update () {
        playerScale = scaleSlider.GetComponent<HUDSlider>().value * SCALE_FACTOR + 1f;
        LMHeadMountedRig.transform.localScale = new Vector3(playerScale,playerScale,playerScale);
        LMHeadMountedRig.transform.position = new Vector3(playerScale*startLMx, playerScale*startLMy, playerScale);
    }
}
