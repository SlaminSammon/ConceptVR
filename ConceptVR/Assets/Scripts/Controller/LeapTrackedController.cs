using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;
public struct GestureEventArgs
{

}
public struct FrameInformation
{
    public bool grabHeld;
    public bool pinchHeld;
    public HandInformation hand;
    public FingerInformation thumb;
    public FingerInformation index;
    public FingerInformation middle;
    public FingerInformation ring;
    public FingerInformation pinky;
}
public struct FingerInformation
{
    public bool isExtrude;
    public Vector3 direction;
    public Vector3 tipPosition;
    public Vector3 tipVelocity;
}
public struct HandInformation
{
    public Vector3 palmVelocity;
    public Vector3 palmPosition;
    public Vector3 direction;
    public float pitch;
    public float yaw;
    public float roll;
    public Quaternion rotation;
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
        return util.checkThumbsUp(hand);
    }
    FrameInformation fetchFrameInformation(Leap.Hand hand)
    {
        FrameInformation frameInfo;
        frameInfo.grabHeld = grabHeld;
        frameInfo.pinchHeld = pinchHeld;
        foreach(Leap.Finger f in hand.Fingers)
        {
            FingerInformation fingerInfo;
            fingerInfo.isExtrude = f.IsExtrude;
            fingerInfo.direction = f.Direction.toVector3();
            fingerInfo.tipPosition = f.TipPosition.toVector3();
            fingerInfo.tipVelocity = f.TipVelocity.toVector3();
            switch (f.type)
            {
                case FingerType.TYPE_INDEX:
                    frameInfo.index = fingerInfo;
                    break;
                case FingerType.TYPE_MIDDLE:
                    frameInfo.middle = fingerInfo;
                    break;
                case FingerType.TYPE_RING:
                    frameInfo.ring = fingerInfo;
                    break;
                case FingerType.TYPE_PINKY:
                    frameInfo.pinky = fingerInfo;
                    break;
                case FingerType.TYPE_THUMB:
                    frameInfo.thumb = fingerInfo;
                    break;
            }
        }
        if(handedness == "Right")
            hand = Hands.Right;
        else
            hand = Hands.Left;
        HandInformation handInfo;
        handInfo.direction = hand.Direction.toVector3();
        handInfo.pitch = hand.Direction.pitch;
        handInfo.roll = hand.Direction.roll;
        handInfo.yaw = hand.Direction.yaw;
        handInfo.palmPosition = hand.palmPosition.toVector3();
        handInfo.palmVelocity = hand.palmvVelocity.toVector3();
        handInfo.rotation = hand.Rotation.ToQuaternion();
        return frameInfo;

    }
}
