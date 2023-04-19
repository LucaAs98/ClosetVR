using Unity.Netcode;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.InputSystem.XR;
public class ClientHandler : NetworkBehaviour
{
    private string clientName; //Client's name

    void Start()
    {
        if (!IsOwner && !IsServer)
        {
            this.gameObject.SetActive(false);
        }
        if (!IsOwner)
        {
            this.gameObject.GetComponent<Camera>().enabled = false;
            this.gameObject.GetComponent<ARPoseDriver>().enabled = false;
            this.gameObject.GetComponent<ARCameraManager>().enabled = false;
            this.gameObject.GetComponent<ARCameraBackground>().enabled = false;
            this.gameObject.GetComponent<ARSessionOrigin>().enabled = false;
        } else
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