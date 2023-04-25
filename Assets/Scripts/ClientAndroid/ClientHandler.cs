using Unity.Netcode;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ClientHandler : NetworkBehaviour
{
    [SerializeField] private GameObject joystickCanvas;
    [SerializeField] private GameObject eventSystem;

    private string clientName; //Client's name
    private Camera cameraAR;
    private Vector3 startingPosition;

    //We activate/deactivate objects depending on IsOwner or not
    void Start()
    {
        cameraAR = Camera.main;
        startingPosition = this.gameObject.transform.position;

        if (!IsOwner && !IsServer)
        {
            this.gameObject.SetActive(false);
        }

        if (!IsOwner)
        {
            joystickCanvas.SetActive(false);
            eventSystem.SetActive(false);

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


    //Useful for resetting the client position in the scene
    public void ResetClientPosition()
    {
        this.GetComponent<CharacterController>().enabled = false;
        this.transform.position = startingPosition;
        this.GetComponent<CharacterController>().enabled = true;
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