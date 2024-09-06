using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class MainMouseController : Singleton<MainMouseController>, ITickable
{
    Action mouseLeftButtonClick;
    Action mouseLeftButtonDown;
    Action mouseLeftButtonUp;
    Dictionary<RectTransform, Action> mouseNextClickCB;
    public bool mouseShow = false;

    public void Init()
    {
        mouseNextClickCB = new Dictionary<RectTransform, Action>();
        TickHelper.Instance.Add(this);
        SetCursorLockstate(CursorLockMode.Locked);
    }

    // 设置鼠标可见
    public void SetCursorVisible(bool state)
    {
        Cursor.visible = state;
        mouseShow = state;
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
        MouseDownTick();
        MouseUpTick();
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

    private void MouseClickTick()
    {
        if (Input.GetMouseButton(0))
        {
            if (mouseLeftButtonClick != null)
            {
                mouseLeftButtonClick.Invoke();
            }
        }
    }

    private void MouseDownTick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mouseLeftButtonDown.IsNotNull())
            {
                mouseLeftButtonDown.Invoke();
            }
        }
    }

    private void MouseUpTick()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if (mouseLeftButtonUp.IsNotNull())
            {
                mouseLeftButtonUp.Invoke();
            }
        }
    }

    public RaycastHit[] GetMouseRayHit()
    {
        return RayHitOnScreen(Input.mousePosition.x, Input.mousePosition.y);
    }

    public RaycastHit[] GetCenterScreenRayHit()
    {
        return RayHitOnScreen(Screen.width / 2, Screen.height / 2);
    }

    Ray ray;
    RaycastHit[] RayHitOnScreen(float x, float y)
    {
        ray = Main.MainCamera.ScreenPointToRay(new Vector3(x, y, 0));
        return Physics.RaycastAll(ray.origin, ray.direction);
    }

    public void AddMouseLeftUp(Action action)
    {
        mouseLeftButtonUp = action;
    }

    public void AddMouseLeftDown(Action action)
    {
        mouseLeftButtonDown = action;
    }

    public void AddMouseLeftClick(Action action)
    {
        mouseLeftButtonClick = action;
    }
}

