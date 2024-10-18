using Constant;
using QEntity;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Manager
{
    // 只有常驻的UI会放在UIManager里管理
    // 像是itemTips 由模块自行管理
    class UIManager : Singleton<UIManager>
    {
        static Stack<UIEntity> UIStack;
        static Dictionary<string, UIEntity> UIs;
        public static bool HasUI()
        {
            return UIStack.Count > 0;
        }


        public void Init()
        {
            UIStack = new Stack<UIEntity>();
            UIs = new Dictionary<string, UIEntity>();
            InitEscPress();
        }

        void InitEscPress()
        {
            Ctrl.SetQuickKey(KeyCode.Escape, () =>
            {
                if(UIStack.Count == 0)
                {
                    Show(UIConstant.SettingUI);
                }
                else
                {
                    HideTop();
                }
            });
        }

        public static void UIPush(UIEntity ui)
        {
            UIStack.Push(ui);
            IfStackJustOneUseMouse();
        }

        public static UIEntity UIPop()
        {
            UIEntity entity = UIStack.Pop();
            IfStackEmptyUnUseMouse();
            return entity;
        }

        public static void Add(string uiName, UIEntity ui)
        {
            if (!HasUI(uiName))
            {
                UIs.Add(uiName, ui);
            }
        }

        public static void DestroyUI(UIEntity ui)
        {
            string name = ui.name;
            DestroyUI(name);
        }

        public static void DestroyUI(string uiName)
        {
            UIEntity ui = Get(uiName);
            if (ui)
            {
                ui.Destroy();
                UIs.Remove(uiName);
            }
        }

        public static UIEntity Get(string uiName)
        {
            UIEntity ui = null;
            UIs.TryGetValue(uiName, out ui);
            return ui;
        }

        public static UIEntity Show(string uiName)
        {
            UIEntity ui = null;
            UIs.TryGetValue(uiName, out ui);
            if (ui.IsNotNull() && ui.IsShow)
            {
                return ui;
            }

            if (ui.IsNull())
            {
                GameObject uiGo = LoadTool.LoadUI(uiName);
                uiGo.transform.position = new Vector3(uiGo.transform.position.x, uiGo.transform.position.y, Constant.UIConstant.UIZAxis);
                uiGo.transform.SetParent(Main.MainCanvas.transform);
                uiGo.transform.SetAsLastSibling();
                ui = uiGo.GetComponent<UIEntity>();
                ui.Init();
                UIs.Add(uiName, ui);
            }
            ui.RefreshUI();
            ui.Show();
            UIPush(ui);
            return ui;
        }

        public static void HideTop()
        {
            if(UIStack.Count == 0)
            {
                return;
            }

            UIEntity ui = UIPop();
            ui.Hide();
        }

        public static void Hide(string uiName)
        {
            if (!HasUI(uiName)) return;

            UIs[uiName].Hide();
            UIPop();
        }

        static void IfStackEmptyUnUseMouse()
        {
            if(UIStack.Count == 0)
            {
                Ctrl.UnUseMouse();
            }
        }

        static void IfStackJustOneUseMouse()
        {
            if (UIStack.Count == 1)
            {
                Ctrl.UseMouse();
            }
        }

        public static bool HasUI(string uiName)
        {
            if (UIs.ContainsKey(uiName))
            {
                return true;
            }
            return false;
        }

        public void Clear()
        {
            UIStack.Clear();
            UIs.Clear();
        }
    }
}
