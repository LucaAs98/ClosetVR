using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RecommendCloth : NetworkBehaviour
{
    private GameObject closet;

    void Start()
    {
        closet = GameObject.FindGameObjectWithTag("Closet");
    }

    [ServerRpc]
    public void RecommendClothServerRpc(string clothName)
    {
        Debug.Log("Recommend " + clothName);
        closet.GetComponent<ManageCloset>().ActiveCrouchHint(clothName);
    }
}