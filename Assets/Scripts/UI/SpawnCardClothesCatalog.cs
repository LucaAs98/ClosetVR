using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnCardClothesCatalog : MonoBehaviour
{
    [SerializeField] private List<GameObject> categories;
    [SerializeField] private List<Transform> containers;
    [SerializeField] private GameObject cardClothCatalogPrefab;


    private string basePath = "ClothImages";
    private List<string> allClothNames;
    private Dictionary<string, string> clothNamesWithCategory = new();

    // Start is called before the first frame update
    void Start()
    {
        allClothNames = TakeAllClothNames();
        clothNamesWithCategory = SplitClothes(allClothNames);

        List<string> clothesOfCategory = new();

        foreach (Transform category in containers)
        {
            clothesOfCategory = GetClothNamesOfCategory(category.name);

            foreach (var clothName in clothesOfCategory)
            {
                InstantiateCard(category, clothName);
            }
        }
    }

    private void InstantiateCard(Transform category, string clothName)
    {
        Debug.Log($"Creo la carta per {category.name}_{clothName}");

        GameObject newCardClothCatalog = Instantiate(cardClothCatalogPrefab, category);
        Texture2D auxImage = RetriveImageForCloth(category.name, $"{category.name}_{clothName}");
        newCardClothCatalog.GetComponent<CardClothCatalog>().SetClothImage(auxImage);
        newCardClothCatalog.GetComponent<CardClothCatalog>().SetClothName(clothName);
    }

    private Texture2D RetriveImageForCloth(string category, string clothName)
    {
        Debug.Log($"Ottengo le immagini per la categoria {category}");

        //Path to take the corresponding leader 
        string completePath = $"{basePath}/{category}/{clothName}";

        //Load of img in the card
        return Resources.Load<Texture2D>(completePath);
    }


    private List<string> TakeAllClothNames()
    {
        ManageCloset manageCloset = GameObject.FindGameObjectWithTag("Closet").GetComponent<ManageCloset>();
        return manageCloset.GetAllClothNames();
    }

    private Dictionary<string, string> SplitClothes(List<string> allClothes)
    {
        string auxCategory;
        string auxName;
        Dictionary<string, string> clothNamesDivided = new();

        foreach (string clothName in allClothes)
        {
            auxCategory = clothName.Split("_")[0];
            auxName = clothName.Replace(auxCategory + "_", "");
            clothNamesDivided.Add(auxName, auxCategory);
        }

        return clothNamesDivided;
    }

    private List<string> GetClothNamesOfCategory(string category)
    {
        List<string> clothesOfCategory = new();
        foreach (string clothName in clothNamesWithCategory.Keys)
        {
            if (clothNamesWithCategory[clothName] == category)
            {
                clothesOfCategory.Add(clothName);
            }
        }

        return clothesOfCategory;
    }

    public void ChangeCategory()
    {
        string selectedCategory = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        foreach (Transform category in containers)
        {
            category.gameObject.SetActive(category.name == selectedCategory);
        }
    }
}