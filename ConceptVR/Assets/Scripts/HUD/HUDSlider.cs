using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDSlider : HUDButton {
    public float friction;
    public Vector3 positiveRange;
    public float defaultValue;

    [HideInInspector] public float value;
    float targetValue;
    Vector3 localPressPos;
    GameObject sliderObject;
    bool touching;

    void Start()
    {
        base.Start();
        value = defaultValue;
        sliderObject = transform.Find("SlidingView").gameObject;
    }

    void Update () {
        base.Update();
        if (touching)
        {
            localPressPos = transform.worldToLocalMatrix * fingerTip.transform.position;
            targetValue = Vector3.Project(localPressPos - positiveRange, positiveRange).magnitude;
            Debug.Log(targetValue);
            targetValue = Mathf.Clamp01(targetValue);
        }
        value = (value * friction + targetValue) / (1+friction);
        sliderObject.transform.localPosition = (value * 2 - 1) * positiveRange;
	}

    public override void OnPress()
    {
        touching = true;
    }

    public override void OnRelease()
    {
        touching = false;
    }
}
