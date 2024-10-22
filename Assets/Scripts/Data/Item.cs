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

public enum ItemType
{
    Prop,
    Weapon,
}

public class ItemInfo
{
    public int ID;
    public int type;
    public string name;
    public int color;
    public int overlap;
    public string prefab;
    public string icon;
    public string desc;
}

public class Item
{
    // 背包，快捷栏，身上，地面
    public ItemType ItemType;
    public From From;
    public ItemInfo Info;
    public UnitEntity Owner;

    public int IndexMarker;
    public int Num;

    private Item() { }

    public Item(ItemType itemType, From from, int num, int id, int type, string name, int color, int overlap, string prefab, string icon, string desc, UnitEntity owner = null)
    {
        ItemType = itemType;
        From = from;
        Num = num;
        IndexMarker = -1;
        Owner = owner;

        Info = new ItemInfo
        {
            ID = id,
            type = type,
            name = name,
            color = color,
            overlap = overlap,
            prefab = prefab,
            icon = icon,
            desc = desc,
        };
    }

    public Item(ItemType itemType, From from, int num, ItemInfo info, UnitEntity owner = null) : this(itemType, from, num, info.ID, info.type, info.name, info.color, info.overlap, info.prefab, info.icon, info.desc, owner)
    {
    }

    public int ItemID => Info.ID;

    public int Type => Info.type;

    public string Name => Info.name;

    public string PrefabPath => Info.prefab;

    public Color Color => ColorConstant.ColorDefine[Info.color];

    public int Overlap => Info.overlap;

    public string Icon => Info.icon;

    public string Desc => Info.desc;

    public Item DeepCopyItem()
    {
        Item i = new Item();
        i.ItemType = this.ItemType;
        i.From = this.From;
        i.IndexMarker = this.IndexMarker;
        i.Num = this.Num;
        i.Owner = this.Owner;
        i.Info = this.Info;
        return i;
    }
}
