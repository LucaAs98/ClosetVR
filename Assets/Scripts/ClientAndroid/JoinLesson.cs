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
        //SET NAME AND THEN SPAWN
        // Debug.Log(namePlayer.text);
        // androidClientPrefab.GetComponent<ClientHandler>().SetPlayerName(namePlayer.text);

        GameObject spawner = GameObject.Find("Spawner");
        spawner.GetComponent<Spawner>().SetPlayerName(namePlayer.text);

        bool connectionOK =
            await NetworkManager.Singleton.GetComponent<RelayLogic>().JoinRelay(code.text);

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