using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayLogic : MonoBehaviour
{
    [SerializeField] private GameObject startServerBtn;
    [SerializeField] private GameObject startServerBtnInMirror;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task<string> CreateRelay()
    {
        Destroy(startServerBtn.gameObject);
        Destroy(startServerBtnInMirror.gameObject);

        try
        {
            //We take the lesson code
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(5);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Lobby code: " + joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );

            //We start the server returning the lesson code
            NetworkManager.Singleton.StartServer();

            //Return the code for joining the lesson
            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

        return null;
    }

    public async Task<bool> JoinRelay(string joinCode, string clientName)
    {
        try
        {
            Debug.Log("Joining Relay with " + joinCode + " JoinCode lenght: " + joinCode.Trim((char)8203).Length);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode.Trim((char)8203));
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
            );
            NetworkManager.Singleton.StartClient();

            //Return true if the connection is OK
            return true;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

        return false;
    }
}