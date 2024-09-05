using QEntity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : UIEntity
{
    private Transform RightDownNode;
    private Transform MiddleDownNode;
    private Transform CenterNode;

    public override void Init()
    {
        base.Init();
        RightDownNode  = transform.Find("RightDownNode");
        MiddleDownNode = transform.Find("MiddleDownNode");
        CenterNode     = transform.Find("CenterNode");
        rectTrans.sizeDelta = Vector2.zero;
    }

    //public void ClearRightDownNode()
    //{
    //    Util.(RightDownNode);
    //}

    //public void ClearMiddleDownNode()
    //{
    //    Util.ClearAllChildren(MiddleDownNode);
    //}

    //public void ClearCenterNode()
    //{
    //    Util.ClearAllChildren(CenterNode);
    //}

    public Transform GetRightDownNode()
    {
        return RightDownNode;
    }

    public Transform GetMiddleDownNode()
    {
        return MiddleDownNode;
    }

    public Transform GetCenterNode()
    {
        return CenterNode;
    }
}
