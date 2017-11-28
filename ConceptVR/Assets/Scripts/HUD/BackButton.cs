using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour {

    HUDManager HUD;
    float cdTime;

    // Use this for initialization
    void Start()
    {
        HUD = GameObject.Find("Managers").GetComponent<HUDManager>();
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
                    HUD.Pop();
                    HUD.setCooldownTime(Time.time + HUD.getCooldown());
                }

            }
        }
    }
}
