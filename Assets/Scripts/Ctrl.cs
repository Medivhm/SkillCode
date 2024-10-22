using Constant;
using QEntity;
using Manager;
using System;
using UnityEngine;
using Tools;

public class Ctrl : Singleton<Ctrl>
{
    public static bool DropItem(ItemType itemType, int id, Vector3 pos)
    {
        return DropItem(CreateItem(itemType, id), pos);
    }

    public static bool DropItem(Item item, Vector3 pos)
    {
        if (item.IsNull() || item.Num < 1) return false;

        GameObject go = LoadTool.LoadItem(item.PrefabPath);
        go.transform.position = pos;
        TimerManager.Add(2f, () =>
        {
            go.MoveToUnit(item.Owner.transform, () =>
            {
                MakeItemToBag(item);
            });
        });

        return true;
    }

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
        QuickKeyManager.SetUIInputIgnore(state);
    }

    public static Item CreateItem(ItemType itemType, int id)
    {
        if (ItemType.Prop.Equals(itemType))
        {
            return CreateProp(id);
        }
        else if (ItemType.Weapon.Equals(itemType))
        {
            return CreateWeapon(id);
        }
        return null;
    }

    public static Weapon CreateWeapon(int weaponID)
    {
        return ItemManager.CreateWeapon(weaponID);
    }

    public static Prop CreateProp(int propID)
    {
        return ItemManager.CreateProp(propID);
    }

    public static Item MakeItemToBag(ItemType itemType,int id)
    {
        Item item = null;
        if (ItemType.Prop.Equals(itemType))
        {
            item = CreateProp(id);
        }
        else if (ItemType.Weapon.Equals(itemType))
        {
            item = CreateWeapon(id);
        }
        if (item.IsNotNull())
        {
            MakeItemToBag(item);
        }
        return item;
    }

    public static Item MakeItemToBag(Item item)
    {
        Bag.Instance.AddItem(item);
        GUI.ActionInfoLog(string.Format("获得 [{0}] {1}个", item.Name, item.Num));
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

    public static void AddKeyPress(KeyCode key, Action action)
    {
        QuickKeyManager.AddPress(key, action);
    }

    public static void AddKeyUp(KeyCode key, Action action)
    {
        QuickKeyManager.AddUp(key, action);
    }

    public static void RemoveKeyPress(KeyCode key, Action action)
    {
        QuickKeyManager.RemovePress(key, action);
    }

    public static void RemoveKeyUp(KeyCode key, Action action)
    {
        QuickKeyManager.RemoveUp(key, action);
    }
}