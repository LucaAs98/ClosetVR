using System.Collections.Generic;
using UnityEngine;

public class ClothesWithSkeletonManager : MonoBehaviour
{
    [Header("Armature Type")] [SerializeField]
    private ArmatureType armatureType;

    [Header("Container of all categories")] [SerializeField]
    private Transform clothesContainer;

    [Header("Clothes categories")] [SerializeField]
    private Transform upperBodyContainer;

    [SerializeField] private Transform lowerBodyContainer;
    [SerializeField] private Transform shoesContainer;
    [SerializeField] private Transform capsContainer;
    [SerializeField] private Transform glassesContainer;
    [SerializeField] private Transform watchesContainer;

    [Header("Body parts")] [SerializeField]
    private Transform leftLeg;

    [SerializeField] private Transform rightLeg;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArm;


    [Header("Skin")] [SerializeField] private GameObject skin;


    private Vector3 initialLeftLegPosition = new Vector3(1.60594936e-06f, 42.789856f, -1.25262886e-07f);
    private Vector3 initialRightLegPosition = new Vector3(9.38307494e-08f, 42.7759018f, -4.80213203e-07f);
    private Vector3 rightLegPositionForShoes = new Vector3(9.31999969f, 41.9599991f, 0);
    private Vector3 leftLegPositionForShoes = new Vector3(-8.26000023f, 42.0499992f, 0);

    private Vector3 rightArmRotationForHanger = new Vector3(55.7211113f, 355.36557f, 348.13974f);
    private Vector3 leftArmRotationForHanger = new Vector3(56.9790916f, 4.55117226f, 11.7212267f);

    private List<Transform> listOfCategories;

    public enum ArmatureType
    {
        GeneralCard,
        Avatar,
        Hanger,
    }

    void Awake()
    {
        listOfCategories = new List<Transform>()
        {
            upperBodyContainer, lowerBodyContainer, shoesContainer, capsContainer, glassesContainer, watchesContainer
        };

        Debug.Log("skeleton: " + armatureType);
        InitForArmatureType();
    }

    private void InitForArmatureType()
    {
        switch (armatureType)
        {
            case ArmatureType.GeneralCard:
                SetArmsForHangerOrCard();
                break;
            case ArmatureType.Hanger:
                SetArmsForHangerOrCard();
                RemoveSkin();
                break;
            case ArmatureType.Avatar:
                break;
        }
    }

    private void RemoveSkin()
    {
        skin.SetActive(false);
    }

    public void SetLegsForShoes(bool areShoes)
    {
        if (areShoes)
        {
            leftLeg.localPosition = leftLegPositionForShoes;
            rightLeg.localPosition = rightLegPositionForShoes;
        }
        else
        {
            Debug.Log($"initialLeftLegPosition: {initialLeftLegPosition}");
            Debug.Log($"initialLeftLegPosition: {initialRightLegPosition}");
            leftLeg.localPosition = initialLeftLegPosition;
            rightLeg.localPosition = initialRightLegPosition;
        }
    }

    public void SetArmsForHangerOrCard()
    {
        rightArm.localRotation = Quaternion.Euler(rightArmRotationForHanger);
        leftArm.localRotation = Quaternion.Euler(leftArmRotationForHanger);
    }

    public void SetArmatureType(ArmatureType type)
    {
        armatureType = type;
        InitForArmatureType();
    }

    public Transform GetClothesContainer()
    {
        return clothesContainer;
    }

    //Returns all the categories transform of the armature
    public List<Transform> GetAllCategoryContainers()
    {
        List<Transform> listOfCategories = new();

        foreach (Transform categoryContainer in clothesContainer)
        {
            listOfCategories.Add(categoryContainer);
        }

        return listOfCategories;
    }

    //Returns all cloth names of the armature
    public Dictionary<string, string> GetAllClothNamesWithCategory()
    {
        Dictionary<string, string> allClothNamesWithCategory = new();

        foreach (Transform category in listOfCategories)
        {
            foreach (Transform cloth in category)
            {
                allClothNamesWithCategory.Add(cloth.name, category.name);
            }
        }

        return allClothNamesWithCategory;
    }

    public string GetArmatureType()
    {
        return armatureType.ToString();
    }

    public List<Transform> GetClothesOfCategory(string category)
    {
        List<Transform> clothesOfCategory = new();

        foreach (Transform categoryContainer in clothesContainer)
        {
            if (categoryContainer.name == category)
            {
                foreach (Transform cloth in categoryContainer)
                {
                    clothesOfCategory.Add(cloth);
                }

                break;
            }
        }

        return clothesOfCategory;
    }
}