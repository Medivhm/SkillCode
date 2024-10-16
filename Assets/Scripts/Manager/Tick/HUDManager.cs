using Constant;
using QEntity;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;

public class HUDManager : Singleton<HUDManager>, ITickable
{
    static List<HUDEntity> HUDs;
    static Transform HUDParent;
    static bool IsShow;
    public static bool SelfShow => IsShow;

    static bool IsClearing = false;

    public void Init()
    {
        TickHelper.Instance.Add(Instance);
        HUDs = new List<HUDEntity>();
        IsShow = true;
        InitHUDCanvas();
    }

    public static void HideHUD()
    {
        if (!IsShow) return;

        IsShow = false;
        foreach(var hud in HUDs)
        {
            hud.gameObject.SetActive(IsShow);
        }
    }

    public static void ShowHUD()
    {
        if (IsShow) return;

        IsShow = true;
        foreach (var hud in HUDs)
        {
            hud.gameObject.SetActive(IsShow);
        }
    }

    private void InitHUDCanvas()
    {
        GameObject HUDGo = new GameObject("HUD3DCanvas");
        Canvas canvas = HUDGo.AddComponent<Canvas>();
        canvas.sortingOrder = HUDConstant.HUD3DCanvasSortingOrder;
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Main.MainCamera;
        GameObject.DontDestroyOnLoad(HUDGo);
        HUDParent = HUDGo.transform;
        HUDGo.SetUISize(Screen.width, Screen.height);
    }

    public static HUDEntity AddHUD(Transform Owner, string name, float maxBlood, float nowBlood)
    {
        GameObject hudGo = GameObject.Instantiate(LoadTool.LoadPrefab("HUD"));
        hudGo.transform.SetParent(HUDParent);
        HUDEntity hud = hudGo.GetComponent<HUDEntity>();
        hud.Init(Owner, name, maxBlood, nowBlood);
        HUDs.Add(hud);
        return hud;
    }

    public static void RemoveHUD(HUDEntity hud)
    {
        HUDs.Remove(hud);
    }

    public static void DestroyHUD(HUDEntity hud)
    {
        if (hud.IsNull()) return;
        RemoveHUD(hud);
        hud.Destroy();
    }

    public void Update()
    {

    }

    public void LateUpdate()
    {
        if (IsClearing) return;

        DoRefreshHUDs();
    }

    private void DoRefreshHUDs()
    {
        foreach (var hud in HUDs)
        {
            hud.RefreshPosition();
        }
    }
    
    public static void ClearAllHUDs()
    {
        IsClearing = true;
        HUDEntity[] HUDCaches = HUDs.ToArray();
        foreach (var hud in HUDCaches)
        {
            hud.Destroy();
        }
        HUDs.Clear();
    }

    public static void ClearComplete()
    {
        IsClearing = false;
    }
}
