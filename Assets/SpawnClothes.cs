using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnClothes : MonoBehaviour
{
    private GameObject[] t_shirtsList; //List of t-shirts to add
    [SerializeField] private GameObject[] trousersList; //List of trousers to add
    [SerializeField] private GameObject[] shoesList; //List of shoes to add
    [SerializeField] private GameObject cardBtnCompletePrefab; //Prefab of the card button complete to instantiate
    [SerializeField] private Transform containerTShirts; //Container of all the t-shirts in the menu
    [SerializeField] private Transform containerTrousers; //Container of all the trousers in the menu
    [SerializeField] private Transform containerShoes; //Container of all the shoes in the menu

    private RenderTexture renderTexture; //Render texture for the visualization of the camera in the raw image
    private GameObject cardTextGameObj; //Text of the cardBtnComplete
    private GameObject cardBtnGameObj; //Card button we are instantiating
    private Transform prefabToComplete; //"3DRepresentation"
    private Transform parent3dModel; //"3DModelParent"
    private Transform camera; //Camera where we visualize the 3D obj that we want to put in the scroll menu
    private Transform representation2D; //Raw image where we visualize in 2D what the camera is seeing

    void Start()
    {
        t_shirtsList = GameObject.Find("TShirtsContainer").GetComponent<ManageCloset>().GetTShirtsGameObjects();

        //Transform[] allContainers = { containerTShirts, containerTrousers, containerShoes };
        Transform[] allContainers = { containerTShirts };

        //GameObject[][] allLists = { t_shirtsList, trousersList, shoesList };
        GameObject[][] allLists = { t_shirtsList };
       
        
        int i = 0;

        foreach (var container in allContainers)
        {
            if (container != null)
            {
                GameObject[] currentListOfClothes = allLists[i];

                foreach (var cloth in currentListOfClothes)
                {
                    AddCardBtn(container, cloth);
                }

                i++;
            }
        }
    }

    //Add the specific cloth to the specific container
    private void AddCardBtn(Transform container, GameObject cloth)
    {
        //Instantiate base prefab
        cardBtnGameObj = Instantiate(cardBtnCompletePrefab, container);
        prefabToComplete = cardBtnGameObj.transform.GetChild(0);

        //Set correct tag to the prefab and its children
        SetCorrectLayer(cloth, "UICamera");

        //Instantiate it in the correct parent
        parent3dModel = prefabToComplete.GetChild(1);
        Instantiate(cloth, parent3dModel);

        //Set the text in the card with the name of the cloth
        cardTextGameObj = cardBtnGameObj.transform.GetChild(1).gameObject;
        cardTextGameObj.GetComponent<TextMeshProUGUI>().text = cloth.name;

        //Create render texture
        renderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        renderTexture.name = cloth.name + "RenderTexture";

        //Take the camera and set the render texture
        camera = prefabToComplete.GetChild(0);
        camera.GetComponent<Camera>().targetTexture = renderTexture;

        //Take the raw image and set the render texture
        representation2D = prefabToComplete.GetChild(2);
        representation2D.GetComponent<RawImage>().texture = renderTexture;
    }

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
}