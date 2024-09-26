using QEntity;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public static class QUtil
{
    public static List<UnitEntity> FindUnitInBox(this GameObject go, Vector3 boxCenterPos, Vector3 halfExtents, Quaternion rotate)
    {
        Collider[] colliders = Physics.OverlapBox(boxCenterPos, halfExtents, rotate, 1 << Constant.Layer.Unit, QueryTriggerInteraction.Ignore);
        List<UnitEntity> units = new List<UnitEntity>();
        foreach (var collider in colliders)
        {
            //if (collider.GetComponent<UnitEntity>().IsNotNull())   // 可能有玩家身上的unit层级的碰撞体
            //{
            units.Add(collider.GetComponent<UnitEntity>());
            //}
        }
        return units;
    }

    public static int GetRandom(int min, int max)
    {
        return Mathf.FloorToInt(UnityEngine.Random.value * (max - min + 1) + min);
    }

    public static float GetRandom(float min, float max)
    {
        return UnityEngine.Random.value * (max - min) + min;
    }

    public static float GetRandom01()
    {
        return UnityEngine.Random.value;
    }

    static PointerEventData pointerEventData;
    public static List<RaycastResult> RayFromMousePosition()
    {
        pointerEventData = new PointerEventData(Main.EventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        Main.GraphicRaycaster.Raycast(pointerEventData, results);
        return results;
    }

    public static bool CheckHitTag(string tag)
    {
        foreach (var res in RayFromMousePosition())
        {
            if (tag.Equals(res.gameObject.tag))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsMultipleOfNum(int bigNum, int num)
    {
        return bigNum % num == 0;
    }

    public static byte[] GetBytes(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static long GetSecondsByTimestamp(long timestamp)
    {
        return GetNowTimestamp() - timestamp;
    }

    public static long GetNowTimestamp()
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    public static AnimationClip GetAnimationClip(Animator animator, string name)
    {
        if (animator == null)
        {
            DebugTool.Error("animator为空");
        }
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }
        return null;
    }

    public static float GetDegX(Vector3 vec3)
    {
        return Vector3.SignedAngle(Vector3.forward, vec3, Vector3.right);
    }

    public static float GetDegY(Vector3 vec3)
    {
        return Vector3.SignedAngle(Vector3.forward, vec3, Vector3.up);
    }

    public static float GetDegZ(Vector3 vec3)
    {
        return Vector3.SignedAngle(Vector3.right, vec3, Vector3.forward);
    }

    // 不会改变原数据，返回的是从小到大的排名，比如sortIndex[0] = 4，则说明原数组[4]是最小的float
    public static int[] QuickSort(float[] array)
    {
        int length = array.Length;
        float[] arrayClone = (float[])array.Clone();
        int[] sortIndex = new int[length];
        for (int i = 0; i < sortIndex.Length; i++)
        {
            sortIndex[i] = i;
        }

        QuickSortEx(arrayClone, 0, length - 1, sortIndex);
        return sortIndex;
    }

    static void QuickSortEx(float[] array, int a, int b, int[] sortIndex)
    {
        if (a >= b) return;

        int changeIndex = a;
        int leftVoid = a;
        int rightVoid = b;
        bool findRight = true;
        int temp2;
        float temp;
        while (leftVoid != rightVoid)
        {
            if (findRight)
            {
                if (array[rightVoid] < array[changeIndex])
                {
                    temp2 = sortIndex[changeIndex];
                    sortIndex[changeIndex] = sortIndex[rightVoid];
                    sortIndex[rightVoid] = temp2;

                    temp = array[changeIndex];
                    array[changeIndex] = array[rightVoid];
                    array[rightVoid] = temp;

                    changeIndex = rightVoid;
                    findRight = !findRight;
                }
                else
                {
                    rightVoid--;
                }
            }
            else
            {
                if (array[leftVoid] > array[changeIndex])
                {
                    // 排排名数据
                    temp2 = sortIndex[changeIndex];
                    sortIndex[changeIndex] = sortIndex[leftVoid];
                    sortIndex[leftVoid] = temp2;
                    // 排实际数据
                    temp = array[changeIndex];
                    array[changeIndex] = array[leftVoid];
                    array[leftVoid] = temp;
                    // 换顺序
                    changeIndex = leftVoid;
                    findRight = !findRight;
                }
                else
                {
                    leftVoid++;
                }
            }
        }
        QuickSortEx(array, a, changeIndex - 1, sortIndex);
        QuickSortEx(array, changeIndex + 1, b, sortIndex);
    }

    public static int[] QuickSort(int[] array)
    {
        int length = array.Length;
        int[] arrayClone = (int[])array.Clone();
        int[] sortIndex = new int[length];
        for (int i = 0; i < sortIndex.Length; i++)
        {
            sortIndex[i] = i;
        }

        QuickSortEx(arrayClone, 0, length - 1, sortIndex);
        return sortIndex;
    }

    static void QuickSortEx(int[] array, int a, int b, int[] sortIndex)
    {
        if (a >= b) return;

        int changeIndex = a;
        int leftVoid = a;
        int rightVoid = b;
        bool findRight = true;
        int temp2;
        int temp;
        while (leftVoid != rightVoid)
        {
            if (findRight)
            {
                if (array[rightVoid] < array[changeIndex])
                {
                    temp2 = sortIndex[changeIndex];
                    sortIndex[changeIndex] = sortIndex[rightVoid];
                    sortIndex[rightVoid] = temp2;

                    temp = array[changeIndex];
                    array[changeIndex] = array[rightVoid];
                    array[rightVoid] = temp;

                    changeIndex = rightVoid;
                    findRight = !findRight;
                }
                else
                {
                    rightVoid--;
                }
            }
            else
            {
                if (array[leftVoid] > array[changeIndex])
                {
                    temp2 = sortIndex[changeIndex];
                    sortIndex[changeIndex] = sortIndex[leftVoid];
                    sortIndex[leftVoid] = temp2;

                    temp = array[changeIndex];
                    array[changeIndex] = array[leftVoid];
                    array[leftVoid] = temp;

                    changeIndex = leftVoid;
                    findRight = !findRight;
                }
                else
                {
                    leftVoid++;
                }
            }
        }
        QuickSortEx(array, a, changeIndex - 1, sortIndex);
        QuickSortEx(array, changeIndex + 1, b, sortIndex);
    }

    public static Vector3 GetMiddleVec(Vector3 v1, Vector3 v2)
    {
        return (v1 + v2) / 2;
    }

    static Vector3 point;
    public static Vector3 GetMiddleVec(List<Vector3> points)
    {
        point = Vector3.zero;
        foreach (var p in points)
        {
            point += p;
        }
        point /= points.Count;
        return point;
    }
}