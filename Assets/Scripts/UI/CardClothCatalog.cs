using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardClothCatalog : MonoBehaviour
{
    [SerializeField] private RawImage clothImage; //Cloth image
    [SerializeField] private TextMeshProUGUI clothName; //Cloth name
    [SerializeField] private GameObject cartElement; //Cart element prefab 

    private Transform cartClothes; //Container of cart clothes
    private string clothCategory; //Cloth category

    private ManageMirrorCards manageMirrorCards;

    void Start()
    {
        manageMirrorCards = this.transform.root.GetComponent<ManageMirrorCards>();
    }

    //Adds the cloth in the shopping cart
    public void AddToCart()
    {
        cartClothes = GameObject.FindGameObjectWithTag("ShoppingCartClothes").transform;

        //Takes the same texture of this object. Don't need to reload the file from the resources images
        Texture2D auxTexture = (Texture2D)this.clothImage.texture;

        //Instantiate the cart card in the shopping cart
        GameObject newCartElement = Instantiate(cartElement, cartClothes);
        //Complete that card putting the image and the cloth name
        newCartElement.GetComponent<CartElement>().CompleteCartCard(this.clothName.text, clothCategory, auxTexture);
    }

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
}