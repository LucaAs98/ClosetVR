using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeScrollContentMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] itemCollectionList;
    [SerializeField] private ScrollRect itemsScrollRect;
    private string buttonClickedName;

    public void ViewClothes()
    {
        //We take the name of the button clicked in the UI
        buttonClickedName = EventSystem.current.currentSelectedGameObject.name;

        foreach (var itemCollection in itemCollectionList)
        {
            //We compare the name of the button with the the name of the specific itemCollection
            if (itemCollection.name == "Items" + buttonClickedName)
            {
                //If it's the same we activate the itemCollaction and we set it as scroll content
                itemCollection.SetActive(true);
                itemsScrollRect.content = itemCollection.GetComponent<RectTransform>();
            }
            else
            {
                //Otherwise we deactivate it
                itemCollection.SetActive(false);
            }
        }
    }
}