using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif

using UnityEngine.Networking;
using System;
using TMPro;

public class AgoraJoinAndroid : MonoBehaviour
{
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };
#endif
    // Fill in your app ID.
    private string _appID = "6e757039e5054cbfb73e7af64a98fbe6";

    // Fill in your channel name.
    private string _channelName = "ClosetVR";

    // Fill in the generated token.
    private string _token = "";


    //For generating token
    private string serverUrl = "https://agora-token-generator.up.railway.app"; // The base URL to your token server."
    private int ExpireTime = 60; //Expire time in Seconds.
    private string uid = "0"; // An integer that identifies the user.

    //Main component
    internal IRtcEngine RtcEngine;

    void Start()
    {
        StartCoroutine(FetchToken(serverUrl, _channelName, uid, ExpireTime, this.FetchRenew));
        SetupVideoSDKEngine();
        Join();
    }


    void Update()
    {
        CheckPermissions();
    }

    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }


    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        foreach (string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
            }
        }
#endif
    }

    private void SetupVideoSDKEngine()
    {
        // Create an instance of the video SDK.
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        // Specify the context configuration to initialize the created instance.
        RtcEngineContext context = new RtcEngineContext(_appID, 0,
            CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION,
            AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT, AREA_CODE.AREA_CODE_GLOB, null);
        // Initialize the instance.
        RtcEngine.Initialize(context);
    }

    public void Join()
    {
        // Enable the video module.
        RtcEngine.EnableVideo();
        // Set the user role as broadcaster.
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
    }

    public void Leave()
    {
        // Leaves the channel.
        RtcEngine.LeaveChannel();
        // Disable the video modules.
        RtcEngine.DisableVideo();
    }

    // Fetches the <Vg k="VSDK" /> token
    IEnumerator FetchToken(string url, string channel, string userId, int TimeToLive, Action<string> callback = null)
    {
        UnityWebRequest request = UnityWebRequest.Get(
            string.Format("{0}/rtc/{1}/1/uid/{2}/?expiry={3}", url, channel, userId, TimeToLive)
        );
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            callback(null);
            yield break;
        }

        TokenStruct tokenInfo = JsonUtility.FromJson<TokenStruct>(request.downloadHandler.text);
        callback(tokenInfo.rtcToken);
    }

    void FetchRenew(string newToken)
    {
        // Update RTC Engine with new token, which will not expire so soon
        RtcEngine.RenewToken(newToken);
        _token = newToken;
        Debug.Log(newToken);
        // Join a channel.
        Debug.Log($"JoinToken: {_token}");
        RtcEngine.JoinChannel(_token, _channelName);
    }
}