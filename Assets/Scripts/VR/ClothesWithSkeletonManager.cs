using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesWithSkeletonManager : MonoBehaviour
{
    [SerializeField] private Transform leftLeg;
    [SerializeField] private Transform rightLeg;
    private Vector3 initialLeftLegPosition = new Vector3(1.60594936e-06f, 42.789856f, -1.25262886e-07f);
    private Vector3 initialRightLegPosition = new Vector3(9.38307494e-08f, 42.7759018f, -4.80213203e-07f);
    Vector3 rightLegPositionForShoes = new Vector3(9.10000038f, 41.7999992f, 4f);
    Vector3 leftLegPositionForShoes = new Vector3(-9.60000038f, 41.9000015f, 0f);


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
}