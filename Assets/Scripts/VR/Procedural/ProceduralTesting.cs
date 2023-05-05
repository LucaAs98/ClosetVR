using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTesting : MonoBehaviour
{


    //Parameters of foot position and rotation
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private Vector3 standingPosition; 
    
    private Quaternion newRotation;
    private Quaternion currentRotation;
    private Quaternion standingRotation;
    private float startingRotationY;

    private Vector3 newFootPosition;
    private Vector3 newFootForward;


    //Parameters for raycasting on ground
    [SerializeField] Transform headTransform;
    [SerializeField] LayerMask terrainLayer;

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
        currentPosition = gameObject.transform.position;
        currentRotation = gameObject.transform.localRotation;

        standingPosition = gameObject.transform.localPosition;
        
        startingRotationY = gameObject.transform.rotation.y;
    }

    private void Update()
    {
        timerForResetting += Time.deltaTime;

        gameObject.transform.position = currentPosition;
        gameObject.transform.localRotation = currentRotation;

        SetFootOnGround();

        FeetMovement();

        ReturnToDefaultPosition();

        Debug.Log(gameObject.transform.rotation);
        Debug.Log("LocalRotation: " + gameObject.transform.localRotation);
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
        newFootPosition = new Vector3(headTransform.transform.position.x, currentPosition.y, headTransform.transform.position.z);

        currentRotation = new Quaternion(currentRotation.x, headTransform.rotation.y, currentRotation.z, currentRotation.w);
       

        //Set new position of foot
        if (Vector3.Distance(newFootPosition, currentPosition) > distanceForStep)
        {
            Vector3 directionOfStep = (newFootPosition - currentPosition);
            currentPosition = currentPosition + (directionOfStep * stepLenght);
            stepHappens = true;
            timerForResetting = 0;
        }


        //Set new rotation of foot

        //--------------------------------------------------------------------------
    }
    private void ReturnToDefaultPosition()
    {
        //--------------------------------------------------------------------------
        //Return to standing position if no movement
        if (timerForResetting > 3f && stepHappens == true)
        {
            Debug.Log("Coming");
            gameObject.transform.localPosition = standingPosition;
            currentPosition = gameObject.transform.position;

            stepHappens = false;
        }
        //--------------------------------------------------------------------------
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(newFootForward, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(gameObject.transform.position, transform.forward*100);
        Gizmos.DrawLine(headTransform.position, headTransform.transform.forward * 100);
    }
}
