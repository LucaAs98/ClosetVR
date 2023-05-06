using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    [SerializeField] public List<GameObject> listClientPrefabs;
    private string auxPlayerName = "";

    private enum Devices
    {
        Android,
        Hololens,
        VR
    }


    [ServerRpc(RequireOwnership = false)]
    public void JoinServerRpc(ulong clientId, int platform, string playerName)
    {
        var tempGO = (GameObject)Instantiate(listClientPrefabs[platform]);

        tempGO.GetComponent<ClientHandler>().SetPlayerName(playerName);

        var netObj = tempGO.GetComponent<NetworkObject>();

        netObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }


    //Spawn prefab as player. We need it to spawn different prefabs depending on the platform where we are running the application
    public override void OnNetworkSpawn()
    {
        if (IsServer) return;

        var clientId = NetworkManager.Singleton.LocalClientId;

        if (Application.platform == RuntimePlatform.Android)
            //Android client spawn
            JoinServerRpc(clientId, (int)Devices.Android, auxPlayerName);

        // else if (Application.platform == RuntimePlatform.WSAPlayerARM)
        // {
        //     //Hololens client spawn
        //     spawner.GetComponent<Spawner>().JoinServerRpc(clientId, (int)Devices.Hololens, playerName);
        // }
        // else
        // {
        //     //VR client spawn
        //     spawner.GetComponent<Spawner>().JoinServerRpc(clientId, (int)Devices.VR, playerName);
        // }
    }

    public void SetPlayerName(string name)
    {
        auxPlayerName = name;
    }
}