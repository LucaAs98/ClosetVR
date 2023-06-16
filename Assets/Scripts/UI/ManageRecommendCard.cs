using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageRecommendCard : MonoBehaviour
{
    [SerializeField] private Transform clothAvatar; //Root of mannequin clothes
    [SerializeField] private TextMeshProUGUI recommendByName; //Title of the card: "Recommended by {userName}"

    //Prefab to spawn in shopping cart when the vr player want to buy the outfit elements
    [SerializeField] private GameObject cartElement;

    [SerializeField] private Camera cardCamera; //Camera of the render texture.
    [SerializeField] private RawImage clothRepresentation2D; //RawImage where the render texture is put

    private Transform cartClothes; //Container of cart clothes
    private string[] outfitClothes; //Array of the outfit's clothing names

    private string basePath = "ClothImages";

    //Set the title of the card
    public void SetUserName(string name)
    {
        recommendByName.text = $"Recommend by: {name}";
    }

    //Complete the card with all necessary data
    public void ConfigureCard(string clothNames, string name)
    {
        ActivateRightClothes(clothNames); //Activate recommended clothes in outfit
        SetCorrectRenderTexture(); //Create and set the render texture
        SetUserName(name); //Set the name in "Recommended by: ..."
    }

    //Activate recommended clothes in outfit
    private void ActivateRightClothes(string clothNames)
    {
        //clothNames contains all the cloth names divided by ","
        outfitClothes = clothNames.Split(",");

        //For every clothName, activate it in the mannequin (outfit)
        foreach (string clothName in outfitClothes)
        {
            string category = clothName.Split("_")[0]; //Take the category from the name
            string newClothName = clothName.Replace(category + "_", ""); //Cloth name without category

            //For every general category, activates clothes only if they are present in outfit
            foreach (Transform categories in clothAvatar)
            {
                if (categories.name == category)
                {
                    foreach (Transform cloth in categories)
                    {
                        cloth.gameObject.SetActive(cloth.name == newClothName);
                    }
                }
            }
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
        GameObject shoppingCart = GameObject.FindGameObjectWithTag("ShoppingCartClothes");
        cartClothes = shoppingCart.GetComponent<ManageShoppingCartMenu>().GetCartClothes(); //Container of cart clothes

        //For every cloth in the outfit add the card in the shopping cart.
        foreach (string clothName in outfitClothes)
        {
            string category = clothName.Split("_")[0]; //Take the category from the name
            string nameWithoutCategory = clothName.Replace(category + "_", ""); //Cloth name without category

            //Path for taking the corresponding image
            string completePath = $"{basePath}/{category}/{clothName}";

            //Takes the image related to the name of the cloth (from the resources folder) to put it in the cart card
            StartCoroutine(FindFileAndCompleteCard(completePath, nameWithoutCategory));
        }
    }

    //Takes the image related to the name of the cloth (from the resources folder) to put it in the cart card
    IEnumerator FindFileAndCompleteCard(string completePath, string clothName)
    {
        //Save the texture related to the cloth in auxTexture
        Texture2D auxTexture = new Texture2D(1920, 1080);
        auxTexture = Resources.Load<Texture2D>(completePath);

        //Instantiate the cart card in the shopping cart
        GameObject newCartElement = Instantiate(cartElement, cartClothes);
        //Complete that card putting the image and the cloth name
        newCartElement.GetComponent<CartElement>().CompleteCartCard(clothName, auxTexture);

        yield return null;
    }

    //Destroy this card
    public void Remove()
    {
        Destroy(this.gameObject);
    }
}