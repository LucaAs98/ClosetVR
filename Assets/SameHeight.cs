using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameHeight : MonoBehaviour
{

    [SerializeField] GameObject Plane;


    void Update()
    {
        if(Plane != null)
        {
            gameObject.transform.position = Plane.transform.position;
        }
    }
}
