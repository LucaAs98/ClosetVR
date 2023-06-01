using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outfit : MonoBehaviour
{
    [SerializeField] private GameObject tshirtAttachPoint;
    [SerializeField] private GameObject trousersAttachPoint;
    [SerializeField] private GameObject hatAttachPoint;
    [SerializeField] private GameObject glassesAttachPoint;
    [SerializeField] private GameObject watchAttachPoint;
    [SerializeField] private GameObject shoesAttachPoint;


    /******** GET ********/
    public GameObject GetTshirtAttachPoint()
    {
        return tshirtAttachPoint;
    }

    public GameObject GetTrousersAttachPoint()
    {
        return trousersAttachPoint;
    }

    public GameObject GetHatAttachPoint()
    {
        return hatAttachPoint;
    }

    public GameObject GetGlassesAttachPoint()
    {
        return glassesAttachPoint;
    }

    public GameObject GetWatchAttachPoint()
    {
        return watchAttachPoint;
    }

    public GameObject GetShoesAttachPoint()
    {
        return shoesAttachPoint;
    }
}