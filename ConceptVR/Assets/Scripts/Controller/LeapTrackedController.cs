using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
public struct GestureEventArgs
{

}
public struct FrameInformation
{
    bool grabHeld;
    bool pinchHeld;
    HandInformation hand;
    FingerInformation thumb;
    FingerInformation index;
    FingerInformation middle;
    FingerInformation ring;
    FingerInformation pinky;
}
public struct FingerInformation
{
    bool isExtrude;
    Vector3 direction;
    Vector3 tipPosition;
    Vector3 tipVelocity;
}
public struct HandInformation
{
    Vector3 palmVelocity;
    Vector3 palmPosition;
    Vector3 direction;
    float pitch;
    float yaw;
    float roll;
    Quaternion rotation;
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
        bool grab = checkGrab();
        bool pinch = false;
        if (!grab)
        {
            if(grabHeld)
                OnGrabGone();
            Debug.Log("Not Grabbing");
            pinch = checkPinch();
        }
        if(!pinch && pinchHeld) OnPinchGone();
        if (grab && !grabHeld)
        {
            Debug.Log("Grabbing");
            OnGrabHeld();
        }
        else if(pinch && !pinchHeld)
        {
            OnPinchHeld();
        }
    }

    public void OnPinchHeld()
    {
        pinchHeld = true;
        if (pinchMade != null)
            pinchMade();
    }
    public void OnPinchGone()
    {
        pinchHeld = false;
        if (pinchGone != null)
            pinchGone();
    }
    public void OnGrabHeld()
    {
        grabHeld = true;
        if (grabMade != null)
            grabMade();
    }
    public void OnGrabGone()
    {
        grabHeld = false;
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
    //in progress
    public bool checkThumbsUp()
    {
        if (handedness == "Right")
            hand = Hands.Right;
        else
            hand = Hands.Left;
        if (hand == null)
            return false;
        position = util.getThumbPos(hand);
        return true;
        //return util.IsThumbsUp();
    }
}
