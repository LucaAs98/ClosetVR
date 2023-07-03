using System.Collections;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine.Windows;

#else
using System.IO;
#endif

public class TakePhoto : MonoBehaviour
{
    [SerializeField] private GameObject cameraPhoto;
    [SerializeField] private GameObject attachPoint;
    [SerializeField] private GameObject armatureWithClothes;

    private GameObject pastCloth;

    public int resWidth = 2550;
    public int resHeight = 3300;

    private static string path = "Assets/Resources/ClothImages/";
    private static string currentPath = "";

    private Vector3[] cameraMovements =
    {
        new Vector3(0, 0, 0), new Vector3(0, -0.67f, 0), new Vector3(0f, -1.15f, -0.7f), new Vector3(0f,0.476000011f,-0.823000014f), new Vector3(0f,0.416000009f,-0.904999971f), new Vector3(-0.342999995f,-0.224000007f,-1.30799997f)
    };

    void Start()
    {
        DeleteImgInDirectory();
        TakePhotos();
    }

    private void DeleteImgInDirectory()
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path);
            Debug.Log("La directory esiste, cancellata");
        }

        Directory.CreateDirectory(path);
    }


    public static string ScreenShotName(int width, int height, string clothName)
    {
        return string.Format(currentPath + clothName + ".png");
    }

    public void TakePhotos()
    {
        ClothesWithSkeletonManager armatureManager = armatureWithClothes.GetComponent<ClothesWithSkeletonManager>();

        int indexCameraMovent = 0;

        foreach (Transform category in armatureManager.GetAllCategoryContainers())
        {
            CreateNewFolder(category.name);
            cameraPhoto.transform.localPosition = cameraMovements[indexCameraMovent];

            foreach (Transform cloth in category)
            {
                if (category.name == "Shoes")
                    armatureManager.SetLegsForShoes(true);

                cloth.gameObject.SetActive(true);
                MoveClothInCamera(cloth);
                PrepareCameraAndShoot(cloth.name);
                cloth.gameObject.SetActive(false);
            }

            indexCameraMovent++;
        }

        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    private void CreateNewFolder(string categoryName)
    {
        currentPath = path + categoryName + "/";

        Directory.CreateDirectory(currentPath);

        Debug.Log($"Creata la nuova directory --> {currentPath}");
    }


    private void MoveClothInCamera(Transform cloth)
    {
        if (pastCloth != null)
        {
            pastCloth.SetActive(false);
            Destroy(pastCloth);
        }

        pastCloth = Instantiate(cloth, attachPoint.transform).gameObject;
    }

    private void PrepareCameraAndShoot(string clothName)
    {
        Camera componentCamera = cameraPhoto.GetComponent<Camera>();

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        componentCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        componentCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        componentCamera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenShotName(resWidth, resHeight, clothName);
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
    }
}