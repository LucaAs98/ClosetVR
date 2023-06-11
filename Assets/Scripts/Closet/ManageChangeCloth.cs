using Unity.Netcode;
using UnityEngine;

public class ManageChangeCloth : NetworkBehaviour
{
    [SerializeField] private Transform clothes; //Avatar's clothes

    //Called in server, does the same thing for the server and the client
    public void ChangeCloth(string clothNames)
    {
        ChangeClothBase(clothNames);
        ChangeClothClientRpc(clothNames);
    }

    //Change cloth in client
    [ClientRpc]
    private void ChangeClothClientRpc(string clothNames)
    {
        ChangeClothBase(clothNames);
    }

    //Change cloth at the avatar
    private void ChangeClothBase(string clothNames)
    {
        //REMINDER! clothNames is the complete string with all the active clothes
        //Cycles avatar clothes and activates only those that the client has selected
        foreach (Transform categoryCloth in clothes)
        {
            foreach (Transform cloth in categoryCloth)
            {
                cloth.gameObject.SetActive(clothNames.Contains(cloth.name));
            }
        }
    } 
}