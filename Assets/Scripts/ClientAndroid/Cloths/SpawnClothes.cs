using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnClothes : MonoBehaviour
{
    private GameObject[] t_shirtsList; //List of t-shirts to add
    private GameObject[] trousersList; //List of trousers to add
    private GameObject[] shoesList; //List of shoes to add
    private GameObject[] capsList; //List of caps to add
    private GameObject[] glassesList; //List of glasses to add
    private GameObject[] watchesList; //List of watches to add

    [SerializeField] private GameObject cardBtnCompletePrefab; //Prefab of the card button complete to instantiate
    [SerializeField] private Transform containerTShirts; //Container of all the t-shirts in the menu
    [SerializeField] private Transform containerTrousers; //Container of all the trousers in the menu
    [SerializeField] private Transform containerShoes; //Container of all the shoes in the menu
    [SerializeField] private Transform containerCaps; //Container of all the caps in the menu
    [SerializeField] private Transform containerGlasses; //Container of all the glasses in the menu
    [SerializeField] private Transform containerWatches; //Container of all the watches in the menu

    private GameObject cardTextGameObj; //Text of the cardBtnComplete
    private GameObject cardBtnGameObj; //Card button we are instantiating


    private GameObject[] currentListOfClothes; //List of clothes we are iterating


    private RawImage imgInCard;
    private string basePath = "ClothImages";

    private string auxNewClothName = "";
    private string auxClothCategory = "";

    void Start()
    {
        TakeClothes(); //Get the clothes from the closet
        AddClothes(); //Add the clothes to the menu
    }

    //Get the clothes from the closet
    private void TakeClothes()
    {
        ManageCloset manageCloset = GameObject.FindGameObjectWithTag("Closet").GetComponent<ManageCloset>();
        t_shirtsList = manageCloset.GetTShirtsGameObjects();
        trousersList = manageCloset.GetTrousersGameObjects();
        shoesList = manageCloset.GetShoesGameObjects();
        capsList = manageCloset.GetCapsGameObjects();
        glassesList = manageCloset.GetGlassesGameObjects();
        watchesList = manageCloset.GetWatchesGameObjects();
    }

    //Add the clothes to the menu
    private void AddClothes()
    {
        //We create the group of containers and clothes list we want to add at the android menu
        Transform[] allContainers =
            { containerTShirts, containerTrousers, containerShoes, containerCaps, containerGlasses, containerWatches };
        GameObject[][] allLists = { t_shirtsList, trousersList, shoesList, capsList, glassesList, watchesList };

        int i = 0; //Used to iterate allLists at the same time of allContainers

        foreach (var container in allContainers)
        {
            currentListOfClothes = allLists[i]; //We take the right list of clothes

            //We add every cloth to the menu
            foreach (var cloth in currentListOfClothes)
            {
                AddCardBtn(container, cloth);
            }

            i++;
        }
    }

    //Add the card btn in the specific container
    private void AddCardBtn(Transform container, GameObject cloth)
    {
        //Instantiate the cardBtnPrefab
        cardBtnGameObj = Instantiate(cardBtnCompletePrefab, container);
        ManageCardCloth manageCardCloth = cardBtnGameObj.GetComponent<ManageCardCloth>();

        //!! Category have to be at the start of the cloth name separated with an underscore !!
        string clothCategory = "";

        //Set the new name and the category at the specific manageCardCloth
        SetClothNameAndCategory(cloth.name, manageCardCloth);

        //Put the image in the instantiated cardBtn and also the name in the card
        PutImage(cloth.name, manageCardCloth);
        PutTitle(manageCardCloth);
    }

    //Set the new name and the category at the specific manageCardCloth
    private void SetClothNameAndCategory(string originalClothName, ManageCardCloth manageCardCloth)
    {
        //Extrapolation of category from cloth name
        auxClothCategory = originalClothName.Split("_")[0];

        //Remove of the category from the cloth name
        auxNewClothName = originalClothName.Replace(auxClothCategory + "_", "");


        //Set of the name and category in the specific component manageCardCloth
        manageCardCloth.SetClothName(auxNewClothName);
        manageCardCloth.SetClothCategory(auxClothCategory);
    }

    //Put the image in the instantiated cardBtn
    private void PutImage(string clothName, ManageCardCloth manageCardCloth)
    {
        //Raw img component where to put the image of the corresponding cloth
        imgInCard = manageCardCloth.GetClothImgGO().GetComponent<RawImage>();

        //Path to take the corresponding leader 
        string completePath = $"{basePath}/{auxClothCategory}/{clothName}";

        //Load of img in the card
        imgInCard.texture = Resources.Load<Texture2D>(completePath);
    }

    //Put the cloth name in the instantiated cardBtn
    private void PutTitle(ManageCardCloth manageCardCloth)
    {
        //Take of the title GO and set of the text with the name without "{category}_"
        cardTextGameObj = manageCardCloth.GetClothNameGO();
        cardTextGameObj.GetComponent<TextMeshProUGUI>().text = auxNewClothName;
    }
}