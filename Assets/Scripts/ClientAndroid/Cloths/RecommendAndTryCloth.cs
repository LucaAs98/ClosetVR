using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RecommendAndTryCloth : NetworkBehaviour
{
    [SerializeField] private Outfit outfitComponent; //Outfit component from which active clothes are taken

    private GameObject closet;
    private GameObject avatar;

    private string userName; //Usefull for "Recommended by ..."

    //We take the name of the cloth and we call the serverRpc for activate the hint at the specific cloth
    public void RecommendCloth()
    {
        //Take all cloth activated names then recommend them "at the server".
        //Warning! "clothNames" is a string with ALL the cloth names divided by ","
        string clothNames = GetClothNames();
        userName = this.gameObject.GetComponent<ClientHandler>().GetPlayerName();
        RecommendClothServerRpc(clothNames, userName);
    }

    //Done in the server. Activate the hints of the corresponding clothes
    [ServerRpc]
    public void RecommendClothServerRpc(string clothNames, string userName)
    {
        //Take the closet
        closet = GameObject.FindGameObjectWithTag("Closet");

        //Activate the hints of the corresponding clothes
        closet.GetComponent<ManageCloset>().ActiveHangerHint(clothNames);
        //Add the recommended clothes to the mirror menu
        closet.GetComponent<ManageCloset>().AddToRecommendMenu(clothNames, userName);
    }

    //Take the name of the cloth and call the serverRpc for changing the cloth in the avatar
    public void TryCloth()
    {
        //Take all cloth activated names then change them at the avatar.
        //Warning! "clothNames" is a string with ALL the cloth names divided by ","
        string clothNames = GetClothNames();
        TryClothServerRpc(clothNames);
    }

    //Done in the server. Put the clothes on the avatar
    [ServerRpc]
    public void TryClothServerRpc(string clothNames)
    {
        //Take the avatar
        avatar = GameObject.FindGameObjectWithTag("Avatar");

        //Put the clothes on the avatar
        avatar.transform.parent.GetComponent<ManageChangeCloth>().ChangeCloth(clothNames);
    }

    //Return a string with all the activated clothes divided by ",". Ex: "Upperbody_redTshirt,Lowerbody_bluePants"
    //Only one string is needed because otherwise we can't do the ServerRPC
    private string GetClothNames()
    {
        //String with all the activated clothes divided by ","
        string allClothesNames = "";

        //Get the active clothes from Outfit
        List<string> activeClothes = outfitComponent.GetActiveClothes();

        //Merge all the names in one string
        foreach (var clothName in activeClothes)
        {
            allClothesNames += (clothName.Replace("_root", "") + ",");
        }

        //Remove the last ","
        allClothesNames = allClothesNames.Substring(0, allClothesNames.Length - 1);

        return allClothesNames;
    }
}