using Constant;
using Info;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum ItemFrom
{
    Bag,
    QuickCell,
    Other,
}

public class Item
{
    public int Num;
    // 背包，快捷栏，地面
    public ItemFrom From;
    public ItemInfo Info;

    private Item()
    {

    }

    public Item(int itemID, ItemFrom from, int num) : this(DataManager.Instance.ItemInfos[itemID], from, num)
    {
    }

    public Item(ItemInfo info, ItemFrom from, int num)
    {
        Info = info;
        From = from;
        Num = num;
    }

    public int ItemID
    {
        get { return Info.ID; }
    }

    public string Name
    {
        get { return Info.name; }
    }

    // "#FFFFFF"
    public Color Color
    {
        get { return ColorConstant.ColorDefine[Info.color]; }
    }

    public int Overlap
    {
        get { return Info.overlap; }
    }

    public string Icon
    {
        get { return Info.icon; }
    }

    public string Desc
    {
        get { return Info.desc; }
    }

    public Item CopyItem()
    {
        Item i = new Item();
        i.Num = this.Num;
        i.Info = this.Info;
        i.From = this.From;
        return i;
    }
}
