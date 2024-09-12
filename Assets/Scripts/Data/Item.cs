using Constant;
using Info;
using Manager;
using QEntity;
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

    public int Type
    {
        get { return Info.type; }
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

    public bool IsEquip()
    {
        if (Type == 3 || Type == 4 || Type == 5)
        {
            return true;
        }
        return false;
    }

    public void Use()
    {
        if (IsEquip())
        {
            string proName = "";
            if(Type == 3)
            {
                proName = "射手";
                Main.MainPlayerCtrl.Profession = Profession.Shooter;
            }
            else if (Type == 4)
            {
                proName = "法师";
                Main.MainPlayerCtrl.Profession = Profession.Wizard;
            }
            else if (Type == 5)
            {
                proName = "战士";
                Main.MainPlayerCtrl.Profession = Profession.Fighter;
            }
            GUI.ActionInfoLog(string.Format("穿上装备 [{0}] 现在的职业是 [{1}]", Name, proName));
        }
    }
}
