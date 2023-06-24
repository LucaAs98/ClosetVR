using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManageRecommendedMenu : MonoBehaviour
{
    [SerializeField] private Transform clothesContainer;

    [SerializeField] private GameObject recommendedNamesMenu;
    [SerializeField] private TextMeshProUGUI recommendedNamesString;

    //string --> OutfitNames List<Tuple<ulong, string>> ---> List of users who recommended that outfit
    private Dictionary<string, List<Tuple<ulong, string>>> recommendedClothesBy = new();
    private string auxShowedRecommendedNamesOutfit = "";

    public Tuple<bool, bool> AddUserToRecommendedCloth(string outfit, Tuple<ulong, string> user)
    {
        bool createCard = false;
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


        if (auxShowedRecommendedNamesOutfit == outfit)
        {
            UpdateRecommendOutfitNames(outfit);
        }

        return new Tuple<bool, bool>(createCard, updateCard);
    }


    public Transform GetClothesContainer()
    {
        return clothesContainer;
    }

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

    public void RemoveOutfit(string key)
    {
        recommendedClothesBy.Remove(key);
    }

    public void ShowOutfitRecommendedNames(string outfitClothes)
    {
        recommendedNamesMenu.SetActive(true);
        auxShowedRecommendedNamesOutfit = outfitClothes;
        UpdateRecommendOutfitNames(outfitClothes);
    }

    public void UpdateRecommendOutfitNames(string outfitClothes)
    {
        string userName;
        string users = "";
        int indexFor = 0;

        foreach (var listOfTuples in recommendedClothesBy[outfitClothes])
        {
            userName = listOfTuples.Item2;
            users += $"{indexFor + 1}. {userName}\n";

            indexFor++;
        }

        recommendedNamesString.rectTransform.sizeDelta = new Vector2(1200, 70 * (indexFor));
        recommendedNamesString.text = users;
    }
}