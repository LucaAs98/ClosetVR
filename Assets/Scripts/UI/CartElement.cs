using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartElement : MonoBehaviour
{
    [SerializeField] private RawImage clothImage;
    [SerializeField] private TextMeshProUGUI clothName;

    private TextMeshProUGUI cartItemCounter;


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

    public void CompleteCartCard(string name, RawImage rawImage)
    {
        this.clothName.text = name;
        this.clothImage.texture = rawImage.texture;

        cartItemCounter = GameObject.FindGameObjectWithTag("CartItemCounter").GetComponent<TextMeshProUGUI>();

        int newNumberOfClothes = int.Parse(cartItemCounter.text) + 1;
        cartItemCounter.text = newNumberOfClothes.ToString();
    }

    public void RemoveCloth()
    {
        int newNumberOfClothes = int.Parse(cartItemCounter.text) - 1;
        cartItemCounter.text = newNumberOfClothes.ToString();

        Destroy(this.gameObject);
    }
}