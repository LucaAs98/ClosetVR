using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayInteraction : MonoBehaviour
{
    private XRBaseInteractor interactor;

    private void Awake()
    {
        // Get the XR Interactor component attached to this object
        interactor = GetComponent<XRBaseInteractor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the object you want to interact with
        if (other.CompareTag("Cloth"))
        {
            // Handle the collision when the interactor touches the object
            Debug.Log("Interactor touched the object!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collider belongs to the object you want to interact with
        if (other.CompareTag("Cloth"))
        {
            // Handle the collision when the interactor stops touching the object
            Debug.Log("Interactor stopped touching the object!");
        }
    }
}
