using System;
using TMPro;
using UnityEngine;

public class ManageShoppingCartMenu : MonoBehaviour
{
    [SerializeField] private Transform cartClothes;

    [SerializeField] private TextMeshProUGUI priceInShoppingCart;

    private float price = 0f;

    //Useful only for the cartClothes reference
    public Transform GetCartClothes()
    {
        return cartClothes;
    }

    public void AddClothPrice(float newClothPrice)
    {
        price += newClothPrice;

        priceInShoppingCart.text = $"Price: {price.ToString("F2")}$";
    }

    public void RemoveClothPrice(float oldClothPrice)
    {
        price -= oldClothPrice;

        priceInShoppingCart.text = $"Price: {price.ToString("F2")}$";
    }

    public bool AlreadyPresentInCart(string name)
    {
        foreach (Transform cartElement in cartClothes.GetChild(0))
        {
            if (cartElement.GetComponent<CartElement>().GetClothNameTextMeshPro().text == name)
            {
                return true;
            }
        }

        return false;
    }
}