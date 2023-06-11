using System.Collections.Generic;
using UnityEngine;

public class Outfit : MonoBehaviour
{
    [SerializeField] private Transform clothesTransform; //Container of all clothes divided by categories

    //Active only the cloth corresponding to clothName in his category, we dont mind about other categories
    public void ActivateChildOfCategory(string clothName, string clothCategory)
    {
        foreach (Transform clothesCategory in clothesTransform)
        {
            if (clothesCategory.name == clothCategory)
            {
                foreach (Transform cloth in clothesCategory)
                {
                    //If the name is the same we want to activate it, otherwise we deactivate it
                    cloth.gameObject.SetActive(cloth.name == clothName);
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
                cloth.gameObject.SetActive(cloth.name == clothName);
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
}