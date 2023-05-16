using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private Transform body;
    [SerializeField] private IKFootSolver otherFoot;
    [SerializeField] private float speed = 5, stepDistance = .3f, stepLength = .3f, stepHeight = .3f;
    [SerializeField] private Vector3 footPosOffset, footRotOffset;

    private float footSpacing, lerp;
    private Vector3 oldPos, currentPos, newPos;
    private Vector3 oldNorm, currentNorm, newNorm;
    private bool isFirstStep = true;

    void Start()
    {
        footSpacing = transform.localPosition.x;
        currentPos = newPos = oldPos = transform.position;
        currentNorm = newNorm = oldNorm = transform.up;
        lerp = 1;
    }

    void Update()
    {
        transform.position = currentPos + footPosOffset;
        transform.localRotation = Quaternion.LookRotation(currentNorm) * Quaternion.Euler(footRotOffset);

        Ray ray = new Ray(body.position + (body.right * footSpacing) + (Vector3.up * 2), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value) || isFirstStep)
        {
            if (isFirstStep || (Vector3.Distance(newPos, hit.point) > stepDistance && !otherFoot.IsMoving() &&
                                !IsMoving()))
            {
                isFirstStep = false;
                lerp = 0;
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPos).z ? 1 : -1;
                newPos = hit.point + (body.forward * (direction * stepLength));
                newNorm = hit.normal;
            }
        }

        if (lerp < 1)
        {
            Vector3 tempPos = Vector3.Lerp(oldPos, newPos, lerp);
            tempPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPos = tempPos;
            currentNorm = Vector3.Lerp(oldNorm, newNorm, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPos = newPos;
            oldNorm = newNorm;
        }
    }


    public bool IsMoving()
    {
        return lerp < 1;
    }
}