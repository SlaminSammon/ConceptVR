using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {

    const float SCALE_FACTOR = 2.75f;
    public GameObject scaleSlider;
    public float playerScale;

    GameObject LMHeadMountedRig;
    float startLMx;
    float startLMy;
    float startLMz;


    // Use this for initialization
    void Start () {
        LMHeadMountedRig = GameObject.Find("LMHeadMountedRig");
        playerScale = 1f;
        startLMx = LMHeadMountedRig.transform.position.x;
        startLMy = LMHeadMountedRig.transform.position.y;
        startLMz = LMHeadMountedRig.transform.position.z;

    }

    // Update is called once per frame
    void Update () {
        playerScale = scaleSlider.GetComponent<HUDSlider>().value * SCALE_FACTOR + 1f;
        LMHeadMountedRig.transform.localScale = new Vector3(playerScale,playerScale,playerScale);
        LMHeadMountedRig.transform.position = new Vector3(playerScale*startLMx, playerScale*startLMy, playerScale);
    }
}
