using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCollidersToCloth : MonoBehaviour
{
    [SerializeField] private ManageChangeCloth manageChangeCloth;

    private CapsuleCollider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        // colliders = manageChangeCloth.GetColliders();
        //
        // // foreach (var collider in colliders)
        // // {
        // //     collider.enabled = true;
        // // }
        //
        // this.GetComponent<Cloth>().capsuleColliders = colliders;
    }
}