using Info;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Manager
{
    class SceneManager : MonoSingleton<SceneManager>
    {
        public static string currScene = null;
        static string toScene = null;

        Action<AsyncOperation> loadCallback;

        public void LoadScene(string sceneName, Action<AsyncOperation> callback = null)
        {
            //toScene = sceneName;
            //loadCallback = callback;
            //StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            DebugTool.WarningFormat("准备传送到地图 [" + toScene + "]");
            BeforeLoadScene();
            yield return null;

            AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(toScene);
            async.allowSceneActivation = true;
            async.completed += LoadSceneCB;
        }

        private void BeforeLoadScene()
        {
            
        }

        private void AfterLoadScene()
        {

        }

        private void LoadSceneCB(AsyncOperation _)
        {
            //if (Main.MainPlayerCtrl != null)
            //{
            //    GameObject MainPlayer = Main.MainPlayerCtrl.gameObject;
            //    MainPlayer.transform.SetParent(Main.SceneRoot);
            //    Main.MainPlayerCtrl.transform.position = Vector3.zero;
            //    Main.MainPlayerCtrl.InitHUD();
            //}
            if (loadCallback != null)
            {
                loadCallback.Invoke(_);
                loadCallback = null;
            }
            currScene = toScene;
            //GameSaveManager.SaveMapID();
            toScene = null;
            AfterLoadScene();
        }
    }
}
