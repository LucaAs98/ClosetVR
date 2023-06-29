using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    [SerializeField] public List<GameObject> listClientPrefabs;
    private string auxPlayerName = "<<UNKNOWN>>";

    private Dictionary<ulong, string> connectedClients = new();

    private enum Devices
    {
        Android,
        Hololens,
        VR
    }


    [ServerRpc(RequireOwnership = false)]
    public void JoinServerRpc(ulong clientId, int platform, string clientName)
    {
        connectedClients.Add(clientId, clientName);

        //Init the avatar clothes in the client who has just connected
        InitAvatarClothesForSpecificClient(clientId);

        //Set the prefab for the connected client
        SetPrefabForClient(clientId, platform, clientName);


        //Every time one client connects, update the recommended menu
        GameObject.FindGameObjectWithTag("RecommendedMenu").GetComponent<ManageRecommendedMenu>()
            .UpdateEveryPercentage();
    }

    //Set the prefab for the connected client
    private void SetPrefabForClient(ulong clientId, int platform, string clientName)
    {
        GameObject tempGO = Instantiate(listClientPrefabs[platform]);
        tempGO.GetComponent<ClientHandler>().SetClientName(clientName);
        NetworkObject netObj = tempGO.GetComponent<NetworkObject>();
        netObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }


    //Spawn prefab as player. We need it to spawn different prefabs depending on the platform where we are running the application
    public override void OnNetworkSpawn()
    {
        if (IsServer) return;

        ulong clientId = NetworkManager.Singleton.LocalClientId;

        if (Application.platform == RuntimePlatform.Android)
        {
            //Android client spawn
            JoinServerRpc(clientId, (int)Devices.Android, auxPlayerName);
        }
    }


    //-------------- Init variables ---------


    //Retrieves the clothes activated in the server avatar and puts them to the avatar of the client who has just connected
    private void InitAvatarClothesForSpecificClient(ulong clientID)
    {
        Debug.Log("Init Avatar Clothes For SpecificClient!");

        //Retrieves the clothes activated in the server avatar
        List<string> activeClothes = GetManageChangeCloth().GetActiveClothes();
        string activeClothesInString = FromListToString(activeClothes);
        Debug.Log($"activeClothesInString: {activeClothesInString}");

        //Execute the code only in the connected client. Puts the clothes to the avatar of the client who has just connected
        InitClothesInAvatarClientRpc(activeClothesInString, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientID },
            }
        });
    }

    //Init the clothes in the avatar when he connects to the room
    [ClientRpc]
    private void InitClothesInAvatarClientRpc(string clothesToActivate, ClientRpcParams clientRpcParams = default)
    {
        Debug.Log($"Init Clothes In Avatar ClientRpc! clothesToActivate: {clientRpcParams}");
        GetManageChangeCloth().ChangeClothBase(clothesToActivate, null);
    }

    //Transform a list of strings in a single string with the values separated by ","
    private string FromListToString(List<string> clothes)
    {
        string clothesInString = "";
        foreach (string cloth in clothes)
        {
            clothesInString += cloth + ",";
        }

        if (clothesInString.Length > 0)
            clothesInString = clothesInString.Substring(0, clothesInString.Length - 1);

        return clothesInString;
    }


    //---------------- GET ------------------
    private ManageChangeCloth GetManageChangeCloth()
    {
        //Take the avatar
        GameObject avatar = GameObject.FindGameObjectWithTag("Avatar");

        //Return the component
        return avatar.transform.parent.GetComponent<ManageChangeCloth>();
    }

    //-------------- GET --------------------
    public void SetClientName(string name)
    {
        auxPlayerName = name;
    }

    public string GetClientName()
    {
        return auxPlayerName;
    }

    //Return the connected clients
    public Dictionary<ulong, string> GetConnectedClients()
    {
        return connectedClients;
    }

    //Return the number of connected clients
    public int GetNumberOfConnectedClients()
    {
        return connectedClients.Count;
    }
}