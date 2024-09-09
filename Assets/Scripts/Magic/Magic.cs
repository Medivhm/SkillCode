using Manager;
using QEntity;
using Tools;
using UnityEngine;

namespace Magic
{
    public partial class Magic
    {
        UnitEntity Owner;

        public static Magic GetAMagic(UnitEntity owner)
        {
            return new Magic(owner);
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

        public void Use(Vector3 startPos, Vector3 shootDir, Vector3? hitPoint)
        {
            Shoot(startPos, shootDir, hitPoint);
        }
    }

    public partial class Magic
    {
        public int num;
        public float power;
        public float scale;
        public float speedMult;
        public float liveTimeMult;
        public int carrierID;
        public int fxID;
        public bool traceCreature;
        public bool lockAtFirst;
        public bool autoSearch;
        public bool shootAtFirst;

        private Magic(UnitEntity owner)
        {
            this.Owner = owner;
            Reset();
        }

        void Reset()
        {
            num = 1;
            power = 1f;
            scale = 1f;
            speedMult = 1f;
            liveTimeMult = 1f;
            carrierID = -1;
            fxID = -1;
            traceCreature = false;            // 发射前自动索敌，最近攻击-》距离最近-》null
            lockAtFirst = false;              // 定点
            autoSearch = false;               // 飞行中自动索敌
        }

        // return: 是否成功发射
        bool Shoot(Vector3 startPos, Vector3 shootDir, Vector3? hitPoint)
        {
            UnitEntity attackTarget = null;
            // 索敌
            if (traceCreature)
            {
                if (Owner.lastAttack.IsNotNull())
                {
                    attackTarget = Owner.lastAttack;
                }
                else
                {
                    attackTarget = Owner.GetClosestOtherCamp();
                }
                if (attackTarget.IsNull())
                {
                    GUI.ActionInfoLog("找不到施法对象");
                    return false;
                }
            }
            // 发射初始化pos
            if (lockAtFirst)
            {
                if (attackTarget.IsNotNull())
                {
                    startPos = attackTarget.Position;
                }
                else if(hitPoint.IsNotNull())
                {
                    startPos = (Vector3)hitPoint;
                }
                else
                {
                    startPos = startPos + shootDir.normalized * 400;      // 施法最远距离
                }
                if (!(traceCreature || autoSearch))
                {
                    speedMult = 0f;
                }
            }
            if(carrierID < 0)
            {
                return false;
            }

            for (int i = 0; i < num; i++)
            {
                GameObject go = LoadTool.LoadCarrier(CarrierManager.GetCarrierInfoByID(carrierID).carrierPath);

                CarrierEntity carrier = go.GetOrAddComponent<CarrierEntity>();
                if (i == 0)
                {
                    carrier.Init(CarrierManager.GetCarrierInfoByID(carrierID),
                        startPos,
                        shootDir,
                        attackTarget.IsNull() ? null : attackTarget.transform,
                        go, 
                        Owner,
                        power,
                        speedMult,
                        liveTimeMult,
                        scale,
                        autoSearch,
                        shootAtFirst);
                }
                else
                {
                    carrier.Init(CarrierManager.GetCarrierInfoByID(carrierID),
                        new Vector3(startPos.x + Util.GetRandom(1, 5), startPos.y + Util.GetRandom(1, 5), startPos.z),
                        shootDir, 
                        attackTarget.IsNull() ? null : attackTarget.transform, 
                        go, 
                        Owner,
                        power,
                        speedMult,
                        liveTimeMult,
                        scale,
                        autoSearch,
                        shootAtFirst);
                }
            }
            return true;
        }
    }
}
