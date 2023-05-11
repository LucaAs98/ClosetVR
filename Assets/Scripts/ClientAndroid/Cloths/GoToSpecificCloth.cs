using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToSpecificCloth : MonoBehaviour
{
    private ManageSpecificCloth manageSpecificClothComponent;

    void Start()
    {
        manageSpecificClothComponent = this.transform.root.GetComponent<ManageSpecificCloth>();
    }

    //We call the main function in the "ManageSpecificCloth" of the client passing the cloth
    public void VisualizeSpecificCloth()
    {
        Transform specificCardCloth = this.transform.GetChild(0);
        manageSpecificClothComponent.VisualizeSpecificCloth(specificCardCloth);
    }
}