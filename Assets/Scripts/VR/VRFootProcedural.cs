using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFootProcedural : MonoBehaviour
{

    [SerializeField] GameObject LeftFoot;
    [SerializeField] GameObject RightFoot;
    public Vector3 footOffset;

    [Range(0, 1)]
    public float rightFootPosWeight = 1;
    [Range(0, 1)]
    public float rightFootRotWeight = 1;
    [Range(0, 1)]
    public float leftFootPosWeight = 1;
    [Range(0, 1)]
    public float leftFootRotWeight = 1;

    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 rightFootPos = RightFoot.transform.position;
        RaycastHit hit;

        bool hasHit = Physics.Raycast(rightFootPos + Vector3.up, Vector3.down, out hit);
        if (hasHit)
        {
        }
        else
        {
        }

        Vector3 leftFootPos = LeftFoot.transform.position;
 

        hasHit = Physics.Raycast(leftFootPos + Vector3.up, Vector3.down, out hit);

        if (hasHit)
        {
        }
        else
        {
        }
    }
}
