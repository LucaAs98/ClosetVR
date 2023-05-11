using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendClothNameToRecommend : MonoBehaviour
{
    private GameObject rootNetworkObj;

    // Start is called before the first frame update
    void Start()
    {
        rootNetworkObj = this.transform.root.gameObject;
    }

    //We take the name of the cloth and we call the serverRpc for activate the hint at the specific cloth
    public void RecommendCloth()
    {
        string clothName = this.transform.GetChild(0).name;
        rootNetworkObj.GetComponent<RecommendCloth>().RecommendClothServerRpc(clothName);
    }
}