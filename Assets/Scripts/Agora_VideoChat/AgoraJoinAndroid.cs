using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif


public class AgoraJoinAndroid : MonoBehaviour
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

    internal IRtcEngine RtcEngine;

    void Start()
    {
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
        // Set the local video view.
        //LocalView.SetForUser(0, "", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA);
        // Start rendering local video.
        //LocalView.SetEnable(true);
        // Join a channel.
        RtcEngine.JoinChannel(_token, _channelName);
    }

    public void Leave()
    {
        // Leaves the channel.
        RtcEngine.LeaveChannel();
        // Disable the video modules.
        RtcEngine.DisableVideo();
        // Stops rendering the remote video.
        //RemoteViews.SetEnable(false);
        // Stops rendering the local video.
        //LocalView.SetEnable(false);
    }
}