using Info;
using Manager;
using QEntity;

public class Weapon : Item, IUseable
{
    public Weapon(int weaponID, ItemType itemType, From from, int num, UnitEntity owner = null)
        : this(
              ItemManager.GetWeaponInfo(weaponID),
              itemType,
              from,
              num,
              owner)
    {
    }

    public Weapon(WeaponInfo weaponInfo, ItemType itemType, From from, int num, UnitEntity owner = null)
        : base(
            itemType,
            from,
            num,
            new ItemInfo()
            {
                ID = weaponInfo.ID,
                type = weaponInfo.type,
                name = weaponInfo.name,
                color = weaponInfo.color,
                overlap = weaponInfo.overlap,
                prefab = weaponInfo.prefab,
                icon = weaponInfo.icon,
                desc = weaponInfo.desc,
            },
            owner)
    {

    }

    public bool Dress()
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

    public bool Use()
    {
        
        return true;
    }
}
