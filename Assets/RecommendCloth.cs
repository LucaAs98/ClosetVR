using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecommendCloth : MonoBehaviour
{
    private GameObject tShirtContainer;

    void Start()
    {
        tShirtContainer = GameObject.Find("TShirtsContainer");
    }

    //Eseguita sul server
    public void Recommend(string clothName)
    {
        Debug.Log("Recommend " + clothName);
        tShirtContainer.GetComponent<ManageCloset>().ActiveCrouchHint(clothName);
    }
}