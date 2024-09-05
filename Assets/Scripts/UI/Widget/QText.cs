using QEntity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QText : UIEntity
{
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }
    
    public void SetLocalPos(Vector3 localPos)
    {
        transform.localPosition = localPos;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void SetString(string str)
    {
        text.text = str;
    }

    public void SetColor(Color color)
    {
        text.color = color;
    }

    public void SetFontSize(int fontSize)
    {
        text.fontSize = fontSize;
    }

    public void SetWidth(float width)
    {
        rectTrans.sizeDelta.Set(width, rectTrans.sizeDelta.y);
    }

    public Text GetText()
    {
        return text;
    }
}
