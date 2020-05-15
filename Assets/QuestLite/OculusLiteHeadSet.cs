using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class OculusLiteHeadSet : MonoBehaviour
{
    public GameObject Head;

    private List<XRNodeState> mNodeStates = new List<XRNodeState>();
    private Vector3 mHeadPos;
    private Quaternion mHeadRot;

    private void Start()
    {
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);
        for (int i = 0; i < subsystems.Count; i++)
        {
            subsystems[i].TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor);
        }
    }

    private void Update()
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
    }
}
