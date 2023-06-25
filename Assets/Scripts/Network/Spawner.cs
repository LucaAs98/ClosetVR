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
        foreach (var client in connectedClients.Values)
        {
            Debug.Log($"Spawner: {client}");
        }

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