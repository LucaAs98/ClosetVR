using System.Collections.Generic;
using UnityEngine;

public class Outfit : MonoBehaviour
{
    [SerializeField] private Transform clothesTransform; //Container of all clothes divided by categories

    [SerializeField] private bool isSpecificCloth = false;

    private ClothesWithSkeletonManager clothesWithSkeletonManager;


    //Active only the cloth corresponding to clothName in his category, we dont mind about other categories
    public void ActivateChildOfCategory(string clothName, string clothCategory)
    {
        foreach (Transform clothesCategory in clothesTransform)
        {
            if (clothesCategory.name == clothCategory)
            {
                foreach (Transform cloth in clothesCategory)
                {
                    Debug.Log(cloth.name);
                    //If the name is the same we want to activate it, otherwise we deactivate it
                    if (cloth.name.Replace("_root", "") != clothName)
                    {
                        cloth.gameObject.SetActive(false);
                    }
                    else
                    {
                        //Save the activated cloth because at the end there is the deactivation of the skin parts associated at it
                        cloth.gameObject.SetActive(true);
                    }
                }

                //We dont want to check other categories after we found the specific one
                return;
            }
        }
    }

    //Active only the cloth corresponding to clothName but deactivate all other clothes, also from other categories
    public void ActivateOnlyOneChild(string clothName)
    {
        foreach (Transform clothesCategory in clothesTransform)
        {
            foreach (Transform cloth in clothesCategory)
            {
                cloth.gameObject.SetActive(cloth.name.Replace("_root", "") == clothName);
            }
        }
    }

    //Return a List of strings with all the clothes name activated
    public List<string> GetActiveClothes()
    {
        List<string> activeClothes = new();

        foreach (Transform clothesCategory in clothesTransform)
        {
            foreach (Transform cloth in clothesCategory)
            {
                if (cloth.gameObject.activeSelf)
                    activeClothes.Add($"{clothesCategory.name}_{cloth.name}");
            }
        }

        return activeClothes;
    }

    public void ActivateInRecommendCard(string clothNames)
    {
        string[] outfitClothes; //Array of the outfit's clothing names

        //clothNames contains all the cloth names divided by ","
        outfitClothes = clothNames.Split(",");

        //For every clothName, activate it in the mannequin (outfit)
        foreach (string clothName in outfitClothes)
        {
            string category = clothName.Split("_")[0]; //Take the category from the name
            string newClothName = clothName.Replace(category + "_", ""); //Cloth name without category

            //For every general category, activates clothes only if they are present in outfit
            foreach (Transform categories in clothesTransform)
            {
                foreach (Transform cloth in categories)
                {
                    if (cloth.name.Replace("_root", "") == newClothName)
                    {
                        cloth.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void MoveLegs(bool areShoes)
    {
        if (isSpecificCloth)
        {
            clothesWithSkeletonManager = clothesTransform.parent.GetComponent<ClothesWithSkeletonManager>();
            clothesWithSkeletonManager.SetLegsForShoes(areShoes);
        }
    }

    public void MoveForearm(bool isWatch)
    {
        if (isSpecificCloth)
        {
            clothesWithSkeletonManager = clothesTransform.parent.GetComponent<ClothesWithSkeletonManager>();
            clothesWithSkeletonManager.SetForearmForWatches(isWatch);
        }
    }

    public Transform GetClothesTransform()
    {
        return clothesTransform;
    }
}