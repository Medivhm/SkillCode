using Constant;
using Info;
using Manager;
using UnityEngine;

public enum From
{
    Bag,
    QuickCell,
    Other,
}

public class Item : Useable
{
    public int Num;
    // 背包，快捷栏，地面
    public From From;
    public ItemInfo Info;

    private Item()
    {

    }

    public Item(int itemID, From from, int num) : this(DataManager.Instance.ItemInfos[itemID], from, num)
    {
    }

    public Item(ItemInfo info, From from, int num)
    {
        Info = info;
        From = from;
        Num = num;
    }

    public int ItemID
    {
        get => Info.ID;
    }

    public int Type
    {
        get => Info.type;
    }

    public string Name
    {
        get => Info.name;
    }

    // "#FFFFFF"
    public Color Color
    {
        get => ColorConstant.ColorDefine[Info.color];
    }

    public int Overlap
    {
        get => Info.overlap;
    }

    public override string Icon
    {
        get => Info.icon;
    }

    public string Desc
    {
        get => Info.desc;
    }

    public Item CopyItem()
    {
        Item i = new Item();
        i.Num = this.Num;
        i.Info = this.Info;
        i.From = this.From;
        return i;
    }

    public override bool Use()
    {
        return true;
    }
}
