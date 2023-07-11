using System.Collections.Generic;
using Mono.CSharp;
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
    public void ChangeClothBase(string clothNames, string category)
    {

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
                            cloth.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        cloth.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    //Return a List of strings with all the clothes name activated
    public List<string> GetActiveClothes()
    {
        List<string> activeClothes = new();

        foreach (Transform clothesCategory in clothes)
        {
            foreach (Transform cloth in clothesCategory)
            {
                if (cloth.gameObject.activeSelf)
                    activeClothes.Add($"{clothesCategory.name}_{cloth.name}");
            }
        }

        return activeClothes;
    }
}