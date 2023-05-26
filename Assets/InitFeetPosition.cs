using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitFeetPosition : MonoBehaviour
{
    [SerializeField] private Transform leftFootPrefab;
    [SerializeField] private Transform rightFootPrefab;

    private Transform leftFoot;
    private Transform rightFoot;


    void Start()
    {
        leftFoot = Instantiate(leftFootPrefab, this.transform);
        rightFoot = Instantiate(rightFootPrefab, this.transform);
    }

    public Vector3 GetLeftFootPosition()
    {
        return leftFoot.position;
    }

    public Vector3 GetRightFootPosition()
    {
        return rightFoot.position;
    }
}