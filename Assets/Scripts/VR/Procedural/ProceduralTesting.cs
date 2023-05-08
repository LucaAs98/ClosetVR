using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTesting : MonoBehaviour
{

    //Other foot
    [SerializeField] ProceduralTesting otherFoot;
    public bool isMoving;
    private bool isActiveFoot;
    public bool startFirst;


    //Parameters of foot position 
    private Vector3 currentPosition;
    private Vector3 standingPosition;

    private Vector3 newFootPosition;
    private Vector3 newFootForward;



    //Parameters of foot rotation
    private Quaternion currentRotation;
    private float headYRotation;
    private float initialYRotation;
    [SerializeField] Transform forwardRotation;
    [SerializeField] GameObject forwardRotationGameObject;


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
    private float timerForResetting;
    private bool stepHappens = false;


    private void Awake()
    {
        isActiveFoot = false;
        isMoving = false;

        currentPosition = gameObject.transform.position;
        currentRotation = gameObject.transform.rotation;

        standingPosition = gameObject.transform.localPosition;

        initialYRotation = gameObject.transform.rotation.eulerAngles.y;

    }

    private void Update()
    {
        timerForResetting += Time.deltaTime;

        if (!otherFoot.IsMoving())
        {
            //Vector3.Lerp(gameObject.transform.position, currentPosition, Time.deltaTime);
            gameObject.transform.position = currentPosition;
            gameObject.transform.rotation = currentRotation;


            SetFootOnGround();

            FeetMovement();

            ReturnToDefaultPosition();
        }
    }


    private void SetFootOnGround()
    {
        //--------------------------------------------------------------------------
        //Fix feet on the ground
        RaycastHit hit;

        bool hasHit = Physics.Raycast(gameObject.transform.position + Vector3.up, Vector3.down, out hit);
        if (hasHit)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, hit.point.y + YOffsetForFoot, gameObject.transform.position.z);
        }
        //--------------------------------------------------------------------------

    }


    private void FeetMovement()
    {
        //--------------------------------------------------------------------------
        //Move feet
        Vector3 tempPosition = headTransform.position + (headTransform.right * footSpacing);
        newFootPosition = new Vector3(tempPosition.x, currentPosition.y, tempPosition.z);


        //Rotate feet
        headYRotation = forwardRotation.rotation.eulerAngles.y;
        currentRotation = Quaternion.Euler(currentRotation.eulerAngles.x, headYRotation + initialYRotation, currentRotation.eulerAngles.z);


        //Set new position of foot
        if (Vector3.Distance(newFootPosition, currentPosition) > distanceForStep)
        {
            isMoving = true;
            Vector3 directionOfStep = (newFootPosition - currentPosition);
            currentPosition = currentPosition + (directionOfStep * stepLenght);
            forwardRotationGameObject.GetComponent<ChangeBodyRotation>().timeForResetting = 0;
            stepHappens = true;
        }


        isMoving = false;
    }

    private void ReturnToDefaultPosition()
    {
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
        if(gameObject.transform.localPosition == standingPosition)
        {
            timerForResetting = 0f;
            forwardRotationGameObject.GetComponent<ChangeBodyRotation>().timeForResetting = 0;
        }
    }


    public bool IsMoving()
    {
        return isMoving;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(newFootForward, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(gameObject.transform.position, transform.forward * 100);

        Gizmos.DrawLine(headTransform.position, headTransform.transform.forward * 100);
        Gizmos.DrawLine(forwardRotation.transform.position, forwardRotation.transform.forward * 100);
    }
}
