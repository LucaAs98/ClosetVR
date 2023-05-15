using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDrawers : MonoBehaviour
{
    public void OpenAll()
    {
        foreach (Transform child in transform)
        {
            float orientation = 1f;

            if (child.localPosition.z < 0)
            {
                orientation = -1f;
            }

            child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y,
                child.localPosition.z - orientation);
        }
    }
}