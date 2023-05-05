using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHint : MonoBehaviour
{
    [SerializeField] private float velocity = 2f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime * velocity);
    }
}