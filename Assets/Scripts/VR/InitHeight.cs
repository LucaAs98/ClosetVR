using Unity.XR.CoreUtils;
using UnityEngine;

public class InitHeight : MonoBehaviour
{
    [SerializeField] GameObject avatar;
    [SerializeField] GameObject referenceSphere;
    private GameObject checkingPlane;


    private void Start()
    {
        checkingPlane = GameObject.Find("PlaneView");
    }

    public void Init(Vector3 newScale)
    {
        this.transform.localScale = newScale;
        avatar.GetComponent<VRRig>().enabled = true;

        InvokeRepeating("FixCameraOffset", 0.02f, 0.02f);
    }

    private void FixCameraOffset()
    {
        Vector3 spherePosition = referenceSphere.transform.position;
        Vector3 planePosition = checkingPlane.transform.position;


        float differenceY = Mathf.Abs(spherePosition.y - planePosition.y);

        if (differenceY < 0.01)
        {
            CancelInvoke();
            Destroy(checkingPlane.transform.parent.gameObject);
            gameObject.GetComponent<InitHeight>().enabled = false;
        }
        else
        {
            if (spherePosition.y <= planePosition.y)
            {
                gameObject.GetComponent<XROrigin>().CameraYOffset += 0.01f;
            }
            else if (spherePosition.y > planePosition.y)
            {
                gameObject.GetComponent<XROrigin>().CameraYOffset -= 0.01f;
            }
        }
    }
}
