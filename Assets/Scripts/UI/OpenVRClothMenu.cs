using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenVRClothMenu : MonoBehaviour
{
    [SerializeField] private float startingXPosition;
    [SerializeField] private float finalXPosition;
    [Range(0, 3)] [SerializeField] private float speed = 0.01f;
    private float lerpDuration = 1f;
    float positionLerp;

    public void OpenMenu(RectTransform menuToOpen)
    {
        if (menuToOpen.gameObject.activeSelf)
            StartCoroutine(MoveMenu(menuToOpen, finalXPosition, startingXPosition, false));
        else
            StartCoroutine(MoveMenu(menuToOpen, startingXPosition, finalXPosition, true));
    }

    IEnumerator MoveMenu(RectTransform menuToOpen, float startValue, float endValue, bool enabled)
    {
        float timeElapsed = 0;

        if (enabled)
        {
            menuToOpen.gameObject.SetActive(true);
        }

        while (timeElapsed < lerpDuration)
        {
            positionLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            menuToOpen.anchoredPosition =
                new Vector3(positionLerp, menuToOpen.localPosition.y, menuToOpen.localPosition.z);
            timeElapsed += Time.deltaTime * speed;
            yield return null;
        }

        menuToOpen.anchoredPosition =
            new Vector3(endValue, menuToOpen.localPosition.y, menuToOpen.localPosition.z);

        if (!enabled)
        {
            menuToOpen.gameObject.SetActive(false);
        }
    }
}