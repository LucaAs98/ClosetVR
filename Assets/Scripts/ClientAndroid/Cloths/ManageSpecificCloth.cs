using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ManageSpecificCloth : MonoBehaviour
{
    [SerializeField] private GameObject generalClothMenu; //Menu for the visualization of all the clothes (like Zalando)

    //Specific cloth menu
    [SerializeField] private GameObject specificClothMenu; //Menu for the visualization of the specific cloth
    [SerializeField] private Transform parentOfSpecificCloth; //Parent of the obj we want to rotate when dragging

    //Title of the specific cloth we want to see before recommend
    [SerializeField] private TextMeshProUGUI titleInTitleBar;

    //Component where we have a lot of utilities for the management of the specific cloth
    //private ManageSpecificCloth manageSpecificClothComponent;

    void Start()
    {
        //We need to activate and deactivate the menu for taking the component
        // specificClothMenu.SetActive(true);
        // manageSpecificClothComponent = specificClothMenu.GetComponent<ManageSpecificCloth>();
        // specificClothMenu.SetActive(false);
    }


    //When we go back we need to destroy the specific cloth we have instantiate for the next one
    public void DestroyObjGoingBack()
    {
        GameObject specificCloth = parentOfSpecificCloth.GetChild(0).gameObject;
        Destroy(specificCloth);
    }

    //Simple return of the container
    public Transform GetContainer3dRepresentation()
    {
        return parentOfSpecificCloth;
    }

    public void VisualizeSpecificCloth(Transform cloth)
    {
        //We Deactivate the general menu and activate the specific one
        //generalClothMenu.SetActive(false);
        specificClothMenu.SetActive(true);

        //We instantiate of the specific cloth in the new specific menu (3DParent)
        Transform clothInstance = Instantiate(cloth, parentOfSpecificCloth);

        /* We don't want to replicate "(Clone)" so we rename it like "ClothName (Clone)" otherwise will be "ClothName (Clone) (Clone)"
         * Then we set the name in the title bar. */
        clothInstance.name = cloth.name;
        SetTitleFromClothName(clothInstance.name);
    }

    //Useful to set the title of the page
    public void SetTitleFromClothName(string clothName)
    {
        //Variable we need for removing the second label "(Clone)" that is automatically added when we "Instantiate" an obj
        string stringToRemove = "(Clone)";
        int stringToRemoveLenght = stringToRemove.Length;

        //We set the title with the specific cloth name removing the text "(Clone)"
        titleInTitleBar.text = clothName.Substring(0, clothName.Length - stringToRemoveLenght);
    }
}