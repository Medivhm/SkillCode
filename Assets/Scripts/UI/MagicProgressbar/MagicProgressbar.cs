using QEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicProgressbar : UIEntity
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private QProgressbar progressbar;
    public bool ChargeOver
    {
        get { return progressbar.chargeOver; }
    }

    protected override void Awake()
    {
        base.Awake();
        Main.MagicProgressbar = this;
    }

    private void OnDestroy()
    {
        Main.MagicProgressbar = null;
    }

    public void ResetData()
    {
        progressbar.ResetData();
    }

    public void Start()
    {
        ResetTransform();
        progressbar.Init();
        progressbar.SetStartCB(()=>progressbar.SetBarColor(Color.red));
        progressbar.SetOverCB(() => progressbar.SetBarColor(Color.yellow));
        this.gameObject.SetActive(false);
    }

    public void SetTitle(string title)
    {
        text.text = title;
    }

    public void StartProgressBar(float time, Action overCB = null)
    {
        if (overCB.IsNotNull())
        {
            progressbar.SetOverCB(overCB);
        }
        progressbar.SetIncreaseByTime(time);
    }

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
    }
}
