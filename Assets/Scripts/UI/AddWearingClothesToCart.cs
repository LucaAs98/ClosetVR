using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWearingClothesToCart : MonoBehaviour
{
    private GameObject avatar;
    private Transform cartClothes; //Container of cart clothes
    private string basePath = "ClothImages";

    //Prefab to spawn in shopping cart when the vr player want to buy the outfit elements
    [SerializeField] private GameObject cartElement;
    private SpawnCardClothesCatalog manageCardCatalog;

    void Start()
    {
        avatar = GameObject.FindGameObjectWithTag("Avatar");
        manageCardCatalog = GameObject.FindGameObjectWithTag("CatalogMenu").GetComponent<SpawnCardClothesCatalog>();
    }

    //Add wearing clothes to the cart
    public void AddWearingClothToCart()
    {
        List<string> activeClothes = avatar.transform.parent.GetComponent<ManageChangeCloth>().GetActiveClothes();
        foreach (string categoryAndClothName in activeClothes)
        {
            AddToCart(categoryAndClothName);
        }
    }

    //Add the outfit's clothes in cart separately 
    public void AddToCart(string categoryAndClothName)
    {
        Transform shoppingCartMenu = GameObject.FindGameObjectWithTag("ShoppingCartMenu").transform;
        string category = categoryAndClothName.Split("_")[0]; //Take the category from the name
        string nameWithoutCategory = categoryAndClothName.Replace(category + "_", ""); //Cloth name without category

        if (!shoppingCartMenu.GetComponent<ManageShoppingCartMenu>().AlreadyPresentInCart(nameWithoutCategory))
        {
            //Path for taking the corresponding image
            string completePath = $"{basePath}/{category}/{nameWithoutCategory}";

            //Container of cart clothes
            cartClothes = shoppingCartMenu.GetComponent<ManageShoppingCartMenu>().GetCartClothes().GetChild(0);

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

        float price = manageCardCatalog.GetPriceFromNameAndCategory(clothName, category);

        //Complete that card putting the image and the cloth name
        newCartElement.GetComponent<CartElement>().CompleteCartCard(clothName, category, auxTexture, price);

        yield return null;
    }
}