using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnClothes : MonoBehaviour
{
    [SerializeField] private GameObject cardBtnCompletePrefab; //Prefab of the card button complete to instantiate
    [SerializeField] private Transform containerUpperBody; //Container of all the t-shirts in the menu
    [SerializeField] private Transform containerLowerBody; //Container of all the trousers in the menu
    [SerializeField] private Transform containerShoes; //Container of all the shoes in the menu
    [SerializeField] private Transform containerCaps; //Container of all the caps in the menu
    [SerializeField] private Transform containerGlasses; //Container of all the glasses in the menu
    [SerializeField] private Transform containerWatches; //Container of all the watches in the menu

    [SerializeField] private Transform armatureWithClothes;
    private ClothesWithSkeletonManager armatureManager;
    private List<Transform> allCategories;

    private GameObject cardTextGameObj; //Text of the cardBtnComplete
    private GameObject cardBtnGameObj; //Card button we are instantiating


    private RawImage imgInCard;
    private string basePath = "ClothImages";

    void Start()
    {
        TakeClothes(); //Get the clothes from the closet
        AddClothes(); //Add the clothes to the menu
    }

    //Get the clothes from the closet
    private void TakeClothes()
    {
        armatureManager = armatureWithClothes.GetComponent<ClothesWithSkeletonManager>();
        allCategories = armatureManager.GetAllCategoryContainers();
    }

    //Add the clothes to the menu
    private void AddClothes()
    {
        Transform container;

        //We create the group of containers and clothes list we want to add at the android menu
        Transform[] allContainers =
        {
            containerUpperBody, containerLowerBody, containerShoes, containerCaps, containerGlasses, containerWatches
        };

        int i = 0; //Used to iterate allLists at the same time of allContainers

        foreach (Transform category in allCategories)
        {
            container = allContainers[i]; //We take the right list of clothes

            //We add every cloth to the menu
            foreach (Transform cloth in armatureManager.GetClothesOfCategory(category.name))
            {
                AddCardBtn(container, cloth.name, category.name);
            }

            i++;
        }
    }

    //Add the card btn in the specific container
    private void AddCardBtn(Transform container, string clothName, string category)
    {
        //Instantiate the cardBtnPrefab
        cardBtnGameObj = Instantiate(cardBtnCompletePrefab, container);
        ManageCardCloth manageCardCloth = cardBtnGameObj.GetComponent<ManageCardCloth>();

        //Set the new name and the category at the specific manageCardCloth
        SetClothNameAndCategory(clothName, category, manageCardCloth);

        //Put the image in the instantiated cardBtn and also the name in the card
        PutImage(clothName, category, manageCardCloth);
        PutTitle(clothName, manageCardCloth);
    }

    //Set the new name and the category at the specific manageCardCloth
    private void SetClothNameAndCategory(string clothName, string category, ManageCardCloth manageCardCloth)
    {
        //Set of the name and category in the specific component manageCardCloth
        manageCardCloth.SetClothName(clothName);
        manageCardCloth.SetClothCategory(category);
    }

    //Put the image in the instantiated cardBtn
    private void PutImage(string clothName, string category, ManageCardCloth manageCardCloth)
    {
        //Raw img component where to put the image of the corresponding cloth
        imgInCard = manageCardCloth.GetClothImgGO().GetComponent<RawImage>();

        //Path to take the corresponding leader 
        string completePath = $"{basePath}/{category}/{clothName}";

        //Load of img in the card
        imgInCard.texture = Resources.Load<Texture2D>(completePath);
    }

    //Put the cloth name in the instantiated cardBtn
    private void PutTitle(string clothName, ManageCardCloth manageCardCloth)
    {
        //Take of the title GO and set of the text with the name without "{category}_"
        cardTextGameObj = manageCardCloth.GetClothNameGO();
        cardTextGameObj.GetComponent<TextMeshProUGUI>().text = clothName;
    }
}