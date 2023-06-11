using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateTeleportationRay : MonoBehaviour
{
    [SerializeField] private XRInteractorLineVisual leftTeleportation;
    [SerializeField] private XRInteractorLineVisual rightTeleportation;

    [SerializeField] private InputActionProperty leftActivate;
    [SerializeField] private InputActionProperty rightActivate;

    void Update()
    {
        leftTeleportation.enabled = leftActivate.action.ReadValue<float>() > 0.1;
        rightTeleportation.enabled = rightActivate.action.ReadValue<float>() > 0.1;
    }
}