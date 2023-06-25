using UnityEngine;

public class ManageMirrorCards : MonoBehaviour
{
    private GameObject avatar;

    void Start()
    {
        avatar = GameObject.FindGameObjectWithTag("Avatar");
    }

    //Put the cloth in the avatar
    public void PutCloth(string clothName, string clothCategory)
    {
        //Take the avatar
        avatar = GameObject.FindGameObjectWithTag("Avatar");

        //Put the clothes on the avatar
        avatar.transform.parent.GetComponent<ManageChangeCloth>().ChangeCloth(clothName, clothCategory);
    }
}