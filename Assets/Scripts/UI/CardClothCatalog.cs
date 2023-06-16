using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardClothCatalog : MonoBehaviour
{
    [SerializeField] private RawImage clothImage; //Cloth image
    [SerializeField] private TextMeshProUGUI clothName; //Cloth name
    [SerializeField] private GameObject cartElement; //Cart element prefab 

    private Transform cartClothes; //Container of cart clothes


    //Adds the cloth in the shopping cart
    public void AddToCart()
    {
        GameObject shoppingCart = GameObject.FindGameObjectWithTag("ShoppingCartClothes");
        cartClothes = shoppingCart.GetComponent<ManageShoppingCartMenu>().GetCartClothes(); //Container of cart clothes

        //Takes the same texture of this object. Don't need to reload the file from the resources images
        Texture2D auxTexture = (Texture2D)this.clothImage.texture;

        //Instantiate the cart card in the shopping cart
        GameObject newCartElement = Instantiate(cartElement, cartClothes);
        //Complete that card putting the image and the cloth name
        newCartElement.GetComponent<CartElement>().CompleteCartCard(this.clothName.text, auxTexture);
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

    public TextMeshProUGUI GetClothName()
    {
        return clothName;
    }

    public RawImage GetClothImage()
    {
        return clothImage;
    }
}