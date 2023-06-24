using TMPro;
using Unity.Netcode;
using UnityEngine;

public class JoinLesson : MonoBehaviour
{
    [SerializeField] private Canvas rootCanvas;
    [SerializeField] private TMP_InputField code;
    [SerializeField] private TextMeshProUGUI placeholderCode;
    [SerializeField] private GameObject errorImageCode;
    [SerializeField] private GameObject esclamationImageCode;
    [SerializeField] private TMP_InputField namePlayer;
    [SerializeField] private TextMeshProUGUI placeholderName;
    [SerializeField] private GameObject esclamationImageName;

    async public void Join()
    {
        if (namePlayer.text != "" && code.text != "")
        {
            esclamationImageCode.SetActive(false);
            esclamationImageName.SetActive(false);

            GameObject spawner = GameObject.Find("Spawner");
            spawner.GetComponent<Spawner>().SetClientName(namePlayer.text);

            bool connectionOK =
                await NetworkManager.Singleton.GetComponent<RelayLogic>().JoinRelay(code.text, namePlayer.text);

            if (connectionOK)
            {
                rootCanvas.gameObject.SetActive(false);
            }
            else
            {
                errorImageCode.SetActive(true);
                placeholderCode.text = "Codice errato";
                code.text = "";
            }
        }
        else
        {
            errorImageCode.SetActive(false);
            esclamationImageCode.SetActive(true);
            esclamationImageName.SetActive(true);
            namePlayer.text = "";
            code.text = "";
            placeholderCode.text = "Please complete both fields!";
            placeholderName.text = "Please complete both fields!";
        }
    }
}