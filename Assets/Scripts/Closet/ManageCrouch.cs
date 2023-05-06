using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCrouch : MonoBehaviour
{
    [SerializeField] private GameObject tShirtContainer;
    [SerializeField] private GameObject hint;

    public GameObject GetTShirtContainer()
    {
        return tShirtContainer;
    }

    public GameObject GetHint()
    {
        return hint;
    }

    public string GetTShirtName()
    {
        return tShirtContainer.transform.GetChild(0).name;
    }

    public void ActivateHint()
    {
        hint.SetActive(true);
    }

    public void DeactivateHint()
    {
        hint.SetActive(false);
    }
}