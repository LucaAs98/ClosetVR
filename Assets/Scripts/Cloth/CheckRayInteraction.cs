using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Outline), typeof(BoxCollider), typeof(XRSimpleInteractable))]
public class CheckRayInteraction : MonoBehaviour
{
    private GameObject avatar; //Avatar, useful for wearing the cloth
    private string clothCategory; //Category of this cloth, used for wearing the cloth
    private XRSimpleInteractable simpleInteractable; //Interactable component of this cloth
    private Outline thisOutline; //Outline component of this cloth


    //Init all the useful variables
    private void Awake()
    {
        //Take the avatar
        avatar = GameObject.FindGameObjectWithTag("Avatar");

        //Add listener for the interactable component
        simpleInteractable = GetComponent<XRSimpleInteractable>();

        simpleInteractable.firstHoverEntered.AddListener(HoverEntered);
        simpleInteractable.hoverExited.AddListener(HoverExited);
        simpleInteractable.selectEntered.AddListener(Selected);

        //Init outline component of this cloth
        thisOutline = GetComponent<Outline>();
        thisOutline.enabled = false;
    }

    private void HoverEntered(HoverEnterEventArgs interactor) => thisOutline.enabled = true;

    private void HoverExited(HoverExitEventArgs interactor) => thisOutline.enabled = false;


    private void Selected(SelectEnterEventArgs interactor)
    {
        //Put the clothes on the avatar
        avatar.transform.parent.GetComponent<ManageChangeCloth>().ChangeCloth(this.name, clothCategory);
    }

    public void SetClothCategory(string category) => clothCategory = category;
}