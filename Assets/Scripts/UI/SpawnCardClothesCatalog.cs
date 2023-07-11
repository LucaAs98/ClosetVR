using System.Collections.Generic;
using UnityEngine;

public class SpawnCardClothesCatalog : MonoBehaviour
{
    [SerializeField] private List<GameObject> categories; //The categories objects
    [SerializeField] private List<Transform> containers; //All the caontainers for every category
    [SerializeField] private GameObject cardClothCatalogPrefab; //Catalog card prefab

    //Base path for loading the cloth images
    private string basePath = "ClothImages";

    //Dictionary where the key is the cloth name and the value is the cloth category
    private Dictionary<string, string> clothNamesWithCategory = new();

    private ClothesWithSkeletonManager armatureManager;

    void Start()
    {
        //Take the avatar
        GameObject avatar = GameObject.FindGameObjectWithTag("Avatar");
        armatureManager = avatar.GetComponent<ClothesWithSkeletonManager>();
        InitCategoryContainers(); //Instantiate all the clothes of the closet in the "catalog menu"
    }

    //Instantiate all the clothes of the closet in the "catalog menu"
    private void InitCategoryContainers()
    {
        clothNamesWithCategory = armatureManager.GetAllClothNamesWithCategory();

        //Fills the containers of the various categories
        FillCategoryContainers();
    }


    //Save the clothes in the dictionary
    private Dictionary<string, string> SplitClothes(List<string> allClothes)
    {
        string auxCategory;
        string auxName;
        Dictionary<string, string> clothNamesDivided = new();

        //For every cloth in the closet, split the category and cloth name from the complete cloth name and add it to the dictionary
        foreach (string clothName in allClothes)
        {
            auxCategory = clothName.Split("_")[0];
            auxName = clothName.Replace(auxCategory + "_", "");
            clothNamesDivided.Add(auxName, auxCategory);
        }

        return clothNamesDivided;
    }

    //Fills the containers of the various categories
    private void FillCategoryContainers()
    {
        List<string> clothesOfCategory = new(); //List of clothes in a specific category

        //For every category instatiate the cloth cards of the specific category
        foreach (Transform category in containers)
        {
            clothesOfCategory = GetClothNamesOfCategory(category.name); //Get the clothes of a specific category

            //For every cloth of that category instantiate the card
            foreach (var clothName in clothesOfCategory)
            {
                //Instantiate the card in the specific category container
                InstantiateCard(category, clothName);
            }
        }
    }

    //Get the clothes of a specific category
    private List<string> GetClothNamesOfCategory(string category)
    {
        List<string> clothesOfCategory = new(); //List of clothes in a specific category

        //For every cloth in the dictionary, if the category is the same of the "category" parameter, the cloth will be added at the list
        foreach (string clothName in clothNamesWithCategory.Keys)
        {
            if (clothNamesWithCategory[clothName] == category)
            {
                clothesOfCategory.Add(clothName);
            }
        }

        return clothesOfCategory;
    }

    //Instantiate the card in the specific category container
    private void InstantiateCard(Transform category, string clothName)
    {
        //Instantiate the card prefab in the specific container
        GameObject newCardClothCatalog = Instantiate(cardClothCatalogPrefab, category);

        //Take the image of the specific cloth and put it in the card together with the name of the cloth
        Texture2D auxImage = RetriveImageForCloth(category.name, $"{clothName}");
        newCardClothCatalog.GetComponent<CardClothCatalog>().SetClothImage(auxImage);
        newCardClothCatalog.GetComponent<CardClothCatalog>().SetClothName(clothName);
        newCardClothCatalog.GetComponent<CardClothCatalog>().SetClothCategory(category.name);
    }

    //Take the image of the specific cloth
    private Texture2D RetriveImageForCloth(string category, string clothName)
    {
        //Path to take the corresponding image 
        string completePath = $"{basePath}/{category}/{clothName}";

        //Load of img in the card
        return Resources.Load<Texture2D>(completePath);
    }

    //Called when a category card is clicked
    public void ChangeCategory()
    {
        //Take the name of the category button pressed
        string selectedCategory = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        //Activate only that category container
        foreach (Transform category in containers)
        {
            category.gameObject.SetActive(category.name == selectedCategory);
        }
    }

    public float GetPriceFromNameAndCategory(string name, string category)
    {
        foreach (Transform container in containers)
        {
            if (container.name == category)
            {
                foreach (Transform card in container)
                {
                    string cardName = card.GetComponent<CardClothCatalog>().GetClothName().text;
                    if (cardName == name)
                    {
                        return card.GetComponent<CardClothCatalog>().GetPriceInFloat();
                    }
                }
            }
        }

        return 0f;
    }
}