using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ManageRecommendedMenu : MonoBehaviour
{
    [SerializeField] private Transform clothesContainer; //RecommendedCards container

    //Information menu where the names of users who have recommended a particular outfit are displayed
    [SerializeField] private GameObject recommendedNamesMenu;

    //Contains all the names of users who have recommended a particular outfit
    [SerializeField] private TextMeshProUGUI recommendedNamesString;

    //string --> OutfitNames List<Tuple<ulong, string>> ---> List of users who recommended that outfit
    private Dictionary<string, List<Tuple<ulong, string>>> recommendedClothesBy = new();

    //Used to memorize which outfit the last open information menu belongs to among the recommended ones.
    private string auxShowedRecommendedNamesOutfit = "";

    //Spawner
    private Spawner spawnerComponent;

    private int numberOfUniqueRecommendNames = 0;

    void Start()
    {
        spawnerComponent = GameObject.Find("Spawner").GetComponent<Spawner>();
    }

    //Associate a user with a particular outfit they recommend
    public Tuple<bool, bool> AddUserToRecommendedCloth(string outfit, Tuple<ulong, string> user)
    {
        /* Flag that says whether a new card needs to be created. It will be true if this particular outfit has never been recommended before, false otherwise.*/
        bool createCard = false;
        /*Flag that tells if there is a need to update the card of that particular outfit.
        * It will be false if a user recommends the same outfit multiple times. It will be true if a
        * new card is being created or if a new user recommends a particular outfit.*/
        bool updateCard = false;

        //If it is the first recommended outfit of that type it is added to the list of outfits and the list of associated students is created
        if (!recommendedClothesBy.ContainsKey(outfit))
        {
            recommendedClothesBy.Add(outfit, new List<Tuple<ulong, string>>());
            createCard = true;
            updateCard = true;
        }


        //Add user to the specific outfit. If the same user has recommended the same outfit multiple times, it is NOT added to the dictionary
        if (!recommendedClothesBy[outfit].Contains(user))
        {
            recommendedClothesBy[outfit].Add(user);
            updateCard = true;
        }

        //This is important for when the last menu is still open.
        if (auxShowedRecommendedNamesOutfit == outfit)
        {
            UpdateRecommendOutfitNames(outfit);
        }

        //Return the created flags usefull for the card update.
        return new Tuple<bool, bool>(createCard, updateCard);
    }

    //Shows the info menu for the specific outfit.
    public void ShowOutfitRecommendedNames(string outfitClothes)
    {
        recommendedNamesMenu.SetActive(true); //Activate it
        auxShowedRecommendedNamesOutfit = outfitClothes; //Save the last one opened
        UpdateRecommendOutfitNames(outfitClothes); //Update the names in this menu
    }

    /* Only the last open info menu is updated. You don't care about updating them all because they will be
     * updated automatically when you open them.*/
    public void UpdateRecommendOutfitNames(string outfitClothes)
    {
        string auxUserName; //Name of the user in the foreach
        string users = ""; //Names of all the users that recommend the specific outfit
        int indexFor = 0; //Index for numbered list

        //Cycle the list of users associated to one specific recommended outfit
        foreach (var listOfTuples in recommendedClothesBy[outfitClothes])
        {
            auxUserName = listOfTuples.Item2; //Name of the user
            users += $"{indexFor + 1}. {auxUserName}\n"; //Create like: "1. Luca"

            indexFor++;
        }

        //Resizes the menu based on how many users have recommended that particular outfit.
        recommendedNamesString.rectTransform.sizeDelta = new Vector2(1200, 70 * (indexFor));
        recommendedNamesString.text = users;
    }

    //Usefull if u want to print the recommended clothes in the log
    private void PrintRecommendedClothesBy()
    {
        string stringToPrint = "";
        foreach (var recommendedCloth in recommendedClothesBy)
        {
            stringToPrint += $"Outfit: {recommendedCloth.Key} ---- Users: ";
            foreach (Tuple<ulong, string> u in recommendedCloth.Value)
            {
                stringToPrint += u + ", ";
            }

            stringToPrint += "\n";
        }

        Debug.Log(stringToPrint);
    }

    //Remove an outfit from the recommended menu
    public void RemoveOutfit(string key)
    {
        recommendedClothesBy.Remove(key);
    }

    /* Find the card associated with this particular outfit (outfitClothes). For each card in the container takes the one
     * with the associated outfit equal to the outfit we are checking (outfitClothes).*/
    public ManageRecommendCard FindRecommendCardFromOutfit(string outfitClothes)
    {
        ManageRecommendCard auxManageRecommendCard;

        foreach (Transform recommendCard in clothesContainer)
        {
            auxManageRecommendCard = recommendCard.GetComponent<ManageRecommendCard>();

            if (auxManageRecommendCard.GetOutfitClothesInString() == outfitClothes)
                return auxManageRecommendCard;
        }

        return null; //Impossible
    }

    //Update the recommendation percentage of all recommended cards 
    public void UpdateEveryPercentage()
    {
        foreach (string outfit in recommendedClothesBy.Keys.ToArray())
        {
            float percentage = CalculatePercentage(outfit);
            FindRecommendCardFromOutfit(outfit).GetComponent<ManageRecommendCard>().ChangePercentage(percentage);
        }
    }

    //Calculates the recommendation percentage based on the ratio of connected users to users who have recommended this particular outfit
    private float CalculatePercentage(string outfit)
    {
        numberOfUniqueRecommendNames = GetNumberOfUniqueRecommendNames();

        //Users who have recommended this particular outfit
        float numUsersForOutfit = GetUsersNumberRecommendOutfit(outfit);

        float percentage = numUsersForOutfit / numberOfUniqueRecommendNames; //Ratio between them

        return percentage; //Return the percentage
    }

    // -------------------------------- GET --------------------------------------

    //Returns the number of users who have recommended that particular outfit
    public int GetUsersNumberRecommendOutfit(string outfitKey)
    {
        return recommendedClothesBy[outfitKey].Count;
    }

    //Return the names of all recommended outfits 
    public string[] GetAllOutfits()
    {
        return recommendedClothesBy.Keys.ToArray();
    }

    public Transform GetClothesContainer()
    {
        return clothesContainer;
    }

    public int GetNumberOfUniqueRecommendNames()
    {
        HashSet<string> uniqueNames = new();
        foreach (List<Tuple<ulong, string>> listOfUser in recommendedClothesBy.Values)
        {
            foreach (Tuple<ulong, string> user in listOfUser)
            {
                uniqueNames.Add(user.Item2);
            }
        }

        return uniqueNames.Count;
    }
}