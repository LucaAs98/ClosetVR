using System.Collections.Generic;
using UnityEngine;

public class ManageSkinParts : MonoBehaviour
{
    //Disables skin parts when cloth is enabled
    public void DeactivateParts(List<string> partsToDeactivate)
    {
        ActivateDeactivate(partsToDeactivate, false);
    }

    //Enable skin parts again after the cloth has been deactivated
    public void ActivateParts(List<string> partsToActivate)
    {
        ActivateDeactivate(partsToActivate, true);
    }


    //Generic function that activates or deactivates various parts based on a boolean
    private void ActivateDeactivate(List<string> parts, bool activate)
    {
        foreach (Transform skinPart in this.transform)
        {
            if (parts.Contains(skinPart.name))
                skinPart.gameObject.SetActive(activate);
        }
    }
}