using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitHandPosition : MonoBehaviour
{
    [SerializeField] private Transform leftHandPrefab;
    [SerializeField] private Transform rightHandPrefab;

    private Transform leftHand;
    private Transform rightHand;


    void Start()
    {
        leftHand = Instantiate(leftHandPrefab, this.transform);
        rightHand = Instantiate(rightHandPrefab, this.transform);
    }

    public Vector3 GetLeftHandPosition()
    {
        return leftHand.position;
    }

    public Vector3 GetRightHandPosition()
    {
        return rightHand.position;
    }
}