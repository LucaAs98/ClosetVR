using Unity.Netcode;
using UnityEngine;

public class RecommendAndTryCloth : NetworkBehaviour
{
    [SerializeField] private Transform parentOfSpecificCloth; //Parent of the obj we want to take the child name

    private GameObject closet;
    private GameObject avatar;

    void Start()
    {
        closet = GameObject.FindGameObjectWithTag("Closet");
        avatar = GameObject.FindGameObjectWithTag("Avatar");
    }

    //We take the name of the cloth and we call the serverRpc for activate the hint at the specific cloth
    public void RecommendCloth()
    {
        string clothName = GetClothName();
        RecommendClothServerRpc(clothName);
    }


    //Done in the server. We activate the hint of the corresponding cloth
    [ServerRpc]
    public void RecommendClothServerRpc(string clothName)
    {
        Debug.Log("Recommend " + clothName);
        closet.GetComponent<ManageCloset>().ActiveHangerHint(clothName);
    }

    //We take the name of the cloth and we call the serverRpc for changing the cloth in the avatar
    public void TryCloth()
    {
        string clothName = GetClothName();
        TryClothServerRpc(clothName);
    }

    //Done in the server. We put the cloth to the avatar
    [ServerRpc]
    public void TryClothServerRpc(string clothName)
    {
        Debug.Log("Try cloth: " + clothName);
        avatar.GetComponent<ManageChangeCloth>().ChangeCloth(clothName);
    }


    //Simply return the specific cloth name
    private string GetClothName()
    {
        return parentOfSpecificCloth.GetChild(0).name;
    }
}