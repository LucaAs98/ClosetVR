using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ManageChangeCloth : NetworkBehaviour
{
    [SerializeField] private CapsuleCollider[] collidersList;
    [SerializeField] private Transform tShirtPoint;
    [SerializeField] private Transform trousersPoint;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftFoot;
    [SerializeField] private Transform rightFoot;
    [SerializeField] private Transform closet;

    private GameObject cloth;

    private void ChangeClothBase(string clothName)
    {
        string[] splitArray = clothName.Split(char.Parse("_"));
        string type = splitArray[0];
        string material = splitArray[1];


        Debug.Log("Try cloth in server: " + clothName + "\nCloth type: " + type + "\nCloth material: " + material);


        cloth = GameObject.Find(clothName);

        PutColliders(cloth);

        cloth.GetComponentInChildren<Cloth>().enabled = false;

        if (type == "T-Shirt")
        {
            Debug.Log("E' UNA MAGLIETTA!!!");
            cloth.transform.SetParent(tShirtPoint, false);
            MoveHands();
        }
        else if (type == "Trousers")
        {
            Debug.Log("E' UNA PANTALONE!!!");
            cloth.transform.SetParent(trousersPoint, false);
            MoveFeet();
        }


        cloth.GetComponentInChildren<Cloth>().enabled = true;
    }

    public void ChangeCloth(string clothName)
    {
        ChangeClothBase(clothName);
        ChangeClothClientRpc(clothName);
    }

    [ClientRpc]
    private void ChangeClothClientRpc(string clothName)
    {
        ChangeClothBase(clothName);
    }

    public void RemoveCloth(GameObject cloth)
    {
        //----- !!!!! ATTENZIONE! SE DA ERRORE FARE IL CHECK DEL NOME, POTREBBE ESSERCI "(Clone)" ALLA FINE.
        GameObject hanger = closet.GetComponent<ManageCloset>().GetHangerFromClothName(cloth.name);
        GameObject attachPoint = hanger.GetComponent<ManageHanger>().GetClothAttachPoint();


        RemoveColliders(cloth);

        cloth.GetComponentInChildren<Cloth>().enabled = false;
        cloth.transform.SetParent(attachPoint.transform, false);
        cloth.GetComponentInChildren<Cloth>().enabled = true;
    }

    private void PutColliders(GameObject cloth)
    {
        cloth.GetComponentInChildren<Cloth>().capsuleColliders = collidersList;
    }

    private void RemoveColliders(GameObject cloth)
    {
        cloth.GetComponentInChildren<Cloth>().capsuleColliders = null;
    }

    private void MoveHands()
    {
        Debug.Log("Disattivo mani");
        ActivateDeactivateHandTracking(false);

        Debug.Log("Muovo mani");
        leftHand.position = cloth.GetComponent<InitHandPosition>().GetLeftHandPosition();
        rightHand.position = cloth.GetComponent<InitHandPosition>().GetRightHandPosition();

        ActivateDeactivateHandTracking(true);
    }

    private void MoveFeet()
    {
        Debug.Log("Disattivo piedi");
        ActivateDeactivateFeetTracking(false);

        Debug.Log("Muovo piedi");
        leftFoot.position = cloth.GetComponent<InitFeetPosition>().GetLeftFootPosition();
        rightFoot.position = cloth.GetComponent<InitFeetPosition>().GetRightFootPosition();

        ActivateDeactivateFeetTracking(true);
    }


    private void ActivateDeactivateHandTracking(bool enable)
    {
        leftHand.gameObject.SetActive(enable);
        rightHand.gameObject.SetActive(enable);
    }

    private void ActivateDeactivateFeetTracking(bool enable)
    {
        leftFoot.GetComponent<IKFootSolver>().enabled = enable;
        rightFoot.GetComponent<IKFootSolver>().enabled = enable;
    }
}