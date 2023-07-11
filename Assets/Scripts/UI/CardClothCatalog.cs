using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardClothCatalog : MonoBehaviour
{
    [SerializeField] private RawImage clothImage; //Cloth image
    [SerializeField] private TextMeshProUGUI clothName; //Cloth name
    [SerializeField] private TextMeshProUGUI clothPrice; //Cloth price
    [SerializeField] private GameObject cartElement; //Cart element prefab 

    private float priceInFloat;
    private Transform cartClothes; //Container of cart clothes
    private string clothCategory; //Cloth category

    private ManageMirrorCards manageMirrorCards; //ManageMirrorCard Component


    //Init some variables
    void Start()
    {
        priceInFloat = Random.Range(100, 2000) / 10f;
        clothPrice.text = $"Price: {priceInFloat.ToString("F2")}$";
        manageMirrorCards = this.transform.root.GetComponent<ManageMirrorCards>();
    }

    //Adds the cloth in the shopping cart
    public void AddToCart()
    {
        Transform shoppingCartMenu = GameObject.FindGameObjectWithTag("ShoppingCartMenu").transform;
        //Container of cart clothes
        cartClothes = shoppingCartMenu.GetComponent<ManageShoppingCartMenu>().GetCartClothes().GetChild(0);

        if (!shoppingCartMenu.GetComponent<ManageShoppingCartMenu>().AlreadyPresentInCart(this.clothName.text))
        {
            //Takes the same texture of this object. Don't need to reload the file from the resources images
            Texture2D auxTexture = (Texture2D)this.clothImage.texture;

            //Instantiate the cart card in the shopping cart
            GameObject newCartElement = Instantiate(cartElement, cartClothes);
            //Complete that card putting the image and the cloth name
            newCartElement.GetComponent<CartElement>()
                .CompleteCartCard(this.clothName.text, clothCategory, auxTexture, priceInFloat);
        }
    }

    //Put cloth of the card in the avatar
    public void PutCloth()
    {
        manageMirrorCards.PutCloth(clothName.text, clothCategory);
    }

    //-------------------------- GET and SET -----------------------
    public void SetClothImage(Texture2D texture)
    {
        clothImage.texture = texture;
    }

    public void SetClothName(string name)
    {
        clothName.text = name;
    }

    public void SetClothCategory(string category)
    {
        clothCategory = category;
    }

    public TextMeshProUGUI GetClothName()
    {
        return clothName;
    }

    public RawImage GetClothImage()
    {
        return clothImage;
    }

    public float GetPriceInFloat()
    {
        return priceInFloat;
    }
}