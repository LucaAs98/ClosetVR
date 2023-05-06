using UnityEngine;

public class ManageHanger : MonoBehaviour
{
    [SerializeField] private GameObject attachPoint; //Parent of the cloth. Modify this for adjusting it in the hanger
    [SerializeField] private GameObject hint; //Hint of the hanger


    //Activate and Deactivate the inner hint
    public void ActivateHint()
    {
        hint.SetActive(true);
    }

    public void DeactivateHint()
    {
        hint.SetActive(false);
    }

    //---------- GET --------------
    public GameObject GetClothAttachPoint()
    {
        return attachPoint;
    }

    public GameObject GetHint()
    {
        return hint;
    }

    //Return the name of the cloth attached to the hanger
    public string GetClothName()
    {
        return attachPoint.transform.GetChild(0).name;
    }
}