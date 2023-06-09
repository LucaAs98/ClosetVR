using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnClothes : MonoBehaviour
{
    private GameObject[] t_shirtsList; //List of t-shirts to add
    private GameObject[] trousersList; //List of trousers to add
    private GameObject[] shoesList; //List of shoes to add
    private GameObject[] capsList; //List of caps to add
    private GameObject[] glassesList; //List of glasses to add
    private GameObject[] watchesList; //List of watches to add

    [SerializeField] private GameObject cardBtnCompletePrefab; //Prefab of the card button complete to instantiate
    [SerializeField] private Transform containerTShirts; //Container of all the t-shirts in the menu
    [SerializeField] private Transform containerTrousers; //Container of all the trousers in the menu
    [SerializeField] private Transform containerShoes; //Container of all the shoes in the menu
    [SerializeField] private Transform containerCaps; //Container of all the caps in the menu
    [SerializeField] private Transform containerGlasses; //Container of all the glasses in the menu
    [SerializeField] private Transform containerWatches; //Container of all the watches in the menu

    private RenderTexture renderTexture; //Render texture for the visualization of the camera in the raw image
    private GameObject cardTextGameObj; //Text of the cardBtnComplete
    private GameObject cardBtnGameObj; //Card button we are instantiating
    private Transform prefabToComplete; //"3DRepresentation"
    private Transform parent3dModel; //"3DModelParent"
    private Transform camera; //Camera where we visualize the 3D obj that we want to put in the scroll menu
    private Transform representation2D; //Raw image where we visualize in 2D what the camera is seeing


    private GameObject[] currentListOfClothes; //List of clothes we are iterating


    private RawImage imgInCard;
    private string basePath = "ClothImages";

    void Start()
    {
        TakeClothes(); //Get the clothes from the closet
        AddClothes(); //Add the clothes to the menu
    }


    //Add the clothes to the menu
    private void AddClothes()
    {
        //We create the group of containers and clothes list we want to add at the android menu
        Transform[] allContainers =
            { containerTShirts, containerTrousers, containerShoes, containerCaps, containerGlasses, containerWatches };
        GameObject[][] allLists = { t_shirtsList, trousersList, shoesList, capsList, glassesList, watchesList };

        int i = 0; //Used to iterate allLists at the same time of allContainers

        foreach (var container in allContainers)
        {
            currentListOfClothes = allLists[i]; //We take the right list of clothes

            //We add every cloth to the menu
            foreach (var cloth in currentListOfClothes)
            {
                //AddCardBtn(container, cloth);
                NewAddCardBtn(container, cloth);
            }

            i++;
        }
    }

    private void NewAddCardBtn(Transform container, GameObject cloth)
    {
        cardBtnGameObj = Instantiate(cardBtnCompletePrefab, container);
        string clothName = cloth.name;
        string category = clothName.Split("_")[0];

        PutImage(clothName, category);
        PutTitle(clothName, category);
    }

    private void PutTitle(string clothName, string category)
    {
        //Set the text in the card with the name of the cloth
        cardTextGameObj = cardBtnGameObj.transform.GetChild(1).GetChild(1).gameObject;
        string newClothName = clothName.Replace(category + "_", "");
        cardTextGameObj.GetComponent<TextMeshProUGUI>().text = newClothName;
    }

    private void PutImage(string clothName, string category)
    {
        imgInCard = cardBtnGameObj.transform.GetChild(1).GetChild(0).GetComponent<RawImage>();

        string completePath = $"{basePath}/{category}/{clothName}";
        Debug.Log(completePath);
        imgInCard.texture = Resources.Load<Texture2D>(completePath);
    }

















    //Get the clothes from the closet
    private void TakeClothes()
    {
        ManageCloset manageCloset = GameObject.FindGameObjectWithTag("Closet").GetComponent<ManageCloset>();
        t_shirtsList = manageCloset.GetTShirtsGameObjects();
        trousersList = manageCloset.GetTrousersGameObjects();
        shoesList = manageCloset.GetShoesGameObjects();
        capsList = manageCloset.GetCapsGameObjects();
        glassesList = manageCloset.GetGlassesGameObjects();
        watchesList = manageCloset.GetWatchesGameObjects();
    }

    //Add the specific cloth to the specific container
    private void AddCardBtn(Transform container, GameObject cloth)
    {
        //Instantiate base prefab
        cardBtnGameObj = Instantiate(cardBtnCompletePrefab, container);
        prefabToComplete = cardBtnGameObj.transform.GetChild(1).GetChild(0);

        //Set correct tag to the prefab and its children
        SetCorrectLayer(cloth, "UICamera");


        //Instantiate it in the correct parent
        parent3dModel = prefabToComplete.GetChild(1);
        GameObject clothInClient = Instantiate(cloth, parent3dModel);

        DisableClothComponent(clothInClient);

        //Set the text in the card with the name of the cloth
        cardTextGameObj = cardBtnGameObj.transform.GetChild(1).GetChild(1).gameObject;
        cardTextGameObj.GetComponent<TextMeshProUGUI>().text = cloth.name;

        //Create render texture
        renderTexture = new RenderTexture(1920, 1080, 16, RenderTextureFormat.ARGB32);
        renderTexture.name = cloth.name + "RenderTexture";

        //Take the camera and set the render texture
        camera = prefabToComplete.GetChild(0);
        camera.GetComponent<Camera>().targetTexture = renderTexture;

        //Take the raw image and set the render texture
        representation2D = prefabToComplete.GetChild(2);
        representation2D.GetComponent<RawImage>().texture = renderTexture;
    }

    //Set the correct layer to the prefab and its children
    private void SetCorrectLayer(GameObject prefab, string layerName)
    {
        //Set layer to the parent
        prefab.layer = LayerMask.NameToLayer(layerName);

        //Set layer to the children
        foreach (Transform child in prefab.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }

    //If present we remove the cloth component
    private void DisableClothComponent(GameObject cloth)
    {
        Cloth clothComponent = cloth.GetComponentInChildren<Cloth>();
        if (clothComponent != null)
        {
            clothComponent.enabled = false;
        }
    }
}