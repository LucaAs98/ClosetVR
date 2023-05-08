using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Vector3 newFootRightForward;


    //Parameters of left position 
    private Vector3 currentLeftPosition;
    private Vector3 standingLeftPosition;
    private Vector3 tempLeftPosition;

    private Vector3 newFootLeftPosition;
    private Vector3 newFootLeftForward;


    private float headYRotation;
    [SerializeField] Transform forwardRotation;
    [SerializeField] GameObject forwardRotationGameObject;

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
    public float speed;


    //Offset betwen ground and feet
    public float YOffsetForFoot;
    public Vector3 footOffset;

    //Timer value
    private float timerForRightResetting;
    private bool stepHappensRight = false;
    private float timerForLeftResetting;
    private bool stepHappensLeft = false;


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
    }

    private void Update()
    {

        timerForRightResetting += Time.deltaTime;
        timerForLeftResetting += Time.deltaTime;

        SetFootOnGround(RightFoot);
        SetFootOnGround(LeftFoot);

        if (whichOneIsMoving)
        {
            //Vector3.Lerp(gameObject.transform.position, currentPosition, Time.deltaTime);
            RightFoot.transform.position = currentRightPosition;
            RightFoot.transform.rotation = currentRightRotation;

            FeetMovement(RightFoot);
        }
        else
        {
            //Vector3.Lerp(gameObject.transform.position, currentPosition, Time.deltaTime);
            LeftFoot.transform.position = currentLeftPosition;
            LeftFoot.transform.rotation = currentLeftRotation;

            FeetMovement(LeftFoot);
        }

        ReturnToDefaultPosition(RightFoot);
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
        if(WhichFeet == RightFoot)
        {
            tempRightPosition = headTransform.position + (headTransform.right * footSpacing);
            newFootRightPosition = new Vector3(tempRightPosition.x, currentRightPosition.y, tempRightPosition.z);


            //Rotate feet
            headYRotation = forwardRotation.rotation.eulerAngles.y;
            currentRightRotation = Quaternion.Euler(currentRightRotation.eulerAngles.x, headYRotation + initialYRightRotation, currentRightRotation.eulerAngles.z);


            //Set new position of foot
            if (Vector3.Distance(newFootRightPosition, currentRightPosition) > distanceForStep)
            {
                Vector3 directionOfStep = (newFootRightPosition - currentRightPosition);
                currentRightPosition = currentRightPosition + (directionOfStep * stepLenght);
                forwardRotationGameObject.GetComponent<ChangeBodyRotation>().timeForResetting = 0;
                stepHappensRight = true;
            }

            whichOneIsMoving = false;
        }
        else
        {
            tempLeftPosition = headTransform.position + (headTransform.right * (-footSpacing));
            newFootLeftPosition = new Vector3(tempLeftPosition.x, currentLeftPosition.y, tempLeftPosition.z);


            //Rotate feet
            headYRotation = forwardRotation.rotation.eulerAngles.y;
            currentLeftRotation = Quaternion.Euler(currentLeftRotation.eulerAngles.x, headYRotation + initialYLeftRotation, currentLeftRotation.eulerAngles.z);


            //Set new position of foot
            if (Vector3.Distance(newFootLeftPosition, currentLeftPosition) > distanceForStep)
            {
                Vector3 directionOfStep = (newFootLeftPosition - currentLeftPosition);
                currentLeftPosition = currentLeftPosition + (directionOfStep * stepLenght);
                forwardRotationGameObject.GetComponent<ChangeBodyRotation>().timeForResetting = 0;
                stepHappensLeft = true;
            }

            whichOneIsMoving = true;
        }


    }
    private void ReturnToDefaultPosition(GameObject WhichFeet)
    {
        /*
        //--------------------------------------------------------------------------
        //Return to standing position if no movement (STEP)
        if (timerForResetting > 1f && stepHappens == true)
        {
            isMoving = true;
            gameObject.transform.localPosition = standingPosition;
            currentPosition = gameObject.transform.position;

            stepHappens = false;
            timerForResetting = 0f;
            forwardRotationGameObject.GetComponent<ChangeBodyRotation>().timeForResetting = 0;
            isMoving = false;
        }

        //Return to standing position if no movement (IDLE)
        if (timerForResetting > 2f && gameObject.transform.localPosition != standingPosition)
        {
            isMoving = true;
            gameObject.transform.localPosition = standingPosition;
            currentPosition = gameObject.transform.position;

            timerForResetting = 0f;
            forwardRotationGameObject.GetComponent<ChangeBodyRotation>().timeForResetting = 0;
            isMoving = false;
        }

        //If the position is the same reset timer
        if (gameObject.transform.localPosition == standingPosition)
        {
            timerForResetting = 0f;
            forwardRotationGameObject.GetComponent<ChangeBodyRotation>().timeForResetting = 0;
        }
        */
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(newFootForward, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(gameObject.transform.position, transform.forward * 100);

        Gizmos.DrawLine(headTransform.position, headTransform.transform.forward * 100);
        Gizmos.DrawLine(forwardRotation.transform.position, forwardRotation.transform.forward * 100);
    }
}
