using UnityEngine;
using UnityEngine.UIElements;

public class ManageCardCloth : MonoBehaviour
{
    [SerializeField] private Transform tridimensionalModelParent;
    private ManageSpecificCloth manageSpecificClothComponent;
    private Transform specificCardCloth;

    void Start()
    {
        manageSpecificClothComponent = this.transform.root.GetComponent<ManageSpecificCloth>();
    }


    public void QuickLook()
    {
        specificCardCloth = tridimensionalModelParent.GetChild(0);

        Debug.Log("------------- Quick Look ------------- " + specificCardCloth.name);

        manageSpecificClothComponent.VisualizeSpecificCloth(specificCardCloth);
    }

    public void PutInOutfit()
    {
        specificCardCloth = tridimensionalModelParent.GetChild(0);

        Debug.Log("------------- PutInOutfit ------------- " + specificCardCloth.name);

        manageSpecificClothComponent.PutSpecificClothInOutfit(specificCardCloth);
    }
}