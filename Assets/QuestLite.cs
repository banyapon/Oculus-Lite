using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class QuestLite : MonoBehaviour
{
    public GameObject Head;
    public Rigidbody LeftHand, RightHand;
    private List<XRNodeState> mNodeStates = new List<XRNodeState>();
    private Vector3 mHeadPos, mLeftHandPos, mRightHandPos;
    private Quaternion mHeadRot, mLeftHandRot, mRightHandRot;
    public Text debugText;

    // Start is called before the first frame update
    void Awake()
    {
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);
        for (int i = 0; i < subsystems.Count; i++)
        {
            subsystems[i].TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor);
        }

    }

    // Update is called once per frame
    void Update()
    {
        InputTracking.GetNodeStates(mNodeStates);

        foreach (XRNodeState nodeState in mNodeStates)
        {
            switch (nodeState.nodeType)
            {
                case XRNode.Head:
                    nodeState.TryGetPosition(out mHeadPos);
                    nodeState.TryGetRotation(out mHeadRot);
                    break;
            }
        }
        Head.transform.position = mHeadPos;
        Head.transform.rotation = mHeadRot.normalized;


        //Test control
        if (Input.GetButton("Rift_A")) { showLog("A button"); }
        if (Input.GetButton("Rift_B")) { showLog("B button"); }
        if (Input.GetButton("Rift_X")) { showLog("X button"); }
        if (Input.GetButton("Rift_Y")) { showLog("Y button"); }
        
        
        TriggerButton("Rift_Left_IndexTrigger");
        TriggerButton("Rift_Left_HandTrigger");
        TriggerButton("Rift_Right_IndexTrigger");
        TriggerButton("Rift_Right_HandTrigger");
        
        ThumbButton("Rift_Left_ThumbX", "Rift_Left_ThumbY", "Left thumbstick");
        ThumbButton("Rift_Right_ThumbX", "Rift_Right_ThumbY", "Left thumbstick");
        
        if (Input.GetButton("Rift_Left_ThumbPress")) { showLog("Left thumbstick press"); }
        if (Input.GetButton("Rift_Right_ThumbPress")) { showLog("Right thumbstick press"); }

    }

    // FixedUpdate stays in sync with the physics engine.
    private void FixedUpdate()
    {
        InputTracking.GetNodeStates(mNodeStates);

        foreach (XRNodeState nodeState in mNodeStates)
        {
            switch (nodeState.nodeType)
            {
                case XRNode.LeftHand:
                    nodeState.TryGetPosition(out mLeftHandPos);
                    nodeState.TryGetRotation(out mLeftHandRot);
                    break;
                case XRNode.RightHand:
                    nodeState.TryGetPosition(out mRightHandPos);
                    nodeState.TryGetRotation(out mRightHandRot);
                    break;
            }
        }

        LeftHand.MovePosition(mLeftHandPos);
        LeftHand.MoveRotation(mLeftHandRot.normalized);
        RightHand.MovePosition(mRightHandPos);
        RightHand.MoveRotation(mRightHandRot.normalized);
    }

    private void TriggerButton(string axis) {
        float pressure = Input.GetAxis(axis);
        if (pressure > 0.1f) {showLog(axis + ":" + pressure);}
    }

    private void ThumbButton(string x, string y, string label) { // thumb axis goes from -1 to 1
        float thumbX = Input.GetAxis(x);
        float thumbY = Input.GetAxis(y);
        if (thumbX < -0.1f || thumbX > 0.1f || thumbY < -0.1f || thumbY > 0.1f) {
            showLog($"{label} x: {thumbX}, y: {thumbY}");
        }
    }
    private void showLog(string stringData) {
        debugText.text = stringData;
    }

}
