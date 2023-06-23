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

public class AgoraSetup : MonoBehaviour
{
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };
#endif
    // Fill in your app ID.
    private string _appID = "6e757039e5054cbfb73e7af64a98fbe6";

    // Fill in your channel name.
    private string _channelName = "ClosetVR";

    // Fill in the temporary token you obtained from Agora Console.
    private string _token = "";
    //"007eJxTYFC8er5HI8yg+jrf87Os34/rH7d8FrT+xC+Ps3cOVNjmLWdRYDBLNTc1NzC2TDU1MDVJTkpLMjdONU9MMzNJtLRIS0o1c1w8NaUhkJEhweAmAyMUgvgcDM45+cWpJWFBDAwAUvoifA==";


    private string serverUrl = "https://agora-token-generator.up.railway.app"; // The base URL to your token server."

    private int ExpireTime = 60; //Expire time in Seconds.
    private string uid = "0"; // An integer that identifies the user.


    // A variable to save the remote user uid.
    private List<uint> remoteUids;
    internal VideoSurface LocalView;
    internal IRtcEngine RtcEngine;

    [SerializeField] private GameObject remoteView;
    [SerializeField] private GameObject camerasContainer;

    void Start()
    {
        StartCoroutine(FetchToken(serverUrl, _channelName, uid, ExpireTime, this.FetchRenew));
        SetupVideoSDKEngine();
        InitEventHandler();
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


    private void InitEventHandler()
    {
        // Creates a UserEventHandler instance.
        UserEventHandler handler = new UserEventHandler(this, remoteView, camerasContainer);
        RtcEngine.InitEventHandler(handler);
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

    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private GameObject userCamerasContainer;
        private GameObject remoteView;
        private GameObject currentRemoteView;

        internal UserEventHandler(AgoraSetup videoSample, GameObject remoteViewPrefab, GameObject camerasContainer)
        {
            remoteView = remoteViewPrefab;
            userCamerasContainer = camerasContainer;
        }

        // This callback is triggered when the local user joins the channel.
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            Debug.Log("You joined channel: " + connection.channelId);
        }

        // public override void OnTokenPrivilegeWillExpire(RtcConnection connection, string token)
        // {
        //     // Retrieve a fresh token from the token server.
        //     _videoSample.StartCoroutine(_videoSample.FetchToken(_videoSample.serverUrl, _videoSample._channelName, _videoSample.uid, _videoSample.ExpireTime, _videoSample.FetchRenew));
        //     Debug.Log("Token Expired");
        // }

        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            currentRemoteView = Instantiate(remoteView, userCamerasContainer.transform);
            SetupRemoteView(currentRemoteView);
            currentRemoteView.GetComponent<VideoSurface>()
                .SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
        }

        private void SetupRemoteView(GameObject remoteView)
        {
            remoteView.AddComponent<VideoSurface>();
            remoteView.transform.Rotate(0.0f, 0.0f, 180.0f);
        }

        // This callback is triggered when a remote user leaves the channel or drops offline.
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            //_videoSample.RemoteView.SetEnable(false);
        }
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
        // Join a channel.
        RtcEngine.JoinChannel(_token, _channelName);
    }
}

public class TokenStruct
{
    public string rtcToken;
}