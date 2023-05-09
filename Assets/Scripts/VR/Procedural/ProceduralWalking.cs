using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VivoxUnity;

public class ProceduralWalking : MonoBehaviour
{

    //BothFoot
    [SerializeField] GameObject RightFoot;
    [SerializeField] GameObject LeftFoot;
    private bool whichOneIsMoving;


    //Parameters of right position 
    private Vector3 currentRightPosition;
    private Vector3 standingRightPosition;
    private Vector3 tempRightPosition;

    private Vector3 newFootRightPosition;

    //Parameters of left position 
    private Vector3 currentLeftPosition;
    private Vector3 standingLeftPosition;
    private Vector3 tempLeftPosition;

    private Vector3 newFootLeftPosition;

    //Parameters of right rotation
    private Quaternion currentRightRotation;
    private float initialYRightRotation;
    
    //Parameters of left rotation
    private Quaternion currentLeftRotation;
    private float initialYLeftRotation;


    //Parameters for raycasting on ground
    [SerializeField] Transform headTransform;

    //Step parameters
    public float distanceForStep;
    public float stepLenght;
    public float footSpacing;
    private float lerpRightFoot;
    public float stepHeight;
    public float stepSpeed;

    Vector3 tempCurrentRight;

    //Offset betwen ground and feet
    public float YOffsetForFoot;
    public Vector3 footOffset;

    //Timer value
    private float timerForResettingTransform;


    private float timeForNextStep;


    private void Awake()
    {
        currentRightPosition = RightFoot.transform.position;
        currentRightRotation = RightFoot.transform.rotation;
        standingRightPosition = RightFoot.transform.localPosition;
        initialYRightRotation = RightFoot.transform.rotation.eulerAngles.y;

        currentLeftPosition = LeftFoot.transform.position;
        currentLeftRotation = LeftFoot.transform.rotation;
        standingLeftPosition = LeftFoot.transform.localPosition;
        initialYLeftRotation = LeftFoot.transform.rotation.eulerAngles.y;

        whichOneIsMoving = true;
        timeForNextStep = 0f;

        lerpRightFoot = 1f;
        tempCurrentRight = new Vector3();
    }

    private void Update()
    {

        timerForResettingTransform += Time.deltaTime;
        timeForNextStep += Time.deltaTime;

        SetFootOnGround(RightFoot);
        SetFootOnGround(LeftFoot);

        LeftFoot.transform.position = currentLeftPosition;
        LeftFoot.transform.rotation = currentLeftRotation;

        RightFoot.transform.position = currentRightPosition;
        RightFoot.transform.rotation = currentRightRotation;

        RotateFeet();

        FeetMovement(RightFoot);

        //ReturnToDefaultTransform();
    }


    private void SetFootOnGround(GameObject WhichFoot)
    {
        RaycastHit hit;

        bool hasHit = Physics.Raycast(WhichFoot.transform.position + Vector3.up, Vector3.down, out hit);
        if (hasHit)
        {
            WhichFoot.transform.position = new Vector3(WhichFoot.transform.position.x, hit.point.y + YOffsetForFoot, WhichFoot.transform.position.z);
        }
    }


