using TMPro;
using Unity.Netcode;
using UnityEngine;

public class JoinLesson : MonoBehaviour
{
    [SerializeField] private Canvas rootCanvas;
    [SerializeField] private TMP_InputField code;
    [SerializeField] private GameObject errorImage;
    [SerializeField] private TextMeshProUGUI placeholder;
    [SerializeField] private TMP_InputField namePlayer;
    [SerializeField] private GameObject androidClientPrefab;

    async public void Join()
    {
        //We create the client that we spawn
       androidClientPrefab.GetComponent<ClientHandler>().SetPlayerName(namePlayer.text);

        bool connectionOK = await NetworkManager.Singleton.GetComponent<RelayLogic>().JoinRelay(code.text);

        if (connectionOK)
        {
            rootCanvas.gameObject.SetActive(false);
        }
        else
        {
            errorImage.SetActive(true);
            placeholder.text = "Codice errato";
            code.text = "";
        }
    }
}