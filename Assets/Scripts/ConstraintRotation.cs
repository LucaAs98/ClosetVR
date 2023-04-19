using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintRotation : MonoBehaviour
{

    public Vector3 offset;
    public Vector3 positionOffset;

    private void Update()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(offset);
        transform.position = transform.position + positionOffset;
    }
}
