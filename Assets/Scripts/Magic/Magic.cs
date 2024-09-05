using Constant;
using Manager;
using QEntity;
using System;
using Tools;
using UnityEngine;

namespace Magic
{
    public partial class Magic
    {
        public static Magic GetAMagic()
        {
            return new Magic();
        }

        public void SetCarrier(Magic magic, int carrierID, int fxID)
        {
            magic.carrierID = carrierID;
            magic.fxID = fxID;
        }

        public void SetMagicSkill(Magic magic, MagicSkill magicSkill)
        {
            magicSkill.DoChange(magic);
        }

        public void SetMagicSkill(Magic magic, int magicSkillID)
        {
            SetMagicSkill(magic, MagicSkillManager.GetMagicSkillByID(magicSkillID));
        }

        public void Use(Vector3 startPos, Vector3 shootDir)
        {
            Shoot(startPos, shootDir);
        }
    }

    public partial class Magic
    {

        public int num;
        public float power;
        public float scale;
        public float speedMult;
        public int carrierID;
        public int fxID;
        public bool traceEnemy;
        public bool lockAtFirst;

        private Magic()
        {
            Reset();
        }

        void Reset()
        {
            num = 1;
            power = 1f;
            scale = 1f;
            speedMult = 1f;
            carrierID = -1;
            fxID = -1;
            traceEnemy = false;
            lockAtFirst = false;
        }

        // return: 是否成功发射
        bool Shoot(Vector3 startPos, Vector3 shootDir)
        {
            if(carrierID < 0)
            {
                return false;
            }

            for (int i = 0; i < num; i++)
            {
                GameObject go = LoadTool.LoadCarrier(CarrierManager.GetCarrierInfoByID(carrierID).carrierPath);
                CarrierEntity carrier = go.AddComponent<CarrierEntity>();
                if (i == 0)
                {
                    carrier.Init(80f,
                        startPos, 5f, shootDir, null, go, Main.MainPlayerCtrl,
                        power,
                        speedMult,
                        scale);
                }
                else
                {
                    carrier.Init(80f,
                        new Vector3(startPos.x + Util.GetRandom(1, 5), startPos.y + Util.GetRandom(1, 5), startPos.z),
                        5f, shootDir, null, go, Main.MainPlayerCtrl,
                        power,
                        speedMult,
                        scale);
                }
            }
            return true;
        }
    }
}
