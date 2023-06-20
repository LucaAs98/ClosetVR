using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMirrorCards : MonoBehaviour
{
    private GameObject avatar;

    void Start()
    {
        avatar = GameObject.FindGameObjectWithTag("Avatar");
    }

    public void PutCloth(string clothName, string clothCategory)
    {
        //Take the avatar
        avatar = GameObject.FindGameObjectWithTag("Avatar");

        //Put the clothes on the avatar
        avatar.GetComponent<ManageChangeCloth>().ChangeCloth(clothName);
    }
}