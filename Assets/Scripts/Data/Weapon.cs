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

public class Weapon : Useable
{
    public int Num;
    public From From;
    public WeaponInfo Info;

    private Weapon()
    {

    }

    public Weapon(int itemID, From from, int num) : this(DataManager.Instance.WeaponInfos[itemID], from, num)
    {
    }

    public Weapon(WeaponInfo info, From from, int num)
    {
        Info = info;
        From = from;
        Num = num;
    }

    public int WeaponID
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

    public bool CanDestroy
    {
        get => Info.canDestroy;
    }

    public Weapon CopyWeapon()
    {
        Weapon w = new Weapon();
        w.Num = this.Num;
        w.Info = this.Info;
        w.From = this.From;
        return w;
    }

    public override bool Use()
    {
        string proName = "";
        if (Type == 1)
        {
            proName = "射手";
            Main.MainPlayerCtrl.Profession = Profession.Shooter;
        }
        else if (Type == 2)
        {
            proName = "法师";
            Main.MainPlayerCtrl.Profession = Profession.Wizard;
        }
        else if (Type == 3)
        {
            proName = "战士";
            Main.MainPlayerCtrl.Profession = Profession.Fighter;
        }
        GUI.ActionInfoLog(string.Format("穿上装备 [{0}] 现在的职业是 [{1}]", Name, proName));
        return true;
    }
}
