using Facepunch;
using UnityEngine;

public class CheckRayInteraction : MonoBehaviour
{
    private GameObject avatar;
    private string clothCategory;

    void Start()
    {
        //Take the avatar
        avatar = GameObject.FindGameObjectWithTag("Avatar");
    }

    private void StartHightlight()
    {
        Highlight.ClearAll();
        Highlight.AddRenderer(this.GetComponent<SkinnedMeshRenderer>());
        Highlight.Rebuild();
    }

    public void HoverEntered()
    {
        Debug.Log($"Sto interagendo con {this.name}");
        StartHightlight();
    }


    public void PutCloth()
    {
        Debug.Log($"Put {this.name} in avatar. Category: {clothCategory}");

        //Put the clothes on the avatar
        avatar.transform.parent.GetComponent<ManageChangeCloth>().ChangeCloth(this.name, clothCategory);
    }

    public void SetClothCategory(string category)
    {
        clothCategory = category;
    }
}