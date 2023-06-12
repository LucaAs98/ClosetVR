using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenVRClothMenu : MonoBehaviour
{
    [SerializeField] private RectTransform menuToOpen;
    [Range(0, 3)] [SerializeField] private float speed = 0.01f;
    private float lerpDuration = 1f;
    float positionLerp;


    public void OpenMenu()
    {
        StartCoroutine(MoveMenu(0, 950));
    }

    IEnumerator MoveMenu(float startValue, float endValue)
    {
        float timeElapsed = 0;
        
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
    }
}