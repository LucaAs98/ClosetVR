using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ManageChangeCloth : NetworkBehaviour
{
    [SerializeField] private Transform clothes; //Avatar's clothes

    //Called in server, does the same thing for the server and the client
    public void ChangeCloth(string clothNames, string category = null)
    {
        ChangeClothBase(clothNames, category);
        ChangeClothClientRpc(clothNames, category);
    }

    //Change cloth in client
    [ClientRpc]
    private void ChangeClothClientRpc(string clothNames, string category)
    {
        ChangeClothBase(clothNames, category);
    }

    //Change cloth at the avatar
    private void ChangeClothBase(string clothNames, string category)
    {
        List<Transform> listOfClothActivated = new(); //List of activated clothes 

        /*REMINDER! clothNames is the complete string with all the active clothes.    
        Cycles avatar clothes and activates only those that the client has selected*/
        foreach (Transform categoryCloth in clothes)
        {
            if (category == null || category == categoryCloth.name)
            {
                foreach (Transform cloth in categoryCloth)
                {
                    if (!clothNames.Contains(cloth.name.Replace("_root", "")))
                    {
                        if (cloth.gameObject.activeSelf)
                        {
                            //First of all deactivate all the skin parts related to the clothes being deactivated.
                            cloth.GetComponent<ManageCloth>().ActivateSkinParts();

                            cloth.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        listOfClothActivated.Add(cloth);
                        cloth.gameObject.SetActive(true);
                    }
                }
            }
        }

        //At the end, deactivate the skin parts related to the clothes we are activating.
        foreach (var cloth in listOfClothActivated)
        {
            cloth.GetComponent<ManageCloth>().DeactivateSkinParts();
        }
    }
}