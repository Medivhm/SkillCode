using Constant;
using Info;
using Manager;
using QEntity;
using UnityEngine;

public enum From
{
    Bag,
    HotBar,
    Body,
    Other,
}

public class Item : Useable
{
    public int Num;
    // 背包，快捷栏，地面
    public From From;
    public int indexMarker;
    public ItemInfo Info;
    public UnitEntity Owner;

    private Item()
    {

    }

    public Item(int itemID, From from, int num, UnitEntity owner = null) : this(DataManager.Instance.ItemInfos[itemID], from, num, owner)
    {
    }

    public Item(ItemInfo info, From from, int num, UnitEntity owner = null)
    {
        Info = info;
        From = from;
        Num = num;
    }

    public int ItemID => Info.ID;

    public int Type => Info.type;

    public string Name => Info.name;

    public string PrefabPath => Info.prefab;

    // "#FFFFFF"
    public Color Color => ColorConstant.ColorDefine[Info.color];

    public int Overlap => Info.overlap;

    public override string Icon => Info.icon;

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
