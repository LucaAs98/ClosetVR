using TMPro;
using UnityEngine;

public class ManageSpecificCloth : MonoBehaviour
{
    [SerializeField] private GameObject generalClothMenu; //Menu for the visualization of all the clothes

    //Specific cloth menu
    [SerializeField] private GameObject specificClothMenu; //Menu for the visualization of the specific cloth

    //Title of the specific cloth we want to see before recommend
    [SerializeField] private TextMeshProUGUI titleInTitleBar;

    //Outfit GameObj 
    [SerializeField] private GameObject outfitRoot;

    //Camera for the specific cloth visualization
    private Transform specificClothCamera;

    void Start()
    {
        specificClothCamera = specificClothMenu.GetComponentInChildren<Camera>().transform;
    }

    //Allows the cloth to be viewed in more detail
    public void VisualizeSpecificCloth(string clothName, string clothCategory)
    {
        bool areShoes = clothCategory == "Shoes";
        //Based on the category of the cloth moves the Camera up and down
        MoveSpecificClothCamera(clothCategory);

        //Deactivate the drag component of the outfit and activate the menu for the specific cloth
        outfitRoot.GetComponentInChildren<DragToMoveSpecificCloth>().enabled = false;
        specificClothMenu.SetActive(true);

        //Active only the cloth corresponding to clothName but deactivate all other clothes, also from other categories
        specificClothMenu.GetComponent<Outfit>().ActivateOnlyOneChild(clothName);
        specificClothMenu.GetComponent<Outfit>().MoveLegs(areShoes);

        titleInTitleBar.text = clothName; //Set the title of the menu with the cloth name
    }

    //Based on the category of the cloth moves the Camera up and down
    private void MoveSpecificClothCamera(string clothCategory)
    {
        switch (clothCategory)
        {
            case "UpperBody":
                specificClothCamera.localPosition = new Vector3(2274, 320, -210);
                break;
            case "LowerBody":
                specificClothCamera.localPosition = new Vector3(2274, 157, -280);
                break;
            case "Shoes":
                specificClothCamera.localPosition = new Vector3(2274, 29, -142);
                break;
        }
    }

    //Allows you to put cloth on the outfit preview
    public void PutSpecificClothInOutfit(string clothName, string clothCategory)
    {
        //Active only the cloth corresponding to clothName in his category, we dont mind about other categories
        outfitRoot.GetComponent<Outfit>().ActivateChildOfCategory(clothName, clothCategory);
    }
}