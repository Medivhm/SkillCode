
using Constant;
using Info;
using Manager;
using UnityEngine;

namespace QEntity
{
    public class CreatureEntity : UnitEntity
    {
        CreatureInfo info;

        public CreatureInfo Info
        {
            get { return info; }
        }

        public virtual void Init(CreatureInfo info, string name)
        {
            HoldRange = CreatureConstant.DefaultHoldRange;
            basicAttr = new BasicAttr(CreatureConstant.DefaultEntityAttr);
            closeAttackAttr = new AttackAttr(CreatureConstant.DefaultAttackAttr);
            farAttackAttr = new AttackAttr(CreatureConstant.DefaultAttackAttr);
            magicAttackAttr = new AttackAttr(CreatureConstant.DefaultAttackAttr);
            this.info = info;
            BloodMax = info.blood;
            Blood = info.blood;
            Name = name ?? info.name;
            InitHUD();
        }

        public virtual void InitHUD()
        {
            hud = HUDManager.AddHUD(this.transform, Name, BloodMax, Blood);
        }

        public override void Dead()
        {
            base.Dead();
            GUI.ActionInfoLog(string.Format("你打败了 <color=red>[{0}]</color>", Name));
        }

        public override void Destroy()
        {
            //CreatureManager.Remove(this);
            base.Destroy();
        }
    }
}
