using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class OculusLiteVR : MonoBehaviour {
    
    
    public GameObject leftHand;
    public GameObject rightHand;
    public Text debugText;

    private void Awake() {
        // listen for changes in state for tracking
        InputTracking.trackingAcquired += OnTrackingAcquired;
        InputTracking.trackingLost += OnTrackingLost;
        InputTracking.nodeAdded += OnNodeAdded;
        InputTracking.nodeRemoved += OnNodeRemoved;
        
        // see if hands are already connected
        CheckTrackedNodes();
    }

    private void OnDestroy() {
        // everything you listen to you have to stop listen to
        InputTracking.trackingAcquired -= OnTrackingAcquired;
        InputTracking.trackingLost -= OnTrackingLost;
        InputTracking.nodeAdded -= OnNodeAdded;
        InputTracking.nodeRemoved -= OnNodeRemoved;
    }

    private void OnNodeAdded(XRNodeState nodeState) {
        debugText.text = $"OnNodeAdded {nodeState.nodeType}\n" + debugText.text;
        CheckTrackedNodes();
    }

    private void OnNodeRemoved(XRNodeState nodeState) {
        debugText.text = $"OnNodeRemoved {nodeState.nodeType}\n" + debugText.text;
        CheckTrackedNodes();
    }

    private void OnTrackingAcquired(XRNodeState nodeState) {
        debugText.text = "OnTrackingAcquired\n" + debugText.text;
    }
    
    private void OnTrackingLost(XRNodeState nodeState) {
        debugText.text = "OnTrackingLost\n" + debugText.text;
    }




    private void CheckTrackedNodes() {
        // assume we dont track left or right hand. 
        leftHand.SetActive(false);
        rightHand.SetActive(false);
        
        var list = new List<XRNodeState>();
        InputTracking.GetNodeStates(list);
        for (int i = 0; i < list.Count; i++) {
            var nodeState = list[i];
            if (nodeState.nodeType == XRNode.LeftHand) {
                leftHand.SetActive(nodeState.tracked); // turn hand on if tracked
                UpdatePositionOfHand(nodeState, leftHand);
            }
            else if (nodeState.nodeType == XRNode.RightHand) {
                rightHand.SetActive(nodeState.tracked); // turn hand on if tracked
                UpdatePositionOfHand(nodeState, rightHand);
            }
        }
    }

    private void UpdatePositionOfHand(XRNodeState nodeState, GameObject hand) {
        if (!nodeState.tracked) { return; }

        if (nodeState.TryGetPosition(out var pos)) {
            hand.transform.localPosition = pos;
        }
        
        if (nodeState.TryGetRotation(out var rot)) {
            hand.transform.localRotation = rot;
        }
    }

    private void Update() {
        CheckTrackedNodes();
        
        // log button presses on controller to the text field
        if (Input.GetButton("Rift_A")) { Log("A button"); }
        if (Input.GetButton("Rift_B")) { Log("B button"); }
        if (Input.GetButton("Rift_X")) { Log("X button"); }
        if (Input.GetButton("Rift_Y")) { Log("Y button"); }
        
        
        LogAxis("Rift_Left_IndexTrigger");
        LogAxis("Rift_Left_HandTrigger");
        LogAxis("Rift_Right_IndexTrigger");
        LogAxis("Rift_Right_HandTrigger");
        
        LogThumb("Rift_Left_ThumbX", "Rift_Left_ThumbY", "Left thumb");
        LogThumb("Rift_Right_ThumbX", "Rift_Right_ThumbY", "Left thumb");
        
        if (Input.GetButton("Rift_Left_ThumbPress")) { Log("Left thumb press"); }
        if (Input.GetButton("Rift_Right_ThumbPress")) { Log("Right thumb press"); }
    }

    private void Log(string s) {
        debugText.text = s;
    }
    private void LogAxis(string axis) { // trigger buttons go from 0 to 1
        float pressure = Input.GetAxis(axis);
        if (pressure > 0.1f) {Log(axis + ":" + pressure);}
    }

    private void LogThumb(string x, string y, string label) { // thumb axis goes from -1 to 1
        float thumbX = Input.GetAxis(x);
        float thumbY = Input.GetAxis(y);
        if (thumbX < -0.1f || thumbX > 0.1f || thumbY < -0.1f || thumbY > 0.1f) {
            Log($"{label} x: {thumbX}, y: {thumbY}");
        }
    }
}
