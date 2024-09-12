using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class HotBarItem : MonoBehaviour
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
