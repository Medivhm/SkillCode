using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.Windows;

namespace Manager
{
    class FunctionManager : Singleton<FunctionManager>
    {
        static Dictionary<int, Action<string>> Functions;

        public void Init()
        {
            Functions = new Dictionary<int, Action<string>>()
            {
                [1]  = JumpMap,
                [2]  = OpenBagUI,
            };
        }

        public static void DoFunction(string funcStr)
        {
            if (funcStr == "") return;

            string[] args = funcStr.Split("#");
            if (args.Length == 0)
            {
                DebugTool.ErrorFormat("FunctionManager:  参数错误 【{0}】", funcStr);
            }
            else if(args.Length == 1)
            {
                Functions[int.Parse(args[0])](null);
            }
            else
            {
                Functions[int.Parse(args[0])](args[1]);
            }
        }

        static void JumpMap(string mapName)
        {
            if (mapName.IsNotNull())
            {
                SceneManager.Instance.LoadScene(mapName, (_) =>
                {
                    
                });
            }
            else
            {
                return;
            }
        }

        static void OpenBagUI(string _)
        {
            GUI.OpenBagUI();
        }
    }
}
