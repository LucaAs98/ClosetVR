using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ProceduralWalking : MonoBehaviour
{

    [SerializeField] Transform body;
    [SerializeField] ProceduralWalking otherFoot;

    private Vector3 currentPosition;
    private Quaternion currentRotation;
    private Vector3 newPosition;
    private Vector3 currentNormal;
    private Vector3 newNormal;
    private Vector3 oldNormal;
    private Quaternion originalRotation;
    private Vector3 oldPosition;

    private float lerp;

    public float stepDistance;
    public float stepLenght;
    public float footSpacing;
    public float stepHeight;
    private Vector3 footOffset;
    public LayerMask terrainLayer;
    public float speed;


    private float timeSinceLastMove;


    Vector3 originalPosition;
    private void Start()
    {
        //originalPosition = transform.localPosition;
        //timeSinceLastMove = 0f;
        //originalRotation = transform.localRotation;
        oldPosition = newPosition = currentPosition = transform.position;
        oldNormal = currentNormal = newNormal = transform.up;
        lerp = 1;
    }

    void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;
        transform.localRotation = currentRotation;

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newPosition, hit.point) > stepDistance && !otherFoot.isMoving() && lerp >= 1)
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                newPosition = hit.point + (body.forward * stepLenght * direction) + footOffset;
                newNormal = hit.normal;
            }
        }
        if (lerp < 1)
        {
            Vector3 tempPos = Vector3.Lerp(oldPosition, newPosition, lerp);
            if (tempPos.y != 0)
            {
                Quaternion tempRot = Quaternion.Lerp(transform.localRotation, originalRotation, lerp);
                currentRotation = tempRot;
            }
            tempPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            currentPosition = tempPos;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }

        /*
        if (isMoving())
        {
            timeSinceLastMove = 0f;
        }
        else
        {
            timeSinceLastMove += Time.deltaTime;
        }

        if (timeSinceLastMove > 4f)
        {
            Debug.Log("Ciao");
            transform.localPosition = new Vector3(originalPosition.x, transform.localPosition.y, originalPosition.z);
            currentPosition = new Vector3(transform.TransformPoint(transform.localPosition).x, currentPosition.y, transform.TransformPoint(transform.localPosition).z);
            timeSinceLastMove = 0f;
        }*/

    }


    private bool isMoving()
    {
        return lerp < 1;
    }


}
