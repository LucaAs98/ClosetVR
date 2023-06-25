using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageRecommendCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recommendByName; //Title of the card: "Recommended by {userName}"

    //Prefab to spawn in shopping cart when the vr player want to buy the outfit elements
    [SerializeField] private GameObject cartElement;

    [SerializeField] private Camera cardCamera; //Camera of the render texture.
    [SerializeField] private RawImage clothRepresentation2D; //RawImage where the render texture is put

    [SerializeField] private Gradient gradient;
    [SerializeField] private TextMeshProUGUI percentage;
    [SerializeField] private Slider recommendBarSlider;
    [SerializeField] private Image barPercentage;

    private Transform cartClothes; //Container of cart clothes

    private string basePath = "ClothImages";
    private string[] outfitClothesArray; //Array of the outfit's clothing names
    private string outfitClothesInString;

    private ManageMirrorCards manageMirrorCards;
    private ManageRecommendedMenu manageRecommendedMenu;
    private int numOfRecommend = 0;
    private string recommendedBy = "";

    void Start()
    {
        manageMirrorCards = this.transform.root.GetComponent<ManageMirrorCards>();
        manageRecommendedMenu =
            GameObject.FindGameObjectWithTag("RecommendedMenu").GetComponent<ManageRecommendedMenu>();
    }

    //Set the title of the card
    public void SetUserName()
    {
        recommendByName.text = $"Recommend by: {recommendedBy}";
    }

    public void UpdateUserName()
    {
        string person = "person";

        if (numOfRecommend - 1 > 1)
            person = "people";

        recommendByName.text = $"Recommend by: {recommendedBy} and other {numOfRecommend - 1} {person}...";
    }


    //Complete the card with all necessary data
    public void ConfigureCard(string clothNames, string name)
    {
        numOfRecommend++;
        recommendedBy = name;
        outfitClothesInString = clothNames;
        //clothNames contains all the cloth names divided by ","
        outfitClothesArray = clothNames.Split(",");
        this.GetComponent<Outfit>().ActivateInRecommendCard(clothNames); //Activate recommended clothes in outfit
        SetCorrectRenderTexture(); //Create and set the render texture
        SetUserName(); //Set the name in "Recommended by: ..."
    }

    public void PutClothInAvatar()
    {
        foreach (string clothName in outfitClothesArray)
        {
            string category = clothName.Split("_")[0]; //Take the category from the name
            string nameWithoutCategory = clothName.Replace(category + "_", ""); //Cloth name without category

            manageMirrorCards.PutCloth(nameWithoutCategory, category);
        }
    }


    //Create and set the render texture
    private void SetCorrectRenderTexture()
    {
        //Create render texture
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 16, RenderTextureFormat.ARGB32);
        renderTexture.name = GetInstanceID() + "RenderTexture";

        //Take the camera and set the render texture
        cardCamera.GetComponent<Camera>().targetTexture = renderTexture;

        //Take the raw image and set the render texture
        clothRepresentation2D.texture = renderTexture;
    }

    //Add the outfit's clothes in cart separately 
    public void AddToCart()
    {
        Transform shoppingCartMenu = GameObject.FindGameObjectWithTag("ShoppingCartMenu").transform;
        //Container of cart clothes
        cartClothes = shoppingCartMenu.GetComponent<ManageShoppingCartMenu>().GetCartClothes().GetChild(0);

        //For every cloth in the outfit add the card in the shopping cart.
        foreach (string clothName in outfitClothesArray)
        {
            string category = clothName.Split("_")[0]; //Take the category from the name
            string nameWithoutCategory = clothName.Replace(category + "_", ""); //Cloth name without category

            //Path for taking the corresponding image
            string completePath = $"{basePath}/{category}/{clothName}";

            //Takes the image related to the name of the cloth (from the resources folder) to put it in the cart card
            StartCoroutine(FindFileAndCompleteCard(completePath, nameWithoutCategory, category));
        }
    }

    //Takes the image related to the name of the cloth (from the resources folder) to put it in the cart card
    IEnumerator FindFileAndCompleteCard(string completePath, string clothName, string category)
    {
        //Save the texture related to the cloth in auxTexture
        Texture2D auxTexture = new Texture2D(1920, 1080);
        auxTexture = Resources.Load<Texture2D>(completePath);

        //Instantiate the cart card in the shopping cart
        GameObject newCartElement = Instantiate(cartElement, cartClothes);
        //Complete that card putting the image and the cloth name
        newCartElement.GetComponent<CartElement>().CompleteCartCard(clothName, category, auxTexture);

        yield return null;
    }

    //Destroy this card
    public void Remove()
    {
        Destroy(this.gameObject);
        manageRecommendedMenu.RemoveOutfit(outfitClothesInString);
    }

    public void UpdateRecommendCard()
    {
        numOfRecommend++;
        UpdateUserName();
    }

    public string GetOutfitClothesInString()
    {
        return outfitClothesInString;
    }

    public void ShowInfoSpecificOutfit()
    {
        manageRecommendedMenu.ShowOutfitRecommendedNames(outfitClothesInString);
    }

    public Color ColorFromGradient(float value)
    {
        return gradient.Evaluate(value);
    }

    public void ChangePercentage(float value)
    {
        recommendBarSlider.value = value;
        percentage.text = $"{(value * 100):F2} %";
        barPercentage.color = ColorFromGradient(value);
    }
}