using QEntity;
using Info;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : UIEntity, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject Grid;
    public Image ItemIcon;
    public Text NumText;
    public Item item;

    private ItemTipsUI tips;
    private ButtonList btnList;

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

    public void Start()
    {
        item = null;
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

    public void SetItem(Item item)
    {
        this.item = item;
        ItemIcon.gameObject.SetActive(true);
        NumText.gameObject.SetActive(true);

        ItemIcon.sprite = LoadTool.LoadSprite(item.Icon);
        NumText.text = item.Num.ToString();
    }

    public void RefreshValue()
    {
        NumText.text = item.Num.ToString();
    }

    public void CreateTips(Vector3 pos)
    {
        if (tips.IsNotNull())
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

        tips = GUI.CreateItemTips(Main.MainCanvas.transform, pos, item);
    }

    public void DestroyTips()
    {
        if (tips.IsNull())
        {
            return;
        }

        tips.Destroy();
        tips = null;
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
        base.Destroy();
    }
}
