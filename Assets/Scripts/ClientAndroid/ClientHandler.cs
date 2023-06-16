using TMPro;
using Unity.Netcode;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ClientHandler : NetworkBehaviour
{
    private string clientName; //Name of the client
    private Vector3 startingPosition; //Starting position of the client

    void Start()
    {
        InitClient(); //Init this client
    }

    //Init this client
    private void InitClient()
    {
        startingPosition = this.gameObject.transform.position; //Set the starting position of the client

        //If it's not the owner deactivate this gameobject
        if (!IsOwner)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            //If it's the owner deactivate VR stuffs that the client doesn't need
            GameObject cameraVR = GameObject.FindGameObjectWithTag("CameraVR");
            if (cameraVR != null)
            {
                cameraVR.GetComponent<Camera>().enabled = false;
                cameraVR.GetComponent<TrackedPoseDriver>().enabled = false;
            }

            //Set the name of the player
            SetPlayerNameInClient();
        }
    }

    //Useful for resetting the client position in the scene
    public void ResetClientPosition()
    {
        this.GetComponent<CharacterController>().enabled = false;
        this.transform.position = startingPosition;
        this.GetComponent<CharacterController>().enabled = true;
    }

    //Set the player name in his gameobject
    private void SetPlayerNameInClient()
    {
        Spawner spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        string name = spawner.GetPlayerName();
        SetPlayerName(name);
    }

    //------------------ GET and SET -------------------------
    //Return the player name
    public string GetPlayerName()
    {
        return clientName;
    }

    //Set the player name 
    public void SetPlayerName(string name)
    {
        clientName = name;
    }
}