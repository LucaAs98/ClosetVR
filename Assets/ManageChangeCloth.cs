using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ManageChangeCloth : NetworkBehaviour
{
    // [SerializeField] private CapsuleCollider[] collidersList;
    //
    // public  CapsuleCollider[] GetColliders()
    // {
    //     return collidersList;
    // }


    public void ChangeCloth(string clothName)
    {
        string[] splitArray = clothName.Split(char.Parse("_"));
        string type = splitArray[0];
        string material = splitArray[1];
        Debug.Log("Try cloth in server: " + clothName + "\nCloth type: " + type + "\nCloth material: " + material);
    }
}