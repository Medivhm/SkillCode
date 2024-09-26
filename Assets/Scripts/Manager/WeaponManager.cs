using QEntity;
using Info;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    class WeaponManager : Singleton<WeaponManager>
    {
        public void Init()
        {

        }

        static WeaponInfo weaponInfo;
        public static Weapon CreateWeapon(int weaponID)
        {
            if (GetWeaponInfo(weaponID).IsNotNull())
            {
                return new Weapon(weaponInfo, From.Other, 1);
            }
            return null;
        }

        public static WeaponInfo GetWeaponInfo(int weaponID)
        {
            if (!DataManager.Instance.WeaponInfos.TryGetValue(weaponID, out weaponInfo))
            {
                DebugTool.Error("WeaponID 错误");
                return null;
            }
            return weaponInfo;
        }
    }
}
