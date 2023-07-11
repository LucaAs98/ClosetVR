using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportWithRightController : MonoBehaviour
{
    private XRRayInteractor rayInteractor;
    private GameObject avatar; //Avatar, useful for wearing the cloth
    private XRSimpleInteractable simpleInteractable; //Interactable component of this cloth
    private RaycastHit res;


    //Init all the useful variables
    private void Awake()
    {
        //Take the avatar
        avatar = GameObject.FindGameObjectWithTag("Avatar");
        //Add listener for the interactable component
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        simpleInteractable.selectEntered.AddListener(Selected);
        rayInteractor = GameObject.Find("RightHand Controller").GetComponent<XRRayInteractor>();
    }


    private void Selected(SelectEnterEventArgs interactor)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out res))
        {
            Vector3 groundPt = res.point;
            Debug.Log($"Coordinates in {res.transform.name}: {groundPt}");
            avatar.transform.parent.position = groundPt;
        }
    }
}