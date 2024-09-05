using QEntity;
using System;
using UnityEngine.UI;

public class ButtonListCell : UIEntity
{
    Button btn;
    Text text;

    public void Init(string title, Action<ButtonListCell> clickCB)
    {
        base.Init();
        btn = this.gameObject.GetComponent<Button>();
        text = this.gameObject.GetComponentInChildren<Text>();
        text.text = title;
        btn.onClick.AddListener(() => { clickCB.Invoke(this); });
    }

    public void SetClickCB()
    {
        btn.onClick.RemoveAllListeners();
    }
}
