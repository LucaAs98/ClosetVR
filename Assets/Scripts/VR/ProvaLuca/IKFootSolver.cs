using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.Interaction.Toolkit.AR;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer; //Layer of the ground
    [SerializeField] private Transform body; //Center of our body, useful to see how much we have traveled
    [SerializeField] private IKFootSolver otherFoot; //Opposite foot, for check if it is in air or not

    [SerializeField] private float speed = 5;
    [SerializeField] private float stepDistance = .3f;
    [SerializeField] private float stepLength = .3f;
    [SerializeField] private float stepHeight = .3f;

    [SerializeField] private Vector3 footPosOffset, footRotOffset; //Useful for the alignment of the foot

    private float footSpacing, lerpStep;
    private Vector3 oldPos, currentPos, newPos;
    private Vector3 oldNorm, currentNorm, newNorm;

    private bool isFirstStep = true;

    void Start()
    {
        //Initialization
        footSpacing = transform.localPosition.x;
        currentPos = newPos = oldPos = transform.position;
        currentNorm = newNorm = oldNorm = transform.up;
        lerpStep = 1;
    }

    void Update()
    {
        //We set the foot position
        transform.position = currentPos + footPosOffset;
        transform.localRotation = Quaternion.LookRotation(currentNorm) * Quaternion.Euler(footRotOffset);


        /* Ray from the foot down to the ground. So we take the body position  + the foot spacing
         * (body.right but if it multiplied with a negative number will be negative as well).
         * We also want to go up a little bit just in case the center of the body is in the floor for some reason. */
        Ray ray = new Ray(body.position + (body.right * footSpacing) + (Vector3.up * 2), Vector3.down);

        //If the ray hit something we want to do this if
        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
        {
            /* If we arrive at a certain distance (step distance) and the other foot is not moving and this foot is not
             * already moving we can start a new step. */
            if (isFirstStep || (Vector3.Distance(newPos, hit.point) > stepDistance && !otherFoot.IsMoving() &&
                                !IsMoving()))
            {
                StartAStep(hit);
            }
        }

        //When lerpStep is less then 1 we have not finished the step yet
        if (lerpStep < 1)
        {
            ContinueStep();
        }
        else
        {
            FinishStep();
        }
    }

    //We calculate te final position of our foot after all the step
    private void StartAStep(RaycastHit hit)
    {
        isFirstStep = false;
        lerpStep = 0; //We start a new lerpStep

        //The character can move forward or backward
        int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPos).z ? 1 : -1;

        //Final new position of the foot
        newPos = hit.point + (body.forward * (direction * stepLength));
        newNorm = hit.normal;
    }

    //If the lerpStep is not finished means that we still want to move our foot
    private void ContinueStep()
    {
        //We are moving the foot from the old position to the new one
        Vector3 tempPos = Vector3.Lerp(oldPos, newPos, lerpStep);

        //Move the foot upwards in an arc in a sin curve and not straight to the final position  
        tempPos.y += Mathf.Sin(lerpStep * Mathf.PI) * stepHeight;

        currentPos = tempPos; //Useful for updating the position of the foot at the start of the update 
        currentNorm = Vector3.Lerp(oldNorm, newNorm, lerpStep);

        //Increase the lerpStep according to the speed of the step 
        lerpStep += Time.deltaTime * speed;
    }

    //If the step is finished we update the old position to the new one
    private void FinishStep()
    {
        oldPos = newPos;
        oldNorm = newNorm;
    }

    //If the lerpStep is less then 1, it means that the foot is still moving
    public bool IsMoving()
    {
        return lerpStep < 1;
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(newPos, 0.1f);

        // Gizmos.color = Color.red;
        // Gizmos.DrawLine(body.position, body.forward);
    }
}