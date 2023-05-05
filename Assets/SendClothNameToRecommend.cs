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

    public void RecommendCloth()
    {
        rootNetworkObj.GetComponent<RecommendCloth>().Recommend(this.transform.GetChild(0).name);
    }
}