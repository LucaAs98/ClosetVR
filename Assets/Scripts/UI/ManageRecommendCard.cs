using System.Collections;
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
    private SpawnCardClothesCatalog manageCardCatalog;
    private int numOfRecommend;
    private string recommendedBy = "";

    //Init some variables at the start
    void Start()
    {
        manageMirrorCards = this.transform.root.GetComponent<ManageMirrorCards>();
        manageRecommendedMenu =
            GameObject.FindGameObjectWithTag("RecommendedMenu").GetComponent<ManageRecommendedMenu>();
        manageCardCatalog = GameObject.FindGameObjectWithTag("CatalogMenu").GetComponent<SpawnCardClothesCatalog>();
    }

    //Set the first title of the card
    public void SetUserName()
    {
        recommendByName.text = $"Recommend by: {recommendedBy}";
    }

    //Update the title of the card when more than 1 person recommend the same outfit
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
        numOfRecommend++; //Increase the number of users who have recommended this outfit
        recommendedBy = name; //First user to recommend this outfit
        outfitClothesInString = clothNames; //Save the clothes of this outfit for next use

        //clothNames contains all the cloth names divided by ","
        outfitClothesArray = clothNames.Split(",");

        this.GetComponent<Outfit>().ActivateInRecommendCard(clothNames); //Activate recommended clothes in outfit
        SetCorrectRenderTexture(); //Create and set the render texture
        SetUserName(); //Set the name in "Recommended by: ..."
    }

    //Put cloth in avatar
    public void PutClothInAvatar()
    {
        //Cycles the clothes in the outfit and makes the avatar wear them
        foreach (string clothName in outfitClothesArray)
        {
            string category = clothName.Split("_")[0]; //Take the category from the name
            string nameWithoutCategory = clothName.Replace(category + "_", ""); //Cloth name without category

            //Put the single cloth in the avatar
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

            if (!shoppingCartMenu.GetComponent<ManageShoppingCartMenu>().AlreadyPresentInCart(nameWithoutCategory))
            {
                //Path for taking the corresponding image
                string completePath = $"{basePath}/{category}/{nameWithoutCategory}";

                //Takes the image related to the name of the cloth (from the resources folder) to put it in the cart card
                StartCoroutine(FindFileAndCompleteCard(completePath, nameWithoutCategory, category));
            }
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

        float price = manageCardCatalog.GetPriceFromNameAndCategory(clothName, category);

        //Complete that card putting the image and the cloth name
        newCartElement.GetComponent<CartElement>().CompleteCartCard(clothName, category, auxTexture, price);

        yield return null;
    }

    //Destroy this card
    public void Remove()
    {
        Destroy(this.gameObject);
        manageRecommendedMenu.RemoveOutfit(outfitClothesInString);
        manageRecommendedMenu.UpdateEveryPercentage();
    }

    //Increase the number of person who recommend this outfit and update the name in the card
    public void UpdateRecommendCard()
    {
        numOfRecommend++;
        UpdateUserName();
    }

    //Open the info menu of the recommended outfit
    public void ShowInfoSpecificOutfit()
    {
        manageRecommendedMenu.ShowOutfitRecommendedNames(outfitClothesInString);
    }

    //Return the color of the gradient associated at the recommended percentage 
    public Color ColorFromGradient(float value)
    {
        return gradient.Evaluate(value);
    }

    //Update the recommended percentage for this outfit
    public void ChangePercentage(float value)
    {
        recommendBarSlider.value = value; //Change the value in the slider
        percentage.text = $"{(value * 100):F2} %"; //Update the percentage in the bar
        barPercentage.color = ColorFromGradient(value); //Chenge the color of the bar
    }


    //--------------- GET ----------------------
    public string GetOutfitClothesInString()
    {
        return outfitClothesInString;
    }
}