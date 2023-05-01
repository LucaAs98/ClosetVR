using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnClothes : MonoBehaviour
{
    [SerializeField] private GameObject[] clothesList;
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform containerCards;
    private RenderTexture rt;

    void Start()
    {
        foreach (var cloth in clothesList)
        {
            //Instantiate base prefab
            GameObject btn = Instantiate(prefabToSpawn, containerCards);
            Transform prefabToComplete = btn.transform.GetChild(0);

            //Set correct tag to prefab
            cloth.layer = LayerMask.NameToLayer("UICamera");

            //Instantiate it in the correct parent
            Transform parent3dModel = prefabToComplete.GetChild(1);
            Instantiate(cloth, parent3dModel);

            //Create render texture
            rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            rt.name = cloth.name + "RenderTexture";

            //Take the camera and set the render texture
            Transform camera = prefabToComplete.GetChild(0);
            camera.GetComponent<Camera>().targetTexture = rt;

            //Take the raw image and set the render texture
            Transform representation2D = prefabToComplete.GetChild(2);
            representation2D.GetComponent<RawImage>().texture = rt;


        }
    }
}