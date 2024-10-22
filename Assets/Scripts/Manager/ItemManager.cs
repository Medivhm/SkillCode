using QEntity;
using Info;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    class ItemManager : Singleton<ItemManager>
    {
        public void Init()
        {

        }

        public static Prop CreateProp(int propId, int num = 1, From from = From.Other, UnitEntity owner = null)
        {
            if (GetPropInfo(propId).IsNotNull())
            {
                return new Prop(propId, ItemType.Prop, from, num, owner);
            }
            return null;
        }

        public static PropInfo GetPropInfo(int itemID)
        {
            PropInfo propInfo;
            if (!DataManager.Instance.PropInfos.TryGetValue(itemID, out propInfo))
            {
                DebugTool.Error("ItemID 错误");
                return null;
            }
            return propInfo;
        }

        public static Weapon CreateWeapon(int weaponID, int num = 1, From from = From.Other, UnitEntity owner = null)
        {
            if (GetWeaponInfo(weaponID).IsNotNull())
            {
                return new Weapon(weaponID, ItemType.Prop, from, num, owner);
            }
            return null;
        }

        public static WeaponInfo GetWeaponInfo(int weaponID)
        {
            WeaponInfo weaponInfo;
            if (!DataManager.Instance.WeaponInfos.TryGetValue(weaponID, out weaponInfo))
            {
                DebugTool.Error("WeaponID 错误");
                return null;
            }
            return weaponInfo;
        }
    }
}
