using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardClothCatalog : MonoBehaviour
{
    [SerializeField] private RawImage clothImage;
    [SerializeField] private TextMeshProUGUI clothName;
    [SerializeField] private GameObject cartElement;

    private Transform cartClothes;


    void Start()
    {
        cartClothes = GameObject.FindGameObjectWithTag("ShoppingCartClothes").transform;
    }

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

    public void AddToCart()
    {
        GameObject newCartElement = Instantiate(cartElement, cartClothes);
        newCartElement.GetComponent<CartElement>().CompleteCartCard(this.clothName.text, this.clothImage);
    }
}