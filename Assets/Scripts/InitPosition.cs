using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class InitPosition : MonoBehaviour
{

    public Transform cameraOffset;
    [SerializeField] Transform headOffset;

    [SerializeField] Transform rightArm;
    [SerializeField] Transform leftArm;
    [SerializeField] Transform spine;

    [SerializeField] Transform rightFoot;
    [SerializeField] Transform leftFoot;

    [SerializeField] Transform controllerLeft;
    [SerializeField] Transform controllerRight;

    private Vector3 middlePoint;
    private Vector3 newMiddle;
    private Vector3 middlePointFeet;
    private Vector3 newMiddleFeet;

    private Vector3 headBodyOffset;
    public Vector3 offset;
    public Vector3 offsetPoint;

    void Start()
    {
        headBodyOffset = transform.position - (headOffset.transform.position);
    }

    void Update()
    {
        transform.position = cameraOffset.position + headBodyOffset + offset;


        middlePoint = Vector3.Lerp(controllerRight.forward * 100, controllerLeft.forward * 100, 0.5f);
        newMiddle = new Vector3(middlePoint.x, spine.position.y, middlePoint.z);

        Vector3 newLeft = new Vector3((leftFoot.forward.x * 100), 0, (leftFoot.forward.z * 100));
        Vector3 newRight = new Vector3((rightFoot.forward.x * 100), 0, (rightFoot.forward.z * 100));

        middlePointFeet = Vector3.Lerp(newLeft, newRight, 0.5f);
        newMiddleFeet = new Vector3(middlePointFeet.x, transform.position.y, middlePointFeet.z);

        spine.LookAt(newMiddle);
        transform.LookAt(newMiddleFeet);

    }



}   
