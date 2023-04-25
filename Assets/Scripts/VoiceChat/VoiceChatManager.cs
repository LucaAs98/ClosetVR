using System;
using TMPro;
using Unity.Netcode;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.UI;
using VivoxUnity;

public class VoiceChatManager : NetworkBehaviour
{
    [SerializeField] private GameObject activateDeactivateBtn;
    private bool enableVoiceChat;
    public ILoginSession LoginSession;


    public void JoinChannel(string channelName, ChannelType channelType, bool connectAudio, bool connectText,
        bool transmissionSwitch = true, Channel3DProperties properties = null)
    {
        if (LoginSession.State == LoginState.LoggedIn)
        {
            Channel channel = new Channel(channelName, channelType, properties);

            IChannelSession channelSession = LoginSession.GetChannelSession(channel);

            channelSession.BeginConnect(connectAudio, connectText, transmissionSwitch, channelSession.GetConnectToken(),
                ar =>
                {
                    try
                    {
                        channelSession.EndConnect(ar);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Could not connect to channel: {e.Message}");
                        return;
                    }
                });
        }
        else
        {
            Debug.LogError("Can't join a channel when not logged in.");
        }
    }

    public void Login(string displayName = null)
    {
        var account = new Account(displayName);
        bool connectAudio = true;
        bool connectText = false;

        LoginSession = VivoxService.Instance.Client.GetLoginSession(account);
        LoginSession.PropertyChanged += LoginSession_PropertyChanged;

        LoginSession.BeginLogin(LoginSession.GetLoginToken(), SubscriptionMode.Accept, null, null, null, ar =>
        {
            try
            {
                LoginSession.EndLogin(ar);
            }
            catch (Exception e)
            {
                // Unbind any login session-related events you might be subscribed to.
                // Handle error
                return;
            }
            // At this point, we have successfully requested to login. 
            // When you are able to join channels, LoginSession.State will be set to LoginState.LoggedIn.
            // Reference LoginSession_PropertyChanged()
        });
    }

    // We immediately join a channel after LoginState changes to LoginState.LoggedIn.
    private void LoginSession_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var loginSession = (ILoginSession)sender;
        if (e.PropertyName == "State")
        {
            if (loginSession.State == LoginState.LoggedIn)
            {
                bool connectAudio = true;
                bool connectText = true;

                JoinChannel("MultipleUserTestChannel", ChannelType.NonPositional, connectAudio, connectText);

                //If we are in the server we have to activate the btn to enable/disable voice chat
                if (IsServer)
                    activateDeactivateBtn.gameObject.SetActive(true);

                //When someone connects we initialize the voice chat
                EnableVoiceChat();
            }
        }
    }

    //Useful for enable/disable the voice chat
    public void EnableVoiceChat(bool onClickBtn = false)
    {
        //if we click the btn we want to change also the flag, otherwise we just want to initialize
        if (onClickBtn)
        {
            enableVoiceChat = !enableVoiceChat;
        }

        if (enableVoiceChat)
        {
            activateDeactivateBtn.GetComponent<Image>().color = Color.red;
            activateDeactivateBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Disable Voice Chat";
            //We call the function for the server and for the clients
            MuteUserBase(true);
            MuteUserClientRpc(true);
        }
        else
        {
            activateDeactivateBtn.GetComponent<Image>().color = Color.green;
            activateDeactivateBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Enable Voice Chat";
            //We call the function for the server and for the clients
            MuteUserBase(false);
            MuteUserClientRpc(false);
        }
    }

    //Base function called from server but also from clients
    private void MuteUserBase(bool mute)
    {
        if (mute)
        {
            //We enable the chat at all clients and server
            LoginSession.SetTransmissionMode(TransmissionMode.All);
        }
        else
        {
            //We disable the chat at all clients and server
            LoginSession.SetTransmissionMode(TransmissionMode.None);
        }
    }

    [ClientRpc]
    private void MuteUserClientRpc(bool mute)
    {
        MuteUserBase(mute);
    }
}