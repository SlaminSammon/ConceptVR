using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
public struct GestureEventArgs
{

}
public delegate void GestureEventHandler();
public class LeapTrackedController : MonoBehaviour
{
    private Leap.Hand hand;
    private HandsUtil util;
    private LeapServiceProvider leapProvider;
    public bool pinchHeld = false;
    public bool grabHeld = false;
    public Vector3 position;
    public string handedness;
    public int toolIndex = 0;
    public event GestureEventHandler pinchMade;
    public event GestureEventHandler pinchGone;
    public event GestureEventHandler grabMade;
    public event GestureEventHandler grabGone;

    // Use this for initialization
    void Start()
    {
        util = new HandsUtil();

    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if a pinch is being held.
        bool pinch = checkPinch();
        bool grab = false; 
        if (!pinch) grab = checkGrab();
        if (pinch && !pinchHeld && !grab)
        {
            pinchHeld = true;
            OnPinchHeld();
        }
        else if (!pinch && pinchHeld && !grab)
        {
            pinchHeld = false;
            OnPinchGone();
        }
        //Check to see if grab is held
        if (grab && !grabHeld)
        {
            grabHeld = true;
            OnGrabHeld();
        }
        else if (!grab && grabHeld)
        {
            grabHeld = false;
            OnGrabGone();
        }
    }

    public void OnPinchHeld()
    {
        if (pinchMade != null)
            pinchMade();
    }
    public void OnPinchGone()
    {
        if (pinchGone != null)
            pinchGone();
    }
    public void OnGrabHeld()
    {
        if (grabMade != null)
            grabMade();
    }
    public void OnGrabGone()
    {
        if (grabGone != null)
            grabGone();
    }
    public bool checkPinch()
    {
        //Determine which hand.
        if(handedness == "Right")
            hand = Hands.Right;
        else
            hand = Hands.Left;
        //Check if hand exists
        if (hand == null)
            return false;
        //Get position and value of Pinch
        position = util.getIndexPos(hand);
        return util.IsPinching(hand);
    }
    public bool checkGrab()
    {
        if (handedness == "Right")
            hand = Hands.Right;
        else
            hand = Hands.Left;
        if (hand == null)
            return false;
        position = util.getIndexPos(hand);
        return util.IsGrabbing(hand);
        
    }
}
