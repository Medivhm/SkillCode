﻿using Manager;
using QEntity;
using System;
using System.Collections.Generic;
using Tools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class UtilExtensions
{
    //    List<UnitEntity> GetEntitiesInSphereArea(this GameObejct go)
    //    {

    //    }

    public static bool PointInRect(this Vector2 point, RectTransform rect)
    {

        //return rect.rect.xMin < point.x && point.x < rect.rect.xMax
        //    && rect.rect.yMin < point.y && point.y < rect.rect.yMax;

        return RectTransformUtility.RectangleContainsScreenPoint(rect, point);
    }

    public static void SetLocalScale(this Transform trans, float scale)
    {
        trans.localScale = new Vector3(scale, scale, scale);
    }

    public static bool TargetInRight(this UnitEntity me, UnitEntity other)
    {
        return Vector3.Dot(me.transform.right, other.Position - me.Position) > 0;
    } 

    public static void MoveToUnit(this GameObject go, Transform Target, Action OverEvent = null)
    {
        MoveToUnit script = go.GetOrAddComponent<MoveToUnit>();
        script.Init(Target, OverEvent);
    }

    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t.IsNull())
        {
            t = go.AddComponent<T>();
        }
        return t;
    }

    public static bool IsMe(this GameObject go, GameObject otherGo)
    {
        return go.GetInstanceID().Equals(otherGo.GetInstanceID());
    }

    public static bool IsNotMe(this GameObject go, GameObject otherGo)
    {
        return !IsMe(go, otherGo);
    }

    static SphereCollider collider;
    public static SphereCollider AddSphereCollider(this Transform trans, float radius, string name = "SphereCollider")
    {
        Transform colliderTrans = GameObject.Instantiate(LoadTool.LoadPrefab("SphereCollider")).transform;
        colliderTrans.name = name;
        colliderTrans.SetParent(trans);
        colliderTrans.localPosition = Vector3.zero;
        collider = colliderTrans.GetComponent<SphereCollider>();
        collider.radius = radius;
        return collider;
    }

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
        return new Vector3(center.x + minRadius + 2 * radius * UnityEngine.Random.value - radius, center.y, center.z + minRadius + 2 * radius * UnityEngine.Random.value - radius);
    }

    public static Vector3 GetRamdomPointRound(this Vector3 center, float radius)
    {
        return new Vector3(center.x + 2 * radius * UnityEngine.Random.value - radius, center.y, center.z + 2 * radius * UnityEngine.Random.value - radius);
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

    // 在对script脚本使用时效果不对，慎用！！！下同  IsUnityNull() ? 
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
        list.RemoveAll(item => item == null);
    }

    public static bool CheckChildActive(this Transform trans)
    {
        if (trans.childCount == 0) return true;
        return trans.GetChild(0).gameObject.activeSelf;
    }

    public static void HideAllChildren(this Transform trans)
    {
        for (int i = 0; i < trans.childCount; i++)
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

    public static void RemoveAllChildren(this Transform parent)
    {
        for(int i = 0; i < parent.childCount; i++)
        {
            GameObject.Destroy(parent.GetChild(i));
        }
    }
}
