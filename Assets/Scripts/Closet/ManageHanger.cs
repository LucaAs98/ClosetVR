using UnityEngine;

public class ManageHanger : MonoBehaviour
{
    [SerializeField] private GameObject attachPoint; //Parent of the cloth. Modify this for adjusting it in the hanger
    [SerializeField] private GameObject hint; //Hint of the hanger
    private GameObject cloth;


    //Activate and Deactivate the inner hint
    public void ActivateHint()
    {
        hint.SetActive(true);
    }

    public void DeactivateHint()
    {
        hint.SetActive(false);
    }

    //---------- GET / SET --------------
    public GameObject GetClothAttachPoint()
    {
        return attachPoint;
    }

    public GameObject GetHint()
    {
        return hint;
    }

    //Set the cloth attached to the hanger
    public void SetClothInHanger(GameObject newCloth)
    {
        cloth = newCloth;
    }

    //Return the name of the cloth attached to the hanger
    public string GetClothName()
    {
        return cloth.name;
    }
}