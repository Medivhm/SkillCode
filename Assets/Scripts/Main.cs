using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Main : Singleton<Main>
{
    // 大更新.功能模块.扩展功能模块.小更新
    public static string Version = "0.1.0.2";
    public static string ConnectTips = null;

    public static string UserName;
    public static string SteamName;
    public static int MapLength = 300;
    public static float mouseSensitivity = 30f;

    public static bool IsServer = false;
    public static bool IsClient
    {
        get { return !IsServer; }
    }
    public static bool AlwaysGrounds = false;

    public static PlayerController MainPlayerCtrl;
    public static CameraController MainCameraCtrl;
    public static GM GM;
    public static ActionInfoUI ActionInfoUI;
    public static ChatUI ChatUI;
    public static LocateUI LocateUI;
    public static MagicProgressbar MagicProgressbar;
    public static HotBar HotBar;
    public static SkyController SkyController;

    public static Canvas MainCanvas;
    public static Camera MainCamera;
    public static Camera UICamera;
    public static EventSystem EventSystem;
    public static AudioSource BackgroundAudio;

    public static Transform SceneRoot;
    public static Transform DontDestroyTemp;

    static bool playBackgroundMusic = false;
    public static bool PlayBackgroundMusic
    {
        get => playBackgroundMusic;
        set
        {
            if (value == playBackgroundMusic) return;

            playBackgroundMusic = value;
            if (Main.BackgroundAudio.IsNotNull())
            {
                if (value)
                {
                    Main.BackgroundAudio.Play();
                }
                else
                {
                    Main.BackgroundAudio.Stop();
                }
            }
        }
    }


    public static float CanvasScaleFactor
    {
        get
        {
            if (MainCanvas)
            {
                return MainCanvas.scaleFactor;
            }
            else
            {
                return 1f;
            }
        }
    }

    public static GraphicRaycaster GraphicRaycaster
    {
        get
        {
            if (MainCanvas.IsNull())
            {
                MainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
            }
            return MainCanvas.GetComponent<GraphicRaycaster>();
        }
    }
}
