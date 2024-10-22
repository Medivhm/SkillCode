using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;
using UnityEngine.UIElements;

struct PressAction
{
    public bool influenceByUIInput;
    public Action action;
}

class QuickKeyManager : MonoSingleton<QuickKeyManager>
{
    //static Dictionary<KeyCode, PressAction> quickKeys;

    //static List<KeyCode> removeList;
    //static bool uiInputIgnore = false;

    //private void Awake()
    //{
    //    quickKeys = new Dictionary<KeyCode, PressAction>();
    //    removeList = new List<KeyCode>();
    //}

    //private void Update()
    //{
    //    if (removeList.IsNull()) return;

    //    foreach(var key in removeList)
    //    {
    //        quickKeys.Remove(key);
    //    }
    //    removeList.Clear();

    //    foreach(var key in quickKeys.Keys)
    //    {
    //        if (Input.GetKeyDown(key))
    //        {
    //            if (!(uiInputIgnore && quickKeys[key].influenceByUIInput))
    //            {
    //                quickKeys[key].action.Invoke();
    //            }
    //        }
    //    }
    //}

    //public void Init()
    //{
    //}

    //public static void SetUIInputIgnore(bool state)
    //{
    //    uiInputIgnore = state;
    //}

    //public static void Add(KeyCode key, Action ac, bool influenceByUIInput = true)
    //{
    //    PressAction action;
    //    action.action = ac;
    //    action.influenceByUIInput = influenceByUIInput;
    //    quickKeys[key] = action;
    //}

    //public static void Remove(KeyCode key)
    //{
    //    removeList.Add(key);
    //}

    //public static void ClearAll()
    //{
    //    foreach (var key in quickKeys.Keys)
    //    {
    //        Remove(key);
    //    }
    //}


    static Dictionary<KeyCode, Action> keyPress;
    static Dictionary<KeyCode, Action> keyUp;

    static bool uiInputIgnore = false;

    private void Awake()
    {
        keyPress = new Dictionary<KeyCode, Action>();
        keyUp    = new Dictionary<KeyCode, Action>();
    }

    private void Update()
    {
        foreach (var key in keyPress.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                keyPress[key].Invoke();
            }
        }
        foreach (var key in keyUp.Keys)
        {
            if (Input.GetKeyUp(key))
            {
                keyUp[key].Invoke();
            }
        }
    }

    public static void SetUIInputIgnore(bool state)
    {
        uiInputIgnore = state;
    }

    public static void AddPress(KeyCode keyCode, Action ac)
    {
        if (keyPress.ContainsKey(keyCode))
        {
            keyPress[keyCode] += ac;
        }
        else
        {
            keyPress.Add(keyCode, ac);
        }
    }

    public static void AddUp(KeyCode keyCode, Action ac)
    {
        if (keyUp.ContainsKey(keyCode))
        {
            keyUp[keyCode] += ac;
        }
        else
        {
            keyUp.Add(keyCode, ac);
        }
    }

    public static void RemovePress(KeyCode keyCode, Action action)
    {
        if(keyPress.ContainsKey(keyCode))
        {
            keyPress[keyCode] -= action;
        }
    }

    public static void RemoveUp(KeyCode keyCode, Action action)
    {
        if (keyUp.ContainsKey(keyCode))
        {
            keyUp[keyCode] -= action;
        }
    }

    public static void ClearPress(KeyCode keyCode)
    {
        keyPress.Remove(keyCode);
    }

    public static void ClearUp(KeyCode keyCode)
    {
        keyUp.Remove(keyCode);
    }

    public static void ClearAll()
    {
        keyPress.Clear();
        keyUp.Clear();
    }
}