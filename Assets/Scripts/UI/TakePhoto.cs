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
    private GameObject closet;
    private List<Transform[]> clothList;
    private GameObject pastCloth;

    public int resWidth = 2550;
    public int resHeight = 3300;

    private static string path = "Assets/Resources/ClothImages/";
    private static string currentPath = "";

    private Vector3[] cameraMovements =
        { new Vector3(0, 0, 0), new Vector3(0, -0.7f, 0), new Vector3(0.04f, -1.15f, -0.88f) };

    void Start()
    {
        DeleteImgInDirectory();
        closet = GameObject.FindWithTag("Closet");
        clothList = closet.GetComponent<ManageCloset>().GetClothList();
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
        int indexCameraMovent = 0;

        foreach (var specificClothList in clothList)
        {
            


            if (specificClothList.Length > 0)
            {
                CreateNewFolder(specificClothList[0]);
                cameraPhoto.transform.localPosition = cameraMovements[indexCameraMovent];
            }

            foreach (var cloth in specificClothList)
            {
                MoveClothInCamera(cloth);
                PrepareCameraAndShoot(cloth.name);
            }

            indexCameraMovent++;
        }

        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    private void CreateNewFolder(Transform firstElement)
    {
        string clothType = firstElement.name.Split("_")[0];

        currentPath = path + clothType + "/";

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