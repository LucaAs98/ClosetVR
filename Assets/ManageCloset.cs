using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCloset : MonoBehaviour
{
    [SerializeField] private float tShirtDistance = 0.3f;
    [SerializeField] private float maxDistance = 3.9f;
    [SerializeField] private Transform crouchPrefab;
    [SerializeField] private Transform[] tShirtList;

    private Transform tShirtContainer;
    private Transform currentCrouchTShirtContainer;
    private List<GameObject> crouchList = new();

    void Start()
    {
        tShirtContainer = this.transform;
        float newDistance = 0;

        foreach (var tShirt in tShirtList)
        {
            if (newDistance > maxDistance)
            {
                Debug.LogErrorFormat(
                    "Massima quantità di vestiti raggiunta in questa parte dell'Armadio! Non vengono spawnati tutti i vestiti");
                return;
            }

            Transform crouchToSpawn = crouchPrefab;
            crouchToSpawn.position = new Vector3(newDistance, crouchToSpawn.position.y, crouchToSpawn.position.z);
            Transform currentCrouch = Instantiate(crouchPrefab, tShirtContainer);
            crouchList.Add(currentCrouch.gameObject);

            currentCrouchTShirtContainer = currentCrouch.GetComponent<ManageCrouch>().GetTShirtContainer().transform;
            Instantiate(tShirt, currentCrouchTShirtContainer);

            newDistance += tShirtDistance;
        }
    }

    public void ActiveCrouchHint(string clothName)
    {
        //Crouch of the cloth we want to recommend
        GameObject clothCrouch = GetCrouchFromClothName(clothName);

        //Activate only clothCrouch hint and deactivate the others
        foreach (var crouch in crouchList)
        {
            if (clothCrouch != crouch)
            {
                //Deactivate others
                crouch.GetComponent<ManageCrouch>().DeactivateHint();
            }
            else
            {
                //Activate the clothCrouch Hint
                clothCrouch.GetComponent<ManageCrouch>().ActivateHint();
            }
        }
    }

    private GameObject GetCrouchFromClothName(string clothName)
    {
        foreach (var currentCrouch in crouchList)
        {
            if (currentCrouch.GetComponent<ManageCrouch>().GetTShirtName() == clothName)
            {
                return currentCrouch;
            }
        }

        return null;
    }

    public GameObject[] GetTShirtsGameObjects()
    {
        GameObject[] newList = new GameObject[tShirtList.Length];

        for (int i = 0; i < tShirtList.Length; i++)
        {
            newList[i] = tShirtList[i].gameObject;
        }

        return newList;
    }
}