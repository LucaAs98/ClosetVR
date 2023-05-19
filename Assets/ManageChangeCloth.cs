using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ManageChangeCloth : NetworkBehaviour
{
    [SerializeField] private CapsuleCollider[] collidersList;
    [SerializeField] private Transform tShirtPoint;


    public void ChangeCloth(string clothName)
    {
        string[] splitArray = clothName.Split(char.Parse("_"));
        string type = splitArray[0];
        string material = splitArray[1];
        Debug.Log("Try cloth in server: " + clothName + "\nCloth type: " + type + "\nCloth material: " + material);
        GameObject cloth = GameObject.Find(clothName);
        PutColliders(cloth);
        Instantiate(cloth, tShirtPoint);
    }

    private void PutColliders(GameObject cloth)
    {
        cloth.GetComponentInChildren<Cloth>().capsuleColliders = collidersList;
    }
}