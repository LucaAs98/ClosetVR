using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBody : MonoBehaviour
{

    [SerializeField] Transform bodyPart;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    void Update()
    {
        gameObject.transform.position = bodyPart.position + positionOffset;
        gameObject.transform.rotation = bodyPart.rotation * Quaternion.Euler(rotationOffset);
    }
}
