using System;
using System.Collections.Generic;
using System.Linq;
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

    //---------------------- CAPS -------------------------------
    [SerializeField] private Transform capsSpace;
    [SerializeField] private Transform capsHangerPrefab;
    [SerializeField] private Transform[] capsList;

    //---------------------- GLASSES -------------------------------
    [SerializeField] private Transform glassesSpace;
    [SerializeField] private Transform glassesHangerPrefab;
    [SerializeField] private Transform[] glassesList;

    //---------------------- WATCHES -------------------------------
    [SerializeField] private Transform watchesSpace;
    [SerializeField] private Transform watchesHangerPrefab;
    [SerializeField] private Transform[] watchesList;

    private List<Transform[]> clothLists;
    private Transform[] clothHangers;
    private Transform[] clothsSpaces;

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


    void Awake()
    {
        //Creation of all the arrays and the lists neededed
        clothsSpaces = new Transform[]
            { tShirtSpace, trousersSpace, shoesSpace, capsSpace, glassesSpace, watchesSpace };
        clothLists = new List<Transform[]>()
            { tShirtList, trousersList, shoesList, capsList, glassesList, watchesList };
        clothHangers = new Transform[]
        {
            tShirtHangerPrefab, trousersHangerPrefab, shoesHangerPrefab, capsHangerPrefab, glassesHangerPrefab,
            watchesHangerPrefab
        };
    }

    //Fill the closet with all the clothes
    public override void OnNetworkSpawn()
    {
        //Fill it only if we are the server
        if (IsServer)
            InitCloset();
    }

    private void InitCloset()
    {
        //Iterate all the lists of clothes
        foreach (var clothList in clothLists)
        {
            //Take all the containers associated to the specific type of cloth
            clothContainers = GetAllChildren(clothsSpaces[typeOfClothIndex]);

            //Set the type of hanger we have to spawn every time
            hangerToSpawn = clothHangers[typeOfClothIndex];

            //Init all the variables needed before iterate the specific clothes associated at that type of cloth
            InitBeforeNewTypeOfCloth();

            //Iterate every cloth of the list
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
                //Move the hanger position in the space we have
                hangerToSpawn.localPosition =
                    new Vector3(newDistance, hangerToSpawn.position.y, hangerToSpawn.position.z);

                //Instantiate the hanger in the currentContainer and add it to the hangers' list 
                Transform currentHanger = Instantiate(hangerToSpawn, currentContainer.GetStartingPoint());
                hangerList.Add(currentHanger.gameObject);

                //Instantiate the cloth in the attach point associated to the current hanger
                currHangerAttPoint = GetHangerAttachPoint(currentHanger);
                Instantiate(cloth, currHangerAttPoint);

                //Set the cloth to the specific hanger
                currentHanger.GetComponent<ManageHanger>().SetClothInHanger(cloth.gameObject);

                //Move the next hanger to the right
                newDistance += currentContainer.GetDistanceBetweenObjInContainer();
                //Pass to the next cloth
                numberOfCloth++;

                //Check if it's not the last one cloth
                if (cloth != clothList.Last())
                {
                    //Check if another cloth fits
                    CheckIfThereIsSpace();
                }
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
                    "\n ----- typeOfCloth (0_tshirt, 1_trousers, 2_shoes, 3_caps, 4_glasses, 5_watches) -> " +
                    typeOfClothIndex +
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
    public void ActiveHangerHint(string clothNames)
    {
        List<GameObject> hangersToActivate = new(); //Hangers with an hint to activate

        //Takes all hangers with a hint to activate
        foreach (string clothName in clothNames.Split(","))
        {
            Debug.Log(clothName);

            //Hanger of the clothes to recommend
            hangersToActivate.Add(GetHangerFromClothName(clothName +"(Clone)"));
        }


        //Activate only hanger hint of active clothes
        foreach (var hanger in hangerList)
        {
            if (hangersToActivate.Contains(hanger))
            {
                //Activate the hanger hint of active clothes
                hanger.GetComponent<ManageHanger>().ActivateHint();
            }
            else
            {
                //Deactivate other hints
                hanger.GetComponent<ManageHanger>().DeactivateHint();
            }
        }
    }

    //Deactivate all hints
    private void DeactivateAllHangers()
    {
        foreach (var hanger in hangerList)
        {
            hanger.GetComponent<ManageHanger>().DeactivateHint();
        }
    }

    private string RemoveCloneString(string clothName)
    {
        string stringToRemove = "(Clone)";
        int stringToRemoveLenght = stringToRemove.Length;

        return clothName.Substring(0, clothName.Length - stringToRemoveLenght);
    }

    //------------ GET ---------------

    //Return the Hanger associated to the clothName passed
    public GameObject GetHangerFromClothName(string clothName)
    {
        clothName = RemoveCloneString(clothName);

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

    //Return all the caps of the closet
    public GameObject[] GetCapsGameObjects()
    {
        GameObject[] newList = new GameObject[capsList.Length];

        for (int i = 0; i < capsList.Length; i++)
        {
            newList[i] = capsList[i].gameObject;
        }

        return newList;
    }

    //Return all the glasses of the closet
    public GameObject[] GetGlassesGameObjects()
    {
        GameObject[] newList = new GameObject[glassesList.Length];

        for (int i = 0; i < glassesList.Length; i++)
        {
            newList[i] = glassesList[i].gameObject;
        }

        return newList;
    }

    //Return all the watches of the closet
    public GameObject[] GetWatchesGameObjects()
    {
        GameObject[] newList = new GameObject[watchesList.Length];

        for (int i = 0; i < watchesList.Length; i++)
        {
            newList[i] = watchesList[i].gameObject;
        }

        return newList;
    }

    public List<Transform[]> GetClothList()
    {
        return clothLists;
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