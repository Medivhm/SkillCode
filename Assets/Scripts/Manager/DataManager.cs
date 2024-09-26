using Info;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Tools;

namespace Manager
{
    class DataManager : Singleton<DataManager>
    {
        //public Dictionary<int, EquipInfo>        EquipInfos;
        //public Dictionary<int, CreatureInfo>        CreatureInfos;
        //public Dictionary<int, MapInfo>          MapInfos;
        //public Dictionary<int, BulletInfo>       BulletInfos;
        public Dictionary<int, ItemInfo>         ItemInfos;
        public Dictionary<int, WeaponInfo>       WeaponInfos;
        //public Dictionary<int, ActivitiesUIInfo> ActivitiesUIInfos;
        //public Dictionary<int, NPCInfo>          NPCInfos;
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
            //// 装备表导入
            //json = LoadTool.LoadJson("tb_equip");
            //List<EquipInfo> equipInfo = JsonConvert.DeserializeObject<List<EquipInfo>>(json);
            //EquipInfos = new Dictionary<int, EquipInfo>();
            //for (int i = 0; i < equipInfo.Count; i++)
            //{
            //    EquipInfos.Add(equipInfo[i].ID, equipInfo[i]);
            //}
            //yield return null;

            //// 敌人表导入
            //json = LoadTool.LoadJson("tb_Creature");
            //List<CreatureInfo> CreatureInfo = JsonConvert.DeserializeObject<List<CreatureInfo>>(json);
            //CreatureInfos = new Dictionary<int, CreatureInfo>();
            //for (int i = 0; i < CreatureInfo.Count; i++)
            //{
            //    CreatureInfos.Add(CreatureInfo[i].ID, CreatureInfo[i]);
            //}
            //yield return null;

            //// 地图表导入
            //json = LoadTool.LoadJson("tb_map");
            //List<MapInfo> mapInfo = JsonConvert.DeserializeObject<List<MapInfo>>(json);
            //MapInfos = new Dictionary<int, MapInfo>();
            //for (int i = 0; i < mapInfo.Count; i++)
            //{
            //    MapInfos.Add(mapInfo[i].ID, mapInfo[i]);
            //}
            //yield return null;

            //// 子弹表导入
            //json = LoadTool.LoadJson("tb_bullet");
            //List<BulletInfo> bulletInfo = JsonConvert.DeserializeObject<List<BulletInfo>>(json);
            //BulletInfos = new Dictionary<int, BulletInfo>();
            //for (int i = 0; i < bulletInfo.Count; i++)
            //{
            //    BulletInfos.Add(bulletInfo[i].ID, bulletInfo[i]);
            //}
            //yield return null;

            // Item表导入
            json = LoadTool.LoadJson("tb_item");
            List<ItemInfo> itemInfo = JsonConvert.DeserializeObject<List<ItemInfo>>(json);
            ItemInfos = new Dictionary<int, ItemInfo>();
            for (int i = 0; i < itemInfo.Count; i++)
            {
                ItemInfos.Add(itemInfo[i].ID, itemInfo[i]);
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

            //// ActivitiesUI表导入
            //json = LoadTool.LoadJson("tb_activitiesui");
            //List<ActivitiesUIInfo> activitiesUIInfos = JsonConvert.DeserializeObject<List<ActivitiesUIInfo>>(json);
            //ActivitiesUIInfos = new Dictionary<int, ActivitiesUIInfo>();
            //for (int i = 0; i < activitiesUIInfos.Count; i++)
            //{
            //    ActivitiesUIInfos.Add(activitiesUIInfos[i].ID, activitiesUIInfos[i]);
            //}
            //yield return null;

            //// NPC表导入
            //json = LoadTool.LoadJson("tb_npc");
            //List<NPCInfo> npcInfos = JsonConvert.DeserializeObject<List<NPCInfo>>(json);
            //NPCInfos = new Dictionary<int, NPCInfo>();
            //for (int i = 0; i < npcInfos.Count; i++)
            //{
            //    NPCInfos.Add(npcInfos[i].ID, npcInfos[i]);
            //}
            //yield return null;

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
