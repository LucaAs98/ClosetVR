using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCloth : MonoBehaviour
{
    [SerializeField] private ManageSkinParts manageSkinParts; //Skin parts container with associated script
    [SerializeField] private List<GameObject> deactivateSkinParts; //Parts of skin to activate/deactivate
    private List<string> deactivatePartsInString = new(); //List of names of skin parts for this cloth


    void Awake()
    {
        //Creation of the list of names of skin parts for this cloth
        foreach (var part in deactivateSkinParts)
        {
            deactivatePartsInString.Add(part.name);
        }
    }


    //Disables skin parts when cloth is enabled
    public void DeactivateSkinParts()
    {
        manageSkinParts.DeactivateParts(deactivatePartsInString);
    }

    //Enable skin parts again after the cloth has been deactivated
    public void ActivateSkinParts()
    {
        manageSkinParts.ActivateParts(deactivatePartsInString);
    }
}