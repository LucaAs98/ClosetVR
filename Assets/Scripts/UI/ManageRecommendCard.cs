using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageRecommendCard : MonoBehaviour
{
    [SerializeField] private Transform clothAvatar;
    [SerializeField] private TextMeshProUGUI recommendByName;
    [SerializeField] private GameObject cartElement;
    [SerializeField] private Camera cardCamera;
    [SerializeField] private RawImage clothRepresentation2D;
    [SerializeField] private GameObject cartItemPrefab;
    private Transform cartClothes;
    private string[] outfitClothes;
    private string basePath = "ClothImages";

    public void SetUserName(string name)
    {
        recommendByName.text = $"Recommend by: {name}";
    }

    public Transform GetClothAvatar()
    {
        return clothAvatar;
    }

    public void ConfigureCard(string clothNames, string name)
    {
        //Activate recommended clothes in outfit
        ActivateRightClothes(clothNames);
        SetCorrectRenderTexture();
        SetUserName(name);
    }

    private void ActivateRightClothes(string clothNames)
    {
        outfitClothes = clothNames.Split(",");

        foreach (string clothName in outfitClothes)
        {
            string category = clothName.Split("_")[0];
            string newClothName = clothName.Replace(category + "_", "");


            foreach (Transform categories in clothAvatar)
            {
                if (categories.name == category)
                {
                    foreach (Transform cloth in categories)
                    {
                        cloth.gameObject.SetActive(cloth.name == newClothName);
                    }
                }
            }
        }
    }

    private void SetCorrectRenderTexture()
    {
        //Create render texture
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 16, RenderTextureFormat.ARGB32);
        renderTexture.name = GetInstanceID() + "RenderTexture";

        //Take the camera and set the render texture
        cardCamera.GetComponent<Camera>().targetTexture = renderTexture;

        //Take the raw image and set the render texture
        clothRepresentation2D.texture = renderTexture;
    }

    public void AddToCart()
    {
        cartClothes = GameObject.FindGameObjectWithTag("ShoppingCartClothes").transform;

        foreach (string clothName in outfitClothes)
        {
            string category = clothName.Split("_")[0];
            string nameWithoutCategory = clothName.Replace(category + "_", "");

            //Path to take the corresponding image
            string completePath = $"{basePath}/{category}/{clothName}";

            StartCoroutine(FindFileAndCompleteCard(completePath, nameWithoutCategory));
        }
    }

    IEnumerator FindFileAndCompleteCard(string completePath, string clothName)
    {
        Texture2D auxTexture = new Texture2D(1920, 1080);
        auxTexture = Resources.Load<Texture2D>(completePath);

        GameObject newCartElement = Instantiate(cartElement, cartClothes);
        newCartElement.GetComponent<CartElement>().CompleteCartCard(clothName, auxTexture);

        yield return null;
    }
}