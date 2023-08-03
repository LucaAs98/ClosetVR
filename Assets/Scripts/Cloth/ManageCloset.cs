using System;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ManageCloset : NetworkBehaviour
{
    [SerializeField] private Transform armatureWithClothes;


    [Header("UpperBody")] [SerializeField] private Transform upperBodySpace;
    [SerializeField] private Transform upperBodyHangerPrefab;


    [Header("UpperBody")] [SerializeField] private Transform lowerBodySpace;
    [SerializeField] private Transform lowerBodyHangerPrefab;

    [Header("Shoes")] [SerializeField] private Transform shoesSpace;
    [SerializeField] private Transform shoesHangerPrefab;

    [Header("Caps")] [SerializeField] private Transform capsSpace;
    [SerializeField] private Transform capsHangerPrefab;

    [Header("Glasses")] [SerializeField] private Transform glassesSpace;
    [SerializeField] private Transform glassesHangerPrefab;

    [Header("Watches")] [SerializeField] private Transform watchesSpace;
    [SerializeField] private Transform watchesHangerPrefab;

    [Header("Recommend menù")] [SerializeField]
    private Transform recommendMenu;

    [SerializeField] private Transform recommendItemPrefab;


    private ManageRecommendedMenu manageRecommendMenu;
    private Transform clothesInRecommendMenu;

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
            { upperBodySpace, lowerBodySpace, shoesSpace, capsSpace, glassesSpace, watchesSpace };

        clothHangers = new Transform[]
        {
            upperBodyHangerPrefab, lowerBodyHangerPrefab, shoesHangerPrefab, capsHangerPrefab, glassesHangerPrefab,
            watchesHangerPrefab
        };

        //Initialization of manageRecommendMenu, clothesInRecommendMenu
        manageRecommendMenu = recommendMenu.GetComponent<ManageRecommendedMenu>();
        clothesInRecommendMenu = manageRecommendMenu.GetClothesContainer();
    }

    //Fill the closet with all the clothes
    public override void OnNetworkSpawn()
    {
        //Fill it only if we are the server
        if (IsServer)
            InitCloset();
    }

    //Init the closet
    private void InitCloset()
    {
        Transform clothesContainer =
            armatureWithClothes.GetComponent<ClothesWithSkeletonManager>().GetClothesContainer();
        Transform[] allCategories = GetFirstLevelChildren(clothesContainer);

        foreach (Transform category in allCategories)
        {
            Debug.Log($"Category: {category}");

            Debug.Log($"index: {typeOfClothIndex}");
            //Take all the containers associated to the specific type of cloth
            clothContainers = GetAllChildren(clothsSpaces[typeOfClothIndex]);

            //Set the type of hanger we have to spawn every time
            hangerToSpawn = clothHangers[typeOfClothIndex];

            //Init all the variables needed before iterate the specific clothes associated at that type of cloth
            InitBeforeNewTypeOfCloth();

            //Iterate every cloth of the list
            AddClothOfSpecificType(category);
        }
    }


    //We iterate every cloth of a specific type
    private void AddClothOfSpecificType(Transform category)
    {
        Transform lastCloth = null;
        int childCount = category.childCount;

        if (childCount > 0)
            lastCloth = category.GetChild(childCount - 1);

        foreach (Transform cloth in category)
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

                InstantiateClothInCloset(cloth, category.name);

                //Set the cloth to the specific hanger
                currentHanger.GetComponent<ManageHanger>().SetClothInHanger(cloth.gameObject);

                //Move the next hanger to the right
                newDistance += currentContainer.GetDistanceBetweenObjInContainer();
                //Pass to the next cloth
                numberOfCloth++;

                //Check if it's not the last one cloth
                if (lastCloth != null && cloth != lastCloth)
                {
                    //Check if another cloth fits
                    CheckIfThereIsSpace();
                }
            }
        }

        typeOfClothIndex++;
    }

    //Instantiate automatically the cloth in the closet
    private void InstantiateClothInCloset(Transform cloth, string category)
    {
        //Take the specific mannequin (!! with all clothes !!)
        Transform newArmature = Instantiate(armatureWithClothes, currHangerAttPoint);

        //Get the specific armature manager
        ClothesWithSkeletonManager specificArmatureManager = newArmature.GetComponent<ClothesWithSkeletonManager>();
        //Set the type of the armature to "Hanger" and check if the cloth are shoes for moving legs
        specificArmatureManager.SetArmatureType(ClothesWithSkeletonManager.ArmatureType.Hanger);
        if (category == "Shoes")
            specificArmatureManager.SetLegsForShoes(true);

        //Gets all clothes of the same category and activate the specific one
        List<Transform> clothesWithSameCategory = specificArmatureManager.GetClothesOfCategory(category);
        foreach (Transform specificCloth in clothesWithSameCategory)
        {
            if (specificCloth.name == cloth.name)
            {
                //Init cloth for the closet
                InitNewClosetCloth(specificCloth, category);
            }
        }
    }

    //Init cloth for the closet, add CheckRayInteraction component and other components related
    private void InitNewClosetCloth(Transform specificCloth, string category)
    {
        specificCloth.gameObject.SetActive(true);
        CheckRayInteraction checkRayInteraction = specificCloth.AddComponent<CheckRayInteraction>();
        checkRayInteraction.SetClothCategory(category);
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
            //Hanger of the clothes to recommend
            hangersToActivate.Add(GetHangerFromClothName(clothName));
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

    //Adds recommended outfit to mirror menu
    public void AddToRecommendMenu(string clothNames, string userName, ulong clientID)
    {
        //Create a user with his ID and name
        Tuple<ulong, string> user = new Tuple<ulong, string>(clientID, userName);

        //Check if there is a need to create a new card or simply update an existing one
        Tuple<bool, bool> createCardUpdate = manageRecommendMenu.AddUserToRecommendedCloth(clothNames, user);
        bool createCard = createCardUpdate.Item1;
        bool updateCard = createCardUpdate.Item2;

        //There is no card with this particular outfit (clothNames), so it creates it
        if (createCard)
            CreateRecommendCard(clothNames, userName);

        //Updates the card data associated with this particular outfit (clothNames)
        if (updateCard && !createCard)
            manageRecommendMenu.FindRecommendCardFromOutfit(clothNames).GetComponent<ManageRecommendCard>()
                .UpdateRecommendCard();

        manageRecommendMenu.UpdateEveryPercentage();
    }

    //Creates the card associated with a specific outfit (clothNames)
    private void CreateRecommendCard(string clothNames, string userName)
    {
        //Instantiate the recommended card in the recommended mirror menu
        Transform recommendItem = Instantiate(recommendItemPrefab, clothesInRecommendMenu);

        //Complete the card with all the informations
        recommendItem.GetComponent<ManageRecommendCard>().ConfigureCard(clothNames, userName);
    }

    //Deactivates all hints
    private void DeactivateAllHangers()
    {
        foreach (var hanger in hangerList)
        {
            hanger.GetComponent<ManageHanger>().DeactivateHint();
        }
    }

    //------------ GET ---------------

    //Returns the Hanger associated to the clothName passed
    public GameObject GetHangerFromClothName(string clothName)
    {
        string toRemove = clothName.Split("_")[0];
        string onlyClothName = clothName.Replace(toRemove + "_", "");
        Debug.Log("GetHangerFromClothName " + onlyClothName);

        foreach (var currentHanger in hangerList)
        {
            if (currentHanger.GetComponent<ManageHanger>().GetClothName() == onlyClothName)
            {
                return currentHanger;
            }
        }

        return null;
    }

    //Returns the attach point associated to the specific hanger
    private Transform GetHangerAttachPoint(Transform hanger)
    {
        return hanger.GetComponent<ManageHanger>().GetClothAttachPoint().transform;
    }

    private GameObject[] CreateArrayOfClothesFromCategory(Transform category)
    {
        GameObject[] newList = new GameObject[category.childCount];
        int i = 0;

        foreach (Transform cloth in category)
        {
            newList[i] = cloth.gameObject;
            i++;
        }

        return newList;
    }

    //Returns all children of a transform
    private Container[] GetAllChildren(Transform transform)
    {
        List<Container> children = new();

        foreach (Transform child in transform)
            children.Add(child.GetComponent<Container>());

        return children.ToArray();
    }

    //Returns children of first level
    private Transform[] GetFirstLevelChildren(Transform parent)
    {
        Transform[] children = new Transform[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
        {
            children[i] = parent.GetChild(i);
        }

        return children;
    }
}