using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class HandsUpDisplay : MonoBehaviour
{
    Leap.Controller leapcontroller;
    Leap.Frame frame;
    Leap.Hand lHand;

    public GameObject LeapHandController;

    // Use this for initialization
    void Start()
    {
        leapcontroller = new Leap.Controller();
    }

    // Update is called once per frame
    void Update()
    {

        frame = leapcontroller.Frame();

        // if there are hands visible in the view.
        if (frame.Hands.Count > 0)
        {
            lHand = frame.Hands[0];
            // If it is the left hand, make the position of the HUD relative to the local position of the palm.
            if (lHand.IsLeft)
            {

                // TODO: IMPLEMENT FOLLOW HAND

               // transform.position = new Vector3(lHand.PalmPosition.x, lHand.PalmPosition.y, lHand.PalmPosition.z) / 1000f; //+ new Vector3(0.0f, -0.37f, 0.0f);

                // THIS ONE WORKS WHEN IT IS A CHILD OF LEAP HAND CONTROLLER
                transform.localPosition = new Vector3(lHand.PalmPosition.x, lHand.PalmPosition.y, -lHand.PalmPosition.z) / 1000f + new Vector3(-0.0f, -1.15f, -1.15f);
            }
        }
    }
}