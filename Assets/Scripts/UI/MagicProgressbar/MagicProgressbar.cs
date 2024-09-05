using QEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicProgressbar : UIEntity
{
    public Text text;
    public QProgressbar progressbar;

    private void Awake()
    {
        Main.MagicProgressbar = this;
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        progressbar.Init();
    }

    public void SetTitle(string title)
    {
        text.text = title;
    }

    public void StartProgressBar(float time, Action overCB)
    {
        progressbar.SetOverCB(overCB);
        progressbar.SetIncreaseByTime(time);
    }
}
