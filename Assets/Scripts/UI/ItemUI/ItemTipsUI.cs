using QEntity;
using Info;
using System.Collections;
using System.Collections.Generic;
using Tools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemTipsUI : UIEntity
{
    public Text Name;
    public Text Desc;

    public void Init(Vector3 pos, Item item)
    {
        base.Init();
        InitPosition(pos);
        InitInfo(item);
    }

    private void InitPosition(Vector3 pos)
    {
        this.transform.position = pos;
    }

    private void InitInfo(Item item)
    {
        Name.text = item.Name;
        Desc.text = item.Desc;
    }
}
