using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;


[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }

}

public class VRRig : MonoBehaviour
{

    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Transform headConstraint;
    public Vector3 headBodyOffset;

    [SerializeField] GameObject spine;
    [SerializeField] GameObject leftController;
    [SerializeField] GameObject rightController;

    private Vector3 middlePoint;
    private Vector3 newMiddle;

    public float smoothness = 5f;

    void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;

        headBodyOffset = new Vector3(0, headBodyOffset.y, 0);
    }

    void Update()
    {
        transform.position = headConstraint.position + headBodyOffset;
        //transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized, Time.deltaTime * smoothness);


        middlePoint = Vector3.Lerp(leftController.transform.forward * 100, leftController.transform.forward * 100, 0.5f);
        newMiddle = new Vector3(middlePoint.x, spine.transform.position.y, middlePoint.z);

        spine.transform.forward = Vector3.Lerp(spine.transform.forward, Vector3.ProjectOnPlane(newMiddle, Vector3.up).normalized, Time.deltaTime * smoothness);

        head.Map();
        leftHand.Map();
        rightHand.Map();

    }

}

