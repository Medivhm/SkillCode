using System;
using System.Collections.Generic;
using UnityEngine;

class MainMouseController : Singleton<MainMouseController>, ITickable
{
    Action mouseLeftButtonClick;
    Action<RaycastHit[]> mouseLeftRayHit;
    Dictionary<RectTransform, Action> mouseNextClickCB;

    public void Init()
    {
        mouseNextClickCB = new Dictionary<RectTransform, Action>();
        TickHelper.Instance.Add(this);
        Ctrl.UseMouse();
    }

    // 设置鼠标可见
    public void SetCursorVisible(bool state)
    {
        Cursor.visible = state;
}

    // 设置鼠标模式
    // None, Locked: 鼠标锁定在屏幕中央，动不了, Confined: 鼠标限制在窗口内，仅限Linux和Windows
    public void SetCursorLockstate(CursorLockMode lockMode)
    {
        Cursor.lockState = lockMode;
    }

    public void Update()
    {
        MouseClickTick();
        MouseNextClick();
    }

    //private bool MouseReady()
    //{
    //    return true;
    //}

    //private void MouseMoveTick()
    //{
    //    //Vector2 playerScreenPos = Main.MainCamera.WorldToScreenPoint(Main.MainPlayer.Position);
    //    //Main.MainPlayer.AttackDir = (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - playerScreenPos).normalized;
    //}

    public void AddMouseNextClick(RectTransform rectTrans, Action action)
    {
        if (mouseNextClickCB.ContainsKey(rectTrans))
        {
            mouseNextClickCB[rectTrans] += action;
        }
        else
        {
            mouseNextClickCB.Add(rectTrans, action);
        }
    }

    public void RemoveMouseNextClick(RectTransform rectTrans, Action action)
    {
        if (mouseNextClickCB.ContainsKey(rectTrans))
        {
            mouseNextClickCB[rectTrans] -= action;
            if (mouseNextClickCB[rectTrans].IsNull())
            {
                mouseNextClickCB.Remove(rectTrans);
            }
        }
    }

    Vector3 mousePosition;
    List<RectTransform> delRects = new List<RectTransform>();
    private void MouseNextClick()
    {
        mousePosition = Input.mousePosition / Main.CanvasScaleFactor;
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && mouseNextClickCB.Count > 0)
        {
            foreach (var pair in mouseNextClickCB)
            {
                if(pair.Key == null)
                {
                    delRects.Add(pair.Key);
                }
                else
                {
                    if (!IsPointerInRect(pair.Key, mousePosition))
                    {
                        delRects.Add(pair.Key);
                        pair.Value.Invoke();
                    }
                }
            }

            foreach(var rect in delRects)
            {
                mouseNextClickCB.Remove(rect);
            }
            delRects.Clear();
        }
    }

    private bool IsPointerInRect(RectTransform rect, Vector2 mousePosition)
    {
        return mousePosition.x > rect.offsetMin.x
            && mousePosition.y > rect.offsetMin.y
            && mousePosition.x < rect.offsetMax.x
            && mousePosition.y < rect.offsetMax.y;
    }

    Ray ray;
    private void MouseClickTick()
    {
        if (Input.GetMouseButton(0))
        {
            if (mouseLeftButtonClick != null)
            {
                mouseLeftButtonClick.Invoke();
            }
            ray = Main.MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            MouseRayHit(ray);
        }
    }

    public void MouseRayHit(Ray ray)
    {
        if (mouseLeftRayHit != null)
        {
            RaycastHit[] infos = Physics.RaycastAll(ray.origin, ray.direction);
            mouseLeftRayHit.Invoke(infos);
        }
    }

    public void AddMouseLeftClick(Action action)
    {
        mouseLeftButtonClick = action;
    }


    public void AddMouseLeftRayHit(Action<RaycastHit[]> action)
    {
        mouseLeftRayHit += action;
    }
}

