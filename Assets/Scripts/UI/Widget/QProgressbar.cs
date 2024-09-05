using QEntity;
using UnityEngine.UI;
using UnityEngine;
using System;

public class QProgressbar : UIEntity
{
    Image bgImage;
    Image barImage;
    Action overCB;
    bool increaseByTime = false;
    float wholeTime;
    float passTime;

    public bool stop = true;

    private void Awake()
    {
        bgImage = GetComponent<Image>();
        barImage = transform.GetChild(0).GetComponent<Image>();
    }

    public override void ResetScale()
    {
        // do nothing
    }

    public override void Init()
    {
        base.Init();
        ResetData();
    }

    public void ResetData()
    {
        stop = false;
        SetBarProgress(0f);
    }

    public void SetIncreaseByTime(float wholeTime)
    {
        ResetData();
        this.wholeTime = wholeTime;
        this.passTime = 0f;
        increaseByTime = true;
    }

    private void Update()
    {
        if(increaseByTime && !stop)
        {
            passTime += Time.deltaTime;
            SetBarProgress(passTime / wholeTime);
        }
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

    public void SetBgImage(Sprite bgSprite)
    {
        bgImage.sprite = bgSprite;
    }

    public void SetBarImage(Sprite barSprite)
    {
        barImage.sprite = barSprite;
    }

    public void SetBgColor(Color color)
    {
        bgImage.color = color;
    }

    public void SetBarColor(Color color)
    {
        barImage.color = color;
    }

    // value: 0 ~ 1
    public void SetBarProgress(float value)
    {
        barImage.fillAmount = value;
        if(value > 0.999999f)
        {
            over();
        }
    }

    public void over()
    {
        if (overCB.IsNotNull())
        {
            overCB.Invoke();
        }
        stop = true;
    }

    public void SetOverCB(Action cb)
    {
        overCB = cb;
    }
}
