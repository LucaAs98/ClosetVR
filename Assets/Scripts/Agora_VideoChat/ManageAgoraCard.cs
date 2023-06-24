using TMPro;
using UnityEngine;

public class ManageAgoraCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clientName;

    public void SetClientName(string newName)
    {
        clientName.text = newName;
    }
}