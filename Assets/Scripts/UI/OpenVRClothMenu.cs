using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenVRClothMenu : MonoBehaviour
{
    [SerializeField] private float startingXPosition; //Starting x position of the lerp
    [SerializeField] private float finalXPosition; //Ending x position of the lerp
    [Range(0, 3)] [SerializeField] private float speed = 0.01f; //Speed of the "animation"
    private float lerpDuration = 1f; //Fixed value, change only the speed
    float positionLerp; //Current lerp position

    //Open or close the related mirror menu
    public void OpenMenu(RectTransform menuToActivate)
    {
        RectTransform menuToMove = menuToActivate.parent.GetComponent<RectTransform>();

        //If the menu is active it will close it and deactivate. On the other hand, if it is deactivated it will open it and activate
        if (menuToActivate.gameObject.activeSelf)
            StartCoroutine(MoveMenu(menuToMove, menuToActivate, finalXPosition, startingXPosition, false));
        else
            StartCoroutine(MoveMenu(menuToMove, menuToActivate, startingXPosition, finalXPosition, true));
    }

    //Move the menu from the starting to the ending position
    IEnumerator MoveMenu(RectTransform menuToMove, RectTransform menuToActivate, float startValue, float endValue, bool wantToOpen)
    {
        float timeElapsed = 0; //Lerp time

        //If the menu is closed
        if (wantToOpen)
        {
            menuToActivate.gameObject.SetActive(true);
        }

        //Lerp from the starting x position to the ending x position
        while (timeElapsed < lerpDuration)
        {
            positionLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            menuToMove.anchoredPosition =
                new Vector3(positionLerp, menuToMove.localPosition.y, menuToMove.localPosition.z);
            timeElapsed += Time.deltaTime * speed;
            yield return null;
        }

        //Anchored position because is a canvas element
        menuToMove.anchoredPosition =
            new Vector3(endValue, menuToMove.localPosition.y, menuToMove.localPosition.z);

        //If the menu is open
        if (!wantToOpen)
        {
            menuToActivate.gameObject.SetActive(false);
        }
    }
}