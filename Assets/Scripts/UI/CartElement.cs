using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartElement : MonoBehaviour
{
    [SerializeField] private RawImage clothImage; //Cloth image in cart element
    [SerializeField] private TextMeshProUGUI clothNameTextMeshPro; //Cloth name in cart element
    private string clothCategory;   //Cloth category

    private TextMeshProUGUI cartItemCounter; //Counter of elements in the cart
    private ManageMirrorCards manageMirrorCards; //ManageMerrorCards component

    //Init some variables
    void Start()
    {
        manageMirrorCards = this.transform.root.GetComponent<ManageMirrorCards>();
    }

    //Complete the cart card putting the image and the cloth name
    public void CompleteCartCard(string name, string category, Texture2D texture)
    {
        //Set the variable of this specific object
        clothNameTextMeshPro.text = name;
        clothCategory = category;
        clothImage.texture = texture;

        //Counter of elements in the cart
        cartItemCounter = GameObject.FindGameObjectWithTag("CartItemCounter").GetComponent<TextMeshProUGUI>();
        //Add 1 at the counter of the elements in cart
        int newNumberOfClothes = int.Parse(cartItemCounter.text) + 1;
        cartItemCounter.text = newNumberOfClothes.ToString();
    }

    //Remove the cloth card from the cart
    public void RemoveCloth()
    {
        //Subtracts one at the counter of the elements in cart
        int newNumberOfClothes = int.Parse(cartItemCounter.text) - 1;
        cartItemCounter.text = newNumberOfClothes.ToString();

        //Destory this card
        Destroy(this.gameObject);
    }

    //Put cloth of the card in the avatar
    public void PutClothInAvatar()
    {
        manageMirrorCards.PutCloth(clothNameTextMeshPro.text, clothCategory);
    }

    //-------------------------- GET and SET -----------------------
    public void SetClothImage(Texture2D texture)
    {
        clothImage.texture = texture;
    }

    public void SetclothNameTextMeshPro(string name)
    {
        clothNameTextMeshPro.text = name;
    }

    public TextMeshProUGUI GetclothNameTextMeshPro()
    {
        return clothNameTextMeshPro;
    }

    public RawImage GetClothImage()
    {
        return clothImage;
    }
}