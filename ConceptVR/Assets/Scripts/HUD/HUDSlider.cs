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
            targetValue = Mathf.Clamp01(Vector3.Project(localPressPos, positiveRange).magnitude / positiveRange.magnitude / 2 + 0.5f);
        }
        value = (value * friction + targetValue);
        sliderObject.transform.position = (value * 2 - 1) * positiveRange;
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
