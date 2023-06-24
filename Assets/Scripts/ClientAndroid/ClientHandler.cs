using TMPro;
using Unity.Netcode;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ClientHandler : NetworkBehaviour
{
    private string clientName; //Client name
    private ulong clientID; //Client ID
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
            SetClientNameAndIDInClient();
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
    private void SetClientNameAndIDInClient()
    {
        Spawner spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        string name = spawner.GetClientName();
        ulong id = this.OwnerClientId;
        SetClientName(name);
        SetClientID(id);
    }

    //------------------ GET and SET -------------------------
    //Return the player name
    public string GetClientName()
    {
        return clientName;
    }

    //Set the player name 
    public void SetClientName(string name)
    {
        clientName = name;
    }

    public ulong GetClientID()
    {
        return clientID;
    }

    //Set the client name 
    public void SetClientID(ulong id)
    {
        clientID = id;
    }
}