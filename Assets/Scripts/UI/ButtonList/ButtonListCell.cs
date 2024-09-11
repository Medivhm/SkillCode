using QEntity;
using System;
using UnityEngine.UI;

public class ButtonListCell : UIEntity
{
    Button btn;
    Text text;

    private void Awake()
    {
        btn = this.gameObject.GetComponent<Button>();
        text = this.gameObject.GetComponentInChildren<Text>();
    }

    public void Init(string title, Action<ButtonListCell> clickCB)
    {
        base.Init();
        text.text = title;
        btn.onClick.AddListener(() => { clickCB.Invoke(this); });
    }

    public void SetClickCB()
    {
        btn.onClick.RemoveAllListeners();
    }
}
