
using Constant;
using Info;
using Manager;
using UnityEngine;

namespace QEntity
{
    public class EnemyEntity : UnitEntity
    {
        EnemyInfo info;

        public EnemyInfo Info
        {
            get { return info; }
        }

        public virtual void Init(EnemyInfo info, string name)
        {
            HoldRange = EnemyConstant.DefaultHoldRange;
            basicAttr = new BasicAttr(EnemyConstant.DefaultEntityAttr);
            closeAttackAttr = new AttackAttr(EnemyConstant.DefaultAttackAttr);
            farAttackAttr = new AttackAttr(EnemyConstant.DefaultAttackAttr);
            magicAttackAttr = new AttackAttr(EnemyConstant.DefaultAttackAttr);
            this.info = info;
            BloodMax = info.blood;
            Blood = info.blood;
            Name = name ?? info.name;
            InitHUD();
        }

        public virtual void InitHUD()
        {
            //hud = HUDManager.AddHUD(this, Name, BloodMax, Blood);
        }

        public override void Dead()
        {
            base.Dead();
            GUI.ActionInfoLog(string.Format("你打败了 <color=red>[{0}]</color>", Name));
        }

        public override void Destroy()
        {
            //EnemyManager.Remove(this);
            base.Destroy();
        }
    }
}
