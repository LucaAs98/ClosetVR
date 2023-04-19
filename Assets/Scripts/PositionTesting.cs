using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PositionTesting : MonoBehaviour
{

    [SerializeField] Transform rightFoot;
    [SerializeField] Transform leftFoot;

    private Vector3 middlePointFeet;
    private Vector3 newMiddleFeet;

    private Vector3 headBodyOffset;
    public Vector3 offset;
    public Vector3 offsetPoint;

    void Start()
    {
    }

    void Update()
    {


        Vector3 newLeft = new Vector3(Mathf.Abs(leftFoot.forward.x * 100), 0, Mathf.Abs(leftFoot.forward.z * 100));
        Vector3 newRight = new Vector3(Mathf.Abs(rightFoot.forward.x * 100), 0, Mathf.Abs(rightFoot.forward.z * 100));

        middlePointFeet = Vector3.Lerp(newLeft, newRight, 0.5f);
        newMiddleFeet = new Vector3(middlePointFeet.x, transform.position.y, middlePointFeet.z);

        transform.LookAt(newMiddleFeet);

    }


    private void OnDrawGizmos()
    {

        Gizmos.DrawLine(rightFoot.position, rightFoot.forward * 100);
        Vector3 test;

        if (rightFoot.forward.z <= 0) { 
            test = new Vector3(rightFoot.forward.x * 100, 0, -rightFoot.forward.z * 100);
        }
        else
        {
            test = new Vector3(rightFoot.forward.x * 100, 0, rightFoot.forward.z * 100);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rightFoot.position, test);
        Debug.Log(test);

        Vector3 testL;

        if (leftFoot.forward.z <= 0)
        {
            testL = new Vector3(leftFoot.forward.x * 100, 0, -leftFoot.forward.z * 100);
        }
        else
        {
            testL = new Vector3(leftFoot.forward.x * 100, 0, leftFoot.forward.z * 100);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(leftFoot.position, testL);

        Gizmos.DrawSphere(testL, 0.2f);

        Gizmos.DrawSphere(newMiddleFeet, 1f);
    }


}   
