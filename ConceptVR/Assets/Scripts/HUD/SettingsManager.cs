using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {

    const float SCALE = 10;
    public GameObject scaleSlider;
    public float playerScale;

    public GameObject LMHeadMountedRig;

    // Use this for initialization
    void Start () {
        playerScale = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        playerScale = scaleSlider.GetComponent<HUDSlider>().value * SCALE + 1f;
        LMHeadMountedRig.transform.localScale = new Vector3(playerScale,playerScale,playerScale);
    }
}
