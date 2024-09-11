using Constant;
using QEntity;
using Manager;
using System;
using UnityEngine;

public class Ctrl : Singleton<Ctrl>
{
    public static void UseMouse()
    {
        SetMouseRotate(false);
        SetMouseLockstate(CursorLockMode.None);
        SetMouseVisible(true);
    }

    public static void UnUseMouse()
    {
        SetMouseRotate(true);
        SetMouseLockstate(CursorLockMode.Locked);
        SetMouseVisible(false);
    }

    public static void SetMouseRotate(bool state)
    {
        Main.MainCameraCtrl.SetMouseRotate(state);
    }

    // 设置鼠标模式
    // None, Locked: 鼠标锁定在屏幕中央，动不了, Confined: 鼠标限制在窗口内，仅限Linux和Windows
    public static void SetMouseLockstate(CursorLockMode lockMode)
    {
        MainMouseController.Instance.SetCursorLockstate(lockMode);
    }

    public static void SetMouseVisible(bool state)
    {
        MainMouseController.Instance.SetCursorVisible(state);
    }

    public static HUDEntity AddHUD(Transform owner, string name, float maxBlood, float nowBlood)
    {
        return HUDManager.AddHUD(owner, name, maxBlood, nowBlood);
    }

    public static void SetQuickkeyIgnore(bool state)
    {
        QuickKeyManager.SetIgnore(state);
    }
    public static Item CreateItem(int itemID)
    {
        return ItemManager.CreateItem(itemID);
    }

    public static Item MakeItemToBag(int itemID)
    {
        Item item = CreateItem(itemID);
        Bag.Instance.AddItem(item);
        return item;
    }

    public static void HideTopUI()
    {
        UIManager.HideTop();
    }

    public static void OpenBag()
    {
        UIManager.Show(UIConstant.BagUI);
    }

    public static void SetFrameRate(int frameRate)
    {
        Application.targetFrameRate = frameRate;
    }

    public static void SetQuality(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    public static void GMActive()
    {
        if (Main.GM.IsNotNull())
        {
            Main.GM.transform.GetChild(0).gameObject.SetActive(!Main.GM.transform.GetChild(0).gameObject.activeSelf);
            if (Main.GM.transform.GetChild(0).gameObject.activeSelf)
            {
                Ctrl.UseMouse();
            }
            else
            {
                Ctrl.UnUseMouse();
            }
        }
    }

    public static void SetQuickKey(KeyCode key, Action action, bool influenceByIgnore = true)
    {
        QuickKeyManager.Add(key, () => { action.Invoke(); }, influenceByIgnore);
    }

    public static void ClearQuickKey(KeyCode key)
    {
        QuickKeyManager.Remove(key);
    }
}