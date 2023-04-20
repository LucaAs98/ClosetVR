using Unity.Netcode;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.InputSystem.XR;
public class ClientHandler : NetworkBehaviour
{
    private string clientName; //Client's name
    private Camera cameraAR;

    void Start()
    {
        if (!IsOwner && !IsServer)
        {
            this.gameObject.SetActive(false);
        }
        if (!IsOwner)
        {
            cameraAR = Camera.main;
            cameraAR.gameObject.GetComponent<Camera>().enabled = false;
            cameraAR.gameObject.GetComponent<ARPoseDriver>().enabled = false;
            cameraAR.gameObject.GetComponent<ARCameraManager>().enabled = false;
            cameraAR.gameObject.GetComponent<ARCameraBackground>().enabled = false;
            cameraAR.gameObject.GetComponent<ARSessionOrigin>().enabled = false;
            cameraAR.gameObject.GetComponent<AudioListener>().enabled = false;
        } 
        else
        {
            GameObject cameraVR = GameObject.FindGameObjectWithTag("CameraVR");
            cameraVR.GetComponent<Camera>().enabled = false;
            cameraVR.GetComponent<TrackedPoseDriver>().enabled = false;
        }
    }

    public string GetPlayerName()
    {
        return clientName;
    }

    public void SetPlayerName(string name)
    {
        clientName = name;
    }
}