using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;
using UnityEngine.UIElements;

struct PressAction
{
    public bool influenceByIgnore;
    public Action action;
}

class QuickKeyManager : MonoSingleton<QuickKeyManager>
{
    static Dictionary<KeyCode, PressAction> quickKeys;

    static List<KeyCode> removeList;
    static bool ignore = false;

    private void Awake()
    {
        quickKeys = new Dictionary<KeyCode, PressAction>();
        removeList = new List<KeyCode>();
    }

    private void Update()
    {
        if (removeList.IsNull()) return;

        foreach(var key in removeList)
        {
            quickKeys.Remove(key);
        }
        removeList.Clear();

        foreach(var key in quickKeys.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                if (!(ignore && quickKeys[key].influenceByIgnore))
                {
                    quickKeys[key].action.Invoke();
                }
            }
        }
    }

    public void Init()
    {
    }

    public static void SetIgnore(bool state)
    {
        ignore = state;
    }
    
    public static void Add(KeyCode key, Action ac, bool influenceByIgnore = true)
    {
        PressAction action;
        action.action = ac;
        action.influenceByIgnore = influenceByIgnore;
        quickKeys[key] = action;
    }

    public static void Remove(KeyCode key)
    {
        removeList.Add(key);
    }

    public static void ClearAll()
    {
        foreach (var key in quickKeys.Keys)
        {
            Remove(key);
        }
    }
}