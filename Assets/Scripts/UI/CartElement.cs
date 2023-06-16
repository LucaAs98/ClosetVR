using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartElement : MonoBehaviour
{
    [SerializeField] private RawImage clothImage; //Cloth image in cart element
    [SerializeField] private TextMeshProUGUI clothName; //Cloth name in cart element

    private TextMeshProUGUI cartItemCounter; //Counter of elements in the cart

    //Complete the cart card putting the image and the cloth name
    public void CompleteCartCard(string name, Texture2D texture)
    {
        //Set the variable of this specific object
        this.clothName.text = name;
        this.clothImage.texture = texture;

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