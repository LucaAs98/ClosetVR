using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBodyRotation : MonoBehaviour
{

    [SerializeField] Transform headRotation;
    private Quaternion initialRotation;
    public float timeForResetting;


    private void Awake()
    {
        initialRotation = gameObject.transform.rotation;
    }
    void Update()
    {
        timeForResetting += Time.deltaTime;
        ChangeRotation();
    }

    private void ChangeRotation()
    {
        float difference = headRotation.rotation.eulerAngles.y - gameObject.transform.rotation.eulerAngles.y;
        if (difference >= 30 || difference <= -30)
        {
            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, headRotation.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z);
            timeForResetting = 0;
        }
        if(timeForResetting >= 2f)
        {
            gameObject.transform.rotation = initialRotation;
            timeForResetting = 0f;
        }
    }

}
