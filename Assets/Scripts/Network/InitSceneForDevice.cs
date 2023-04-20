using UnityEngine;

public class InitSceneForDevice : MonoBehaviour
{
    [SerializeField] private GameObject arSession;
    [SerializeField] private GameObject canvasAnd;
    [SerializeField] private GameObject VRStuffs;


    void Start()
    {
        bool isAndroid = Application.platform == RuntimePlatform.Android;

        if (isAndroid)
        {
            Instantiate(canvasAnd);
            Destroy(GameObject.Find("EventSystem"));
        }

        arSession.gameObject.SetActive(isAndroid);
        VRStuffs.gameObject.SetActive(!isAndroid);
    }
}