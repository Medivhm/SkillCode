using QEntity;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class HotBarItem : UIEntity
{
    public Image bgIcon;
    public Image itemIcon;
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
                bgIcon.sprite = Main.HotBar.selectedSprite;
                if (item.IsNotNull() && item.IsEquip())
                {
                    item.Use();
                }
            }
            else
            {
                bgIcon.sprite = Main.HotBar.normalSprite;
            }
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
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void SetItem(Item item)
    {
        if (this.item.IsNotNull()) return;

        this.item = item;
        itemIcon.sprite = LoadTool.LoadSprite(item.Icon);
        itemIcon.gameObject.SetActive(true);
    }

    public Item GetItem()
    {
        return item;
    }
}
