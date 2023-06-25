using UnityEngine;

public class ManageShoppingCartMenu : MonoBehaviour
{
    [SerializeField] private Transform cartClothes;

    //Useful only for the cartClothes reference
    public Transform GetCartClothes()
    {
        return cartClothes;
    }
}