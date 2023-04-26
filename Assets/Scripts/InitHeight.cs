using DitzelGames.FastIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitHeight : MonoBehaviour
{
    [SerializeField] GameObject avatar;
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;
    [SerializeField] GameObject head;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Vector3 newScale)
    {
        avatar.transform.localScale = newScale;
        avatar.gameObject.GetComponent<InitPosition>().enabled = true;

        ActivateBoneFollower();
    }

    private void ActivateBoneFollower()
    {
        leftHand.GetComponent<BoneFollower>().enabled = true;
        rightHand.GetComponent<BoneFollower>().enabled = true;
        head.GetComponent<BoneFollower>().enabled = true;
    }
}
