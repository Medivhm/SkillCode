using QEntity;
using Info;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Manager;

public class ItemUI : UIEntity, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject Grid;
    public Image ItemIcon;
    public Text NumText;
    public Item item;

    private ItemTipsUI tip;
    private ButtonList btnList;
    private bool showTips = true;

    public ItemInfo ItemInfo
    {
        get { return item.Info; }
    }

    public int ItemID
    {
        get { return ItemInfo.ID; }
    }

    public string Name
    {
        get { return ItemInfo.name; }
    }

    public Color Color
    {
        get { return item.Color; }
    }

    public int Overlap
    {
        get { return ItemInfo.overlap; }
    }

    public string Icon
    {
        get { return ItemInfo.icon; }
    }

    public string Desc
    {
        get { return ItemInfo.desc; }
    }

    protected override void Awake()
    {
        item = null;
        ResetTransform();
    }

    public override void Init()
    {
        base.Init();
        BeginDragEvent = null;
        DragEvent = null;
        EndDragEvent = null;
    }

    public override void ResetTransform()
    {
        base.ResetTransform();
    }

    public void JustItem()
    {
        Grid.SetActive(false);
        ItemIcon.gameObject.SetActive(true);
        NumText.gameObject.SetActive(true);
    }

    public void JustGrid()
    {
        item = null;
        Grid.SetActive(true);
        ItemIcon.gameObject.SetActive(false);
        NumText.gameObject.SetActive(false);
    }

    public bool IsJustGrid()
    {
        return item.IsNull();
    }

    public void SetShowTips(bool state)
    {
        showTips = state;
    }

    public void SetItem(Item item)
    {
        if (item.IsNull())
        {
            JustGrid();
            return;
        }

        this.item = item;
        ItemIcon.gameObject.SetActive(true);
        NumText.gameObject.SetActive(true);

        ItemIcon.sprite = LoadTool.LoadSprite(item.Icon);
        if(item.Overlap == 1 || item.Num == 1)
        {
            NumText.text = "";
        }
        else
        {
            NumText.text = item.Num.ToString();
        }
    }

    public override void RefreshUI()
    {
        NumText.text = item.Num.ToString();
    }

    public void CreateTips(Vector3 pos)
    {
        if (!showTips) return;

        if (tip.IsNotNull())
        {
            return;
        }

        if (btnList.IsNotNull())
        {
            return;
        }

        if (item.IsNull())
        {
            return;
        }

        tip = GUI.CreateItemTips(Main.MainCanvas.transform, pos, item);
        UIManager.AddTip(this);
    }

    public void DestroyTips()
    {
        if (tip.IsNull())
        {
            return;
        }

        tip.Destroy();
        UIManager.RemoveTip(this);
        tip = null;
    }

    public void CreateBtnList(Vector3 pos)
    {
        if (btnList.IsNotNull())
        {
            return;
        }

        if (item.IsNull())
        {
            return;
        }

        btnList = GUI.CreateButtonList(Main.MainCanvas.transform, pos);
        MainMouseController.Instance.AddMouseNextClick(btnList.rectTrans, DestroyBtnList);
        btnList.AddButton("Ê¹ÓÃ1¸ö", (_) =>
        {
            UseItem(1);
            MainMouseController.Instance.RemoveMouseNextClick(btnList.rectTrans, DestroyBtnList);
            DestroyBtnList();
        });
    }

    public void DestroyBtnList()
    {
        if (btnList.IsNull())
        {
            return;
        }

        btnList.Destroy();
        btnList = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CreateTips(eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyTips();
        if (btnList.IsNotNull())
        {
            MainMouseController.Instance.AddMouseNextClick(btnList.rectTrans, DestroyBtnList);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DestroyTips();
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            CreateBtnList(eventData.position);
        }
    }

    public ItemUI SetLocalScale(float scale)
    {
        transform.SetLocalScale(scale);
        return this;
    }

    public ItemUI SetLocalPosition(Vector3 pos)
    {
        transform.localPosition = pos;
        return this;
    }

    public void UseItem(int num)
    {
        if (num < 1)
        {
            return;
        }

        Bag.Instance.UseItem(item, num);
    }

    public override void Destroy()
    {
        DestroyBtnList();
        DestroyTips();
        base.Destroy();
    }


    public Action<ItemUI, PointerEventData> BeginDragEvent;
    public Action<ItemUI, PointerEventData> DragEvent;
    public Action<ItemUI, PointerEventData> EndDragEvent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (BeginDragEvent.IsNotNull())
        {
            BeginDragEvent.Invoke(this, eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DragEvent.IsNotNull())
        {
            DragEvent.Invoke(this, eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EndDragEvent.IsNotNull())
        {
            EndDragEvent.Invoke(this, eventData);
        }
    }
}
