using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif


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
    private string _token =
        "007eJxTYNALZpV22mD1Y7m4++wHnX8+STIyqu65vUB4TerUYydTHt9RYDBLNTc1NzC2TDU1MDVJTkpLMjdONU9MMzNJtLRIS0o1Y9rXldIQyMhwQtOSmZEBAkF8DgbnnPzi1JKwIAYGAKOcIII=";

    // A variable to save the remote user uid.
    private List<uint> remoteUids;
    internal VideoSurface LocalView;
    internal IRtcEngine RtcEngine;

    [SerializeField] private GameObject remoteView;
    [SerializeField] private GameObject camerasContainer;

    void Start()
    {
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
        // Join a channel.
        RtcEngine.JoinChannel(_token, _channelName);
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
}