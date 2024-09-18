using QEntity;
using UnityEngine;
using UnityEngine.UI;

public class QButton : UIEntity
{
    public Text text;
    public Button button;
    public Button.ButtonClickedEvent onClick => button.onClick;

    private void Start()
    {
        ResetTransform();
    }

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
    }

    public void SetText(string str)
    {
        text.text = str;
    }

    public void SetWidth(float width)
    {
        SetWidthAndHeight(width, rectTrans.sizeDelta.y);
    }

    public void SetHeight(float height)
    {
        SetWidthAndHeight(rectTrans.sizeDelta.x, height);
    }

    public void SetWidthAndHeight(float width, float height)
    {
        rectTrans.sizeDelta = new Vector2(width, height);
    }

    public void SetFontSize(int fontSize)
    {
        text.fontSize = fontSize;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }
}
