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
                    item.Use();
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
        if (this.item.IsNotNull()) return;

        if (item.IsNull())
        {
            RemoveItem();
            return;
        }

        this.item = item;
        itemUI.gameObject.SetActive(true);
        itemUI.SetItem(item);
    }

    public Item GetItem()
    {
        return item;
    }
}
