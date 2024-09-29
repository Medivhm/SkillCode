using QEntity;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TabUI : UIEntity
{
    public Button button;
    public Text text;

    private void Start()
    {
        ResetTransform();
    }

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
    }

    public void SetTitle(string title)
    {
        text.text = title;
    }

    public void SetClickCB(Action onClickCB)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(()=> { onClickCB.Invoke(); });
    }
}
