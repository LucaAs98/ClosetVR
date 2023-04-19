using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionFoot : MonoBehaviour
{

    [SerializeField] Transform foot;
    public Vector3 offset;

    void Update()
    {
        transform.position = new Vector3(foot.position.x, 0, foot.position.y);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(foot.position, foot.forward * 10f);
    }

}
