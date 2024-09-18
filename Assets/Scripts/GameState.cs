using Constant;
using Manager;
using Steamworks;
using System;
using UnityEngine;

public enum GameStateEnum
{ 
    None,
    Start,
    Menu,
    Game,
}

public enum NetStateEnum
{
    LAN,
    Steam,
}

public class GameState : MonoSingleton<GameState>
{
    static GameStateEnum State;
    static NetStateEnum NetState;

    public static void ChangeNetState(NetStateEnum netState)
    {
        NetState = netState;
    }

    public static void ChangeState(GameStateEnum state, Action<AsyncOperation> callback = null)
    {
        //if (State == state) return;

        //State = state;
        //// 后state
        //if (GameStateEnum.Start == State)
        //{
        //    // 必然只调用一次
        //    StartGame(callback);
        //}
        //else if (GameStateEnum.Menu == State)
        //{
        //    ToMenu(callback);
        //}
        //else if (GameStateEnum.Game == State)
        //{
        //    ToGame(callback);
        //}
    }

    static void StartGame(Action<AsyncOperation> callback)
    {
        ChangeState(GameStateEnum.Menu, callback);
    }


    static void ToMenu(Action<AsyncOperation> callback)
    {
        if(SceneManager.currScene == SceneName.MenuScene)
        {
            return;
        }

        ClearAll();
        SceneManager.Instance.LoadScene(SceneName.MenuScene, callback);
    }

    static void ClearAll()
    {
        OutGameClear();
        ClearNet();
    }

    static void ToGame(Action<AsyncOperation> callback)
    {
        if (string.IsNullOrEmpty(Main.UserName))
        {
            Main.UserName = Main.SteamName;
        }

        if (SceneManager.currScene == SceneName.GameScene)
        {
            return;
        }

        SceneManager.Instance.LoadScene(SceneName.GameScene, (_)=> 
        { 
            if (callback.IsNotNull())
            {
                callback.Invoke(_);
            }
            InGameSave();





        });
    }

    private static void InGameSave()
    {
        Main.MainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        Main.SceneRoot = GameObject.Find("Root").transform;
        Main.GM = GameObject.Find("GMCanvas").GetComponent<GM>();
    }

    private static void OutGameClear()
    {
        Main.MainCanvas = null;
        Main.MainCamera = null;
        Main.MainCameraCtrl = null;
        Main.ChatUI = null;
        Main.SceneRoot = null;
        Main.GM = null;

        QuickKeyManager.ClearAll();
        HUDManager.ClearAllHUDs();
        HUDManager.ClearComplete();
        UIManager.Instance.Clear();
        ChunkManager.Instance.Clear(); 
    }

    static void ClearNet()
    {
        if (NetStateEnum.Steam == NetState)
        {
        }
    }

    private void OnApplicationQuit()
    {
        SteamAPI.Shutdown();
    }
}
