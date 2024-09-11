using QEntity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgScrollUI : UIEntity
{
    private Transform ScrollContent;
    private ActionInfoCell item;

    public void Start()
    {
        ScrollContent = transform.Find("ScrollContent");
    }

}
