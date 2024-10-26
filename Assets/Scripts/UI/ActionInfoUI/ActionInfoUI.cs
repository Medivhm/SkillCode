using Constant;
using DG.Tweening;
using QEntity;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class ActionInfoUI : UIEntity
{
    private Transform ScrollView;
    private List<ActionInfoCell> scripts;

    protected override void Awake()
    {
        base.Awake();
        Main.ActionInfoUI = this;

        ScrollView = transform.Find("Mask");
        scripts = new List<ActionInfoCell>();
        waitDestroy = new List<ActionInfoCell>();
    }

    private void Start()
    {
        ResetTransform();
    }

    private void OnDestroy()
    {
        Main.ActionInfoUI = null;
    }

    public void CreateInfo(string info)
    {
        OtherCellMoveUp();
        GameObject cellGo = LoadTool.LoadUI(UIConstant.ActionInfoCell);
        cellGo.transform.SetParent(ScrollView);
        ActionInfoCell script = cellGo.GetComponent<ActionInfoCell>();
        script.Init(info);
        if (!scripts.Contains(script))
        {
            scripts.Add(script);
        }
    }

    private List<ActionInfoCell> waitDestroy;
    private void OtherCellMoveUp()
    {
        waitDestroy.Clear();
        foreach(var sc in scripts)
        {
            if (sc.IsDestroy)
            {
                waitDestroy.Add(sc);
            }
            else
            {
                sc.endYAdd();
            }
        }
        foreach(var destroySc in waitDestroy)
        {
            scripts.Remove(destroySc);
        }
    }

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
    }
}
