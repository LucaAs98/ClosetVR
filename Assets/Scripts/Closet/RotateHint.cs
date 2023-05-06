using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHint : MonoBehaviour
{
    [SerializeField] private float velocity = 2f;
    [SerializeField] private bool upAndDown = true;

    [SerializeField] private float amplitude = 0.001f;
    [SerializeField] private float frequency = 3;

    private Vector3 initPos;


    void Start()
    {
        initPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime * velocity);
        if (upAndDown)
        {
            transform.localPosition = new Vector3(initPos.x, initPos.y + (Mathf.Sin(Time.time * frequency) * amplitude), initPos.z);
        }
    }
}