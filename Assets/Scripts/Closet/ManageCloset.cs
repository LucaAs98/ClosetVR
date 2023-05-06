using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ManageCloset : NetworkBehaviour
{
    //--------------------- T_SHIRTS -----------------------------
    [SerializeField] private Transform tShirtSpace;
    [SerializeField] private Transform tShirtHangerPrefab;
    [SerializeField] private Transform[] tShirtList;

    //--------------------- TROUSERS -----------------------------
    [SerializeField] private Transform trousersSpace;
    [SerializeField] private Transform trousersHangerPrefab;
    [SerializeField] private Transform[] trousersList;

    //---------------------- SHOES -------------------------------
    [SerializeField] private Transform shoesSpace;
    [SerializeField] private Transform shoesHangerPrefab;
    [SerializeField] private Transform[] shoesList;


    //List of all the hangers, useful for the Activation/Deactivation of the hints
    private List<GameObject> hangerList = new();
    private Container[] clothContainers; //Containers of one type of cloth. Overwritten everytime
    private Container currentContainer; //Specific conteiner we are considering
    private Transform currHangerAttPoint; //Attach point of the hanger we are considering
    private Transform hangerToSpawn; //Type of hanger we have to spawn
    private int containerIndex; //Index useful to browse the various containers
    private int typeOfClothIndex; //Index useful to browse all the components for a specific type of cloth
    private bool stopThisCloth = false; //Flag useful for stopping the excecution when there is no more space 
    private float newDistance; //Starting point of the cloth position. It is incremented every time

    private int numberOfCloth; //Number of the cloth we are adding at the closet

    //When we start the server we fill the closet with all the clothes
    public override void OnNetworkSpawn()
    {
        //We fill it only if we are the server
        if (IsServer)
            InitCloset();
    }

    private void InitCloset()
    {
        //We create all the arrays and the lists we need
        Transform[] clothsSpaces = { tShirtSpace, trousersSpace, shoesSpace };
        List<Transform[]> clothLists = new List<Transform[]>() { tShirtList, trousersList, shoesList };
        Transform[] clothHangers = { tShirtHangerPrefab, trousersHangerPrefab, shoesHangerPrefab };

        //We iterate all the lists of clothes
        foreach (var clothList in clothLists)
        {
            //We take all the containers associated to the specific type of cloth
            clothContainers = GetAllChildren(clothsSpaces[typeOfClothIndex]);

            //We set the type of hanger we have to spawn every time
            hangerToSpawn = clothHangers[typeOfClothIndex];

            //We init all the variables we need before iterate the specific clothes associated at that type of cloth
            InitBeforeNewTypeOfCloth();

            //We iterate every cloth of the list
            AddClothOfSpecificType(clothList);
        }
    }

    //We iterate every cloth of a specific type
    private void AddClothOfSpecificType(Transform[] clothList)
    {
        foreach (var cloth in clothList)
        {
            if (!stopThisCloth)
            {
                //We move every time the hanger position in the space we have
                hangerToSpawn.position =
                    new Vector3(newDistance, hangerToSpawn.position.y, hangerToSpawn.position.z);

                //We instantiate the hanger in the currentContainer and we add it to our list of hangers
                Transform currentHanger = Instantiate(hangerToSpawn, currentContainer.transform);
                hangerList.Add(currentHanger.gameObject);

                //We take the attachPoint associated to our current hanger and we instantiate the cloth in it
                currHangerAttPoint = GetHangerAttachPoint(currentHanger);
                Instantiate(cloth, currHangerAttPoint);

                //We move the next hanger to the right
                newDistance += currentContainer.GetDistanceBetweenObjInContainer();
                //We pass to the next cloth
                numberOfCloth++;

                //Check if another cloth fits
                CheckIfThereIsSpace();
            }
        }

        typeOfClothIndex++;
    }

    /* Check if another cloth fits. If there is no more space then we move to the next container. If we dont have other containers we stop
     * the insertion of other clothes of the current type. */
    private void CheckIfThereIsSpace()
    {
        if (numberOfCloth > currentContainer.GetMaxNumberOfClothes())
        {
            //If the number of the cloth exceed the max number of clothes that the container can hold we pass to the next container
            containerIndex++;

            //If we dont have other containers we stop the insertion of this type of clothes. 
            if (clothContainers.Length == containerIndex)
            {
                Debug.LogErrorFormat(
                    "Massima quantità di vestiti raggiunta nel container!" +
                    "\n ----- typeOfCloth (1_tshirt, 2_trousers, 3_shoes) -> " + typeOfClothIndex +
                    "\n ----- current container -> " + currentContainer +
                    "\n ----- Non sono stati spawnati tutti i vestiti!");

                stopThisCloth = true;
            }
            else
            {
                //Otherwise we init the number of the cloth, the starting distance and we choose the next container
                numberOfCloth = 1;
                newDistance = 0;
                currentContainer = clothContainers[containerIndex];
            }
        }
    }

    //Reset of all the variables. We are starting another type of clothes list
    private void InitBeforeNewTypeOfCloth()
    {
        stopThisCloth = false;
        currentContainer = clothContainers[0];
        newDistance = 0;
        numberOfCloth = 1;
        containerIndex = 0;
    }

    //Activate the hint of a specific cloth and deactivate the others
    public void ActiveHangerHint(string clothName)
    {
        //Hanger of the cloth we want to recommend
        GameObject clothHanger = GetHangerFromClothName(clothName);

        //Activate only clothHanger hint and deactivate the others
        foreach (var hanger in hangerList)
        {
            if (clothHanger != hanger)
            {
                //Deactivate others
                hanger.GetComponent<ManageHanger>().DeactivateHint();
            }
            else
            {
                //Activate the clothHanger Hint
                clothHanger.GetComponent<ManageHanger>().ActivateHint();
            }
        }
    }


    //------------ GET ---------------

    //Return the Hanger associated to the clothName passed
    private GameObject GetHangerFromClothName(string clothName)
    {
        foreach (var currentHanger in hangerList)
        {
            if (currentHanger.GetComponent<ManageHanger>().GetClothName() == clothName)
            {
                return currentHanger;
            }
        }

        return null;
    }

    //Return the attach point associated to the specific hanger
    private Transform GetHangerAttachPoint(Transform hanger)
    {
        return hanger.GetComponent<ManageHanger>().GetClothAttachPoint().transform;
    }

    //Return all the tshirt of the closet
    public GameObject[] GetTShirtsGameObjects()
    {
        GameObject[] newList = new GameObject[tShirtList.Length];

        for (int i = 0; i < tShirtList.Length; i++)
        {
            newList[i] = tShirtList[i].gameObject;
        }

        return newList;
    }

    //Return all the trousers of the closet
    public GameObject[] GetTrousersGameObjects()
    {
        GameObject[] newList = new GameObject[trousersList.Length];

        for (int i = 0; i < trousersList.Length; i++)
        {
            newList[i] = trousersList[i].gameObject;
        }

        return newList;
    }

    //Return all the shoes of the closet
    public GameObject[] GetShoesGameObjects()
    {
        GameObject[] newList = new GameObject[shoesList.Length];

        for (int i = 0; i < shoesList.Length; i++)
        {
            newList[i] = shoesList[i].gameObject;
        }

        return newList;
    }

    //Return all children of a transform
    private Container[] GetAllChildren(Transform transform)
    {
        List<Container> children = new();

        foreach (Transform child in transform)
            children.Add(child.GetComponent<Container>());

        return children.ToArray();
    }
}