    private void FeetMovement(GameObject WhichFeet)
    {
        tempRightPosition = headTransform.position + (headTransform.right * footSpacing);
        newFootRightPosition = new Vector3(tempRightPosition.x, currentRightPosition.y, tempRightPosition.z);

        //Set new position of foot
        if (Vector3.Distance(newFootRightPosition, currentRightPosition) > distanceForStep && lerpRightFoot >= 1)
        {
            Debug.Log("Fai passo");
            lerpRightFoot = 0f;

            Vector3 directionOfStep = (newFootRightPosition - currentRightPosition);
            tempCurrentRight = currentRightPosition + (directionOfStep * stepLenght);
            //timerForResettingTransform = 0f;
        }

        if (lerpRightFoot < 1)
        {
            Vector3 tempPos = Vector3.Lerp(currentRightPosition, tempCurrentRight, lerpRightFoot);
            tempPos.y += Mathf.Sin(lerpRightFoot * Mathf.PI) * stepHeight;
            currentRightPosition = tempPos;
            lerpRightFoot += Time.deltaTime * stepSpeed;
        }
  
        /*
        tempRightPosition = headTransform.position + (headTransform.right * footSpacing);
        newFootRightPosition = new Vector3(tempRightPosition.x, currentRightPosition.y, tempRightPosition.z);

        //Set new position of foot
        if (Vector3.Distance(newFootRightPosition, currentRightPosition) > distanceForStep && lerpRightFoot >= 1)
        {
            lerpRightFoot = 0f;

            Vector3 directionOfStep = (newFootRightPosition - currentRightPosition);
            tempCurrentRight = currentRightPosition + (directionOfStep * stepLenght);
            //timerForResettingTransform = 0f;
        }

        if (lerpRightFoot < 1)
        {
            Vector3 tempPos = Vector3.Lerp(currentRightPosition, tempCurrentRight, lerpRightFoot);
            tempPos.y += Mathf.Sin(lerpRightFoot * Mathf.PI) * stepHeight;
            currentRightPosition = tempPos;
            lerpRightFoot += Time.deltaTime * stepSpeed;
        }
            /*
        else
        {
            tempLeftPosition = headTransform.position + (headTransform.right * (-footSpacing));
            newFootLeftPosition = new Vector3(tempLeftPosition.x, currentLeftPosition.y, tempLeftPosition.z);

            //Set new position of foot
            if (Vector3.Distance(newFootLeftPosition, currentLeftPosition) > distanceForStep)
            {
                Vector3 directionOfStep = (newFootLeftPosition - currentLeftPosition);
                currentLeftPosition = currentLeftPosition + (directionOfStep * stepLenght);
                whichOneIsMoving = true;
                timeForNextStep = 0f;
                timerForResettingTransform = 0f;
            }
        }*/
    }

    private void RotateFeet()
    {
        Quaternion temp1 = Quaternion.Euler(0, RightFoot.transform.rotation.eulerAngles.y, 0);
        Quaternion temp2 = Quaternion.Euler(0, headTransform.transform.rotation.eulerAngles.y, 0);
        float difference = Quaternion.Angle(temp1,temp2);


        if (difference >= 30 || difference <= -30)
        {
            currentRightRotation = Quaternion.Euler(currentRightRotation.eulerAngles.x, headTransform.transform.rotation.eulerAngles.y + initialYRightRotation, currentRightRotation.eulerAngles.z);
            currentLeftRotation = Quaternion.Euler(currentLeftRotation.eulerAngles.x, headTransform.transform.rotation.eulerAngles.y + initialYLeftRotation, currentLeftRotation.eulerAngles.z);
        }
    }


    private void ReturnToDefaultTransform()
    {
        //Return to standing position if no movement (IDLE)
        if (timerForResettingTransform > 2f)
        {
            RightFoot.transform.localPosition = standingRightPosition;
            LeftFoot.transform.localPosition = standingLeftPosition;
            currentRightPosition = RightFoot.transform.position;
            currentLeftPosition = LeftFoot.transform.position;


            currentRightRotation = Quaternion.Euler(currentRightRotation.eulerAngles.x, headTransform.transform.rotation.eulerAngles.y + initialYRightRotation, currentRightRotation.eulerAngles.z);
            currentLeftRotation = Quaternion.Euler(currentLeftRotation.eulerAngles.x, headTransform.transform.rotation.eulerAngles.y + initialYLeftRotation, currentLeftRotation.eulerAngles.z);


            timerForResettingTransform = 0f;
        }

        //If the position is the same reset timer
        if ((RightFoot.transform.localPosition == standingRightPosition) && (LeftFoot.transform.localPosition == standingLeftPosition))
        {
            timerForResettingTransform = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(newFootForward, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(gameObject.transform.position, transform.forward * 100);

        Gizmos.DrawLine(headTransform.position, headTransform.transform.forward * 100);
    }
}
