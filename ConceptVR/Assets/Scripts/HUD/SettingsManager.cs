using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
    public static SettingsManager sm;

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
        sm = this;

        LMHeadMountedRig = GameObject.Find("LMHeadMountedRig");
        playerScale = 1f;
    }
    
    void Update () {
        playerScale = scaleSlider.GetComponent<HUDSlider>().value * SCALE_FACTOR + 1f;
        LMHeadMountedRig.transform.localScale = new Vector3(playerScale,playerScale,playerScale);
    }

    public Vector3 snapToGrid(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x / gridSnap) * gridSnap, 
            Mathf.Round(v.y / gridSnap) * gridSnap, 
            Mathf.Round(v.z / gridSnap) * gridSnap);
    }
}
