using TMPro;
using Unity.Netcode;
using UnityEngine;

public class StartServer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyCodeText; //LobbyCode we get from Relay

    // Function where we spawn the object that corresponds to the selected card 
    public async void CreateServer()
    {
        //Getting the lobbycode
        string joinCode = await NetworkManager.Singleton.GetComponent<RelayLogic>().CreateRelay();

        //If the connection is ok we spawn the interested model
        if (joinCode != null)
        {
            lobbyCodeText.text = "Codice: " + joinCode;
        }
        else
        {
            lobbyCodeText.text = "Errore nella creazione del relay";
        }
    }
}