using Constant;
using Manager;
using System;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public static void OpenBagUI()
    {
        UIManager.Show(UIConstant.BagUI);
    }

    public static QText CreateText(Transform parent, Vector3 localPos, float width, int fontSize)
    {
        GameObject textGo = GameObject.Instantiate(LoadTool.LoadWidgetPrefab(Widgets.Text));
        QText script = textGo.GetComponent<QText>();
        script.SetParent(parent);
        script.SetWidth(width);
        script.SetLocalPos(localPos);
        script.SetFontSize(fontSize);
        script.Init();
        return script;
    }

    public static void ActionInfoLog(string msg)
    {
        if (Main.ActionInfoUI.IsNotNull())
        {
            Main.ActionInfoUI.CreateInfo(msg);
        }
    }

    public static ItemTipsUI CreateItemTips(Transform parent, Vector3 pos, Item item)
    {
        GameObject uiGo = LoadTool.LoadUI(UIConstant.ItemTips);
        uiGo.transform.SetParent(parent);
        ItemTipsUI script = uiGo.GetComponent<ItemTipsUI>();
        script.Init(pos, item);
        return script;
    }

    public static ButtonList CreateButtonList(Transform parent, Vector3 pos)
    {
        GameObject btnListGo = LoadTool.LoadUI(UIConstant.ButtonList);
        btnListGo.transform.SetParent(parent);
        ButtonList script = btnListGo.GetComponent<ButtonList>();
        script.Init(pos);
        return script;
    }

    //    public static void SliderOutTask(string title, string detail, Action okCB, Action cancelCB)
    //    {
    //        TaskDetail ui = (TaskDetail)UIManager.Show(UIConstant.TaskDetail);
    //        ui.SetInfo(title, detail, okCB, cancelCB);
    //        ui.MoveToLeft();
    //    }

    //    public static void SliderInTask()
    //    {
    //        TaskDetail ui = (TaskDetail)UIManager.Get(UIConstant.TaskDetail);
    //        if (ui)
    //        {
    //            ui.MoveToRight();
    //        }
    //    }

    public static TabUI CreateTab(string tabName, Action clickCB = null, Transform parent = null)
    {
        TabUI tab = LoadTool.LoadUI(UIConstant.Tab).GetComponent<TabUI>();
        tab.SetTitle(tabName);
        if (clickCB.IsNotNull())
        {
            tab.SetClickCB(clickCB);
        }
        if (parent)
        {
            tab.transform.SetParent(parent);
        }
        tab.Init();
        return tab;
    }

    public static ItemUI CreateGrid(Transform parent = null)
    {
        ItemUI ui = LoadTool.LoadUI(UIConstant.Item).GetComponent<ItemUI>();
        ui.JustGrid();
        if (parent)
        {
            ui.transform.SetParent(parent);
        }
        ui.Init();
        return ui;
    }
}