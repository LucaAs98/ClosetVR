using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    [SerializeField] Transform directionPoint;
    private void Awake()
    {
        Vector3 direction = gameObject.transform.position - directionPoint.position;
        gameObject.transform.position = directionPoint.position + direction;
        
    }
}
