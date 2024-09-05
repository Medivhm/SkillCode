using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UtilExtensions
{
    public static void SetMagicSkill(this Magic.Magic magic, int magicSkillID)
    {
        magic.SetMagicSkill(magic, magicSkillID);
    }

    public static void SetCarrier(this Magic.Magic magic, int carrierID, int fxID)
    {
        magic.SetCarrier(magic, carrierID, fxID);
    }

    public static int ClosestIntToZero(this float value)
    {
        return (value > 0) ? Mathf.FloorToInt(value) : Mathf.CeilToInt(value);
    }

    public static Vector3 ToReal(this Vector3 value)
    {
        DebugTool.Log(Main.CanvasScaleFactor);
        return value * Main.CanvasScaleFactor;
    }

    public static Vector2 ToReal(this Vector2 value)
    {
        return value / Main.CanvasScaleFactor;
    }

    public static float ToReal(this float value)
    {
        return value / Main.CanvasScaleFactor;
    }

    public static void ClearAllChildren(this Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject.Destroy(parent.GetChild(i).gameObject);
        }
    }

    public static Vector3 CopyAndChangeZ(this Vector3 vec3, float value)
    {
        vec3.z = value;
        return vec3;
    }

    public static Vector3 CopyAndChangeY(this Vector3 vec3, float value)
    {
        vec3.y = value;
        return vec3;
    }

    public static Vector3 CopyAndChangeX(this Vector3 vec3, float value)
    {
        vec3.x = value;
        return vec3;
    }

    public static Vector3 GetRamdomPointRoundMin(this Vector3 center, float radius, float minRadius)
    {
        return new Vector3(center.x + minRadius + 2 * radius * Random.value - radius, center.y, center.z + minRadius + 2 * radius * Random.value - radius);
    }

    public static Vector3 GetRamdomPointRound(this Vector3 center, float radius)
    {
        return new Vector3(center.x + 2 * radius * Random.value - radius, center.y, center.z + 2 * radius * Random.value - radius);
    }

    static Vector3 vec9999 = new Vector3(9999, 9999, 9999);
    public static void MoveTo9999(this GameObject go)
    {
        go.transform.position = vec9999;
    }

    public static void MoveTo9999(this Transform trans)
    {
        trans.position = vec9999;
    }

    // 有时候效果不对，慎用！！！下同
    public static bool IsNotNull(this object obj)
    {  
        return obj != null;
    }

    public static bool IsNull(this object obj)
    {
        return obj == null;
    }

    public static void ListCleanNull<T>(this List<T> list)
    {
        list.RemoveAll(item  => item == null);
    }

    public static bool CheckChildActive(this Transform trans)
    {
        if(trans.childCount == 0) return true;
        return trans.GetChild(0).gameObject.activeSelf;
    }

    public static void HideAllChildren(this Transform trans)
    {
        for(int i = 0; i < trans.childCount; i++)
        {
            trans.GetChild(i).gameObject.SetActive(false);
        }
    }

    public static void ShowAllChildren(this Transform trans)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            trans.GetChild(i).gameObject.SetActive(true);
        }
    }

    public static void ForceRebuildLayout(this Transform trans)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(trans.GetComponent<RectTransform>());
    }

    public static void ForceRebuildLayout(this RectTransform rect)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    public static void SetUISize(this GameObject go, int width, int height)
    {
        if (go.IsNull())
        {
            Debug.LogWarning("SetUISize: GameObject is null.");
            return;
        }

        RectTransform rect = go.GetComponent<RectTransform>();
        if (rect)
        {
            rect.sizeDelta = new Vector2(width, height);
        }
        else
        {
            Debug.LogWarning($"SetUISize: GameObject {go.name} does not have a RectTransform component.");
        }
    }
}