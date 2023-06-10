using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ManageCardCloth : MonoBehaviour
{
    [SerializeField] private GameObject clothNameGO; //Title of the card
    [SerializeField] private GameObject clothImgGO; //Img of the card

    private string clothName; //Name of the associated cloth
    private string clothCategory; //Category of the associated cloth

    //Component that handles the outfit preview and the quick look function.
    private ManageSpecificCloth manageSpecificClothComponent;

    void Start()
    {
        manageSpecificClothComponent = this.transform.root.GetComponent<ManageSpecificCloth>();
    }


    //Allows the cloth to be viewed in more detail
    public void QuickLook()
    {
        manageSpecificClothComponent.VisualizeSpecificCloth(clothName, clothCategory);
    }

    //Allows you to put cloth on the outfit preview
    public void PutInOutfit()
    {
        manageSpecificClothComponent.PutSpecificClothInOutfit(clothName, clothCategory);
    }

    //--------------------- GET and SET ---------------------------------------
    public void SetClothName(string name) => clothName = name;
    public void SetClothCategory(string category) => clothCategory = category;

    public GameObject GetClothNameGO()
    {
        return clothNameGO;
    }

    public GameObject GetClothImgGO()
    {
        return clothImgGO;
    }
}