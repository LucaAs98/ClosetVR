using UnityEngine;

public class InitSceneForDevice : MonoBehaviour
{
    [SerializeField] private GameObject arSession;
    [SerializeField] private GameObject canvasAnd;
    [SerializeField] private GameObject VRStuffs;
    [SerializeField] private GameObject VideochatWall;


    void Start()
    {
        bool isAndroid = Application.platform == RuntimePlatform.Android;




        if (isAndroid)
        {
            Destroy(VRStuffs);

            Instantiate(canvasAnd);
            Instantiate(arSession);
        }
        else
        {
            Instantiate(VideochatWall);
        }
    }
}