using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outfit : MonoBehaviour
{
    [SerializeField] private GameObject tshirtAttachPoint;
    [SerializeField] private GameObject trousersAttachPoint;


    /******** GET ********/
    public GameObject GetTshirtAttachPoint()
    {
        return tshirtAttachPoint;
    }

    public GameObject GetTrousersAttachPoint()
    {
        return trousersAttachPoint;
    }
}