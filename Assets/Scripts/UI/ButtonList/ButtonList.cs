using Constant;
using QEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class ButtonList : UIEntity
{
    public Transform buttonList;

    public void Init(Vector3 pos)
    {
        base.Init();
        InitPosition(pos);
        this.transform.ClearAllChildren();
    }
    private void InitPosition(Vector3 pos)
    {
        this.transform.position = pos;
    }

    public ButtonListCell AddButton(string title, Action<ButtonListCell> clickCB)
    {
        GameObject cellGo = CreateBtnCell();
        cellGo.transform.SetParent(buttonList);
        ButtonListCell cell = cellGo.GetComponent<ButtonListCell>();
        cell.Init(title, clickCB);
        return cell;
    }

    private GameObject CreateBtnCell()
    {
        return LoadTool.LoadUI(UIConstant.ButtonListCell);
    }
}
