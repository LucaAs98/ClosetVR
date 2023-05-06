using Unity.Netcode;
using UnityEngine;

public class RecommendCloth : NetworkBehaviour
{
    private GameObject closet;

    void Start()
    {
        closet = GameObject.FindGameObjectWithTag("Closet");
    }

    //Done in the server. We activate the hint of the corresponding cloth
    [ServerRpc]
    public void RecommendClothServerRpc(string clothName)
    {
        Debug.Log("Recommend " + clothName);
        closet.GetComponent<ManageCloset>().ActiveHangerHint(clothName);
    }
}