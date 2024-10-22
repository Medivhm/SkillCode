using QEntity;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class HotBarItem : UIEntity
{
    public Image bgIcon;
    public Image selectedIcon;
    public ItemUI itemUI;
    public Image itemIcon;
    public Text numText;

    private Item item;
    private bool selected;

    public bool Selected
    {
        get { return selected; }
        set
        {
            if (value == selected) return;

            selected = value;
            if (selected)
            {
                selectedIcon.gameObject.SetActive(true);
                if (item.IsNotNull())
                {
                    // 如果是武器类道具，选中后穿上
                    if(item is Weapon)
                    {
                        (item as Weapon).Dress();
                    }
                }
            }
            else
            {
                selectedIcon.gameObject.SetActive(false);
            }
        }
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        if (item.IsNotNull())
        {
            itemUI.RefreshUI();
        }
        else
        {
            RemoveItem();
        }
    }

    private void Start()
    {
        ResetTransform();
    }

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
    }

    public void RemoveItem()
    {
        item = null;
        itemUI.JustGrid();
        itemUI.gameObject.SetActive(false);
    }

    public void SetItem(Item item)
    {
        if (item.IsNull())
        {
            RemoveItem();
            return;
        }

        this.item = item;
        itemUI.JustItem();
        itemUI.SetLocalScale(0.75f);
        itemUI.gameObject.SetActive(true);
        itemUI.SetItem(item);
        if (selected && item is Weapon)
        {
            (item as Weapon).Dress();
        }
    }

    public Item GetItem()
    {
        return item;
    }
}
