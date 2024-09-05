using DG.Tweening;
using QEntity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TaskDetail : UIEntity
{
    public Text title;
    public Text detail;
    public Button okBtn;
    public Button cancelBtn;

    float width;

    public override void Init()
    {
        base.Init();
        width = this.rectTrans.rect.width;
        StartCoroutine(YieldInit());
        rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, 0);
    }

    public void SetInfo(string title, string detail, Action okCB, Action cancelCB)
    {
        this.title.text = title;
        this.detail.text = detail;
        okBtn.onClick.RemoveAllListeners();
        okBtn.onClick.AddListener(()=> 
        { 
            okCB(); 
        });
        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(() =>
        {
            cancelCB();
        });
    }

    IEnumerator YieldInit()
    {
        yield return null;
    }

    public void MoveToLeft()
    {
        rectTrans.DOAnchorPosX(-width, 0.3f);
    }

    public void MoveToRight()
    {
        rectTrans.DOAnchorPosX(0f, 0.3f);
    }

    public override void Destroy()
    {
        this.transform.DOPause();
        rectTrans.DOPause();
        base.Destroy();
    }
}
