using Info;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Tools;

namespace Manager
{
    class DataManager : Singleton<DataManager>
    {
        public Dictionary<int, PropInfo>         PropInfos;
        public Dictionary<int, WeaponInfo>       WeaponInfos;

        public BagInfo                           BagInfos;
        public Dictionary<int, FxInfo>           FxInfos;
        public Dictionary<int, MagicSkillInfo>   MagicSkillInfos;
        public Dictionary<int, CarrierInfo>      CarrierInfos;


        public IEnumerator Init()
        {
            yield return InitEx();
        }

        string json;
        IEnumerator InitEx()
        {
            #region // Item
            //////////////////////////////////////////     ITEM 相关         //////////////////////////////////////////////////////////////////
            // Prop表导入
            json = LoadTool.LoadJson("tb_prop");
            List<PropInfo> propInfo = JsonConvert.DeserializeObject<List<PropInfo>>(json);
            PropInfos = new Dictionary<int, PropInfo>();
            for (int i = 0; i < propInfo.Count; i++)
            {
                PropInfos.Add(propInfo[i].ID, propInfo[i]);
            }
            yield return null;

            // Weapon表导入
            json = LoadTool.LoadJson("tb_weapon");
            List<WeaponInfo> weaponInfo = JsonConvert.DeserializeObject<List<WeaponInfo>>(json);
            WeaponInfos = new Dictionary<int, WeaponInfo>();
            for (int i = 0; i < weaponInfo.Count; i++)
            {
                WeaponInfos.Add(weaponInfo[i].ID, weaponInfo[i]);
            }
            yield return null;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion


            // Bag表导入
            json = LoadTool.LoadJson("tb_bag");
            BagInfos = JsonConvert.DeserializeObject<List<BagInfo>>(json)[0];
            yield return null;

            // Fx表导入
            json = LoadTool.LoadJson("tb_fx");
            List<FxInfo> fxInfos = JsonConvert.DeserializeObject<List<FxInfo>>(json);
            FxInfos = new Dictionary<int, FxInfo>();
            for (int i = 0; i < fxInfos.Count; i++)
            {
                FxInfos.Add(fxInfos[i].ID, fxInfos[i]);
            }
            yield return null;

            // MagicSkill表导入
            json = LoadTool.LoadJson("tb_magicskill");
            List<MagicSkillInfo> magicSkillInfos = JsonConvert.DeserializeObject<List<MagicSkillInfo>>(json);
            MagicSkillInfos = new Dictionary<int, MagicSkillInfo>();
            for (int i = 0; i < magicSkillInfos.Count; i++)
            {
                MagicSkillInfos.Add(magicSkillInfos[i].ID, magicSkillInfos[i]);
            }
            yield return null;

            // Carrier表导入
            json = LoadTool.LoadJson("tb_carrier");
            List<CarrierInfo> carrierInfos = JsonConvert.DeserializeObject<List<CarrierInfo>>(json);
            CarrierInfos = new Dictionary<int, CarrierInfo>();
            for (int i = 0; i < carrierInfos.Count; i++)
            {
                CarrierInfos.Add(carrierInfos[i].ID, carrierInfos[i]);
            }
            yield return null;
        }
    }
}   
