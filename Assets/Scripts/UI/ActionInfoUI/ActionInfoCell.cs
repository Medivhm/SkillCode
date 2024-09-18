using DG.Tweening;
using QEntity;
using Info;
using System.Collections;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class ActionInfoCell : UIEntity
{
    public Image Icon;
    public Text TextName;
    public Text TextNum;

    private float width;
    private float height;
    private float startX;
    private float endX;

    private float endY;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        ResetTransform();
    }

    public void Init(ItemInfo itemInfo, int num)
    {
        Icon.sprite = LoadTool.LoadSprite(itemInfo.icon);
        TextNum.text = num.ToString();
        Init(itemInfo.name);
    }

    public void Init(string text)
    {
        base.Init();
        TextName.text = text;
        InitMovePos();
    }

    IEnumerator Move()
    {
        MoveToLeft();
        yield return new WaitForSeconds(3f);
        MoveToRight();
    }

    public void InitMovePos()
    {
        endY = 0;
        transform.localPosition = Vector3.zero;
        width  = GetComponent<RectTransform>().sizeDelta.x;
        height = GetComponent<RectTransform>().sizeDelta.y;
        startX = this.transform.localPosition.x;
        endX   = startX - width;
        StartCoroutine(Move());
    }

    public void endYAdd()
    {
        endY += height;
        MoveToTop();
    }

    public void MoveToLeft()
    {
        this.transform.DOLocalMoveX(endX, 0.3f);
    }

    public void MoveToRight()
    {
        this.transform.DOLocalMoveX(startX, 0.3f).OnComplete(()=>Destroy());
    }

    public void MoveToTop()
    {
        if(!IsDestroy)
        {
            this.transform.DOLocalMoveY(endY, 0.2f);
        }
    }

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
    }

    public override void Destroy()
    {
        this.transform.DOPause();
        base.Destroy();
    }
}
