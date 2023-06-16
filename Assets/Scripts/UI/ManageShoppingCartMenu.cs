using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageShoppingCartMenu : MonoBehaviour
{
    [SerializeField] private Transform cartClothes;


    public Transform GetCartClothes()
    {
        return cartClothes;
    }
}