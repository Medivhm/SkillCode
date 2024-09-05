using QEntity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgScrollUI : UIEntity
{
    private Transform ScrollContent;
    private ActionInfoCell item;

    public override void Init()
    {
        base.Init();
        ScrollContent = transform.Find("ScrollContent");
    }

}